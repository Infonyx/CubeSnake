using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    private float movementSpeed = 0;
    public float turnSpeed = 250f;
    public float sideTurnSpeed = 200f;
    public float onGroundDistance = 1f;

    private float turningSafety = 0.1f;
    private float size;

    private bool startedTurning = false;
    private bool inTurning = false;
    RaycastHit hit;
    RaycastHit previousHit;
    private Vector3 edgeVector;

    private float positionLerp = 0.1f;
    private float rotationLerp = 0.1f;

    private Touch newestTouch;

    private Vector3 realPosition;

    private float tilt = 1;

    private Vector3 lastPlaneNormal = Vector3.up;
	
	void Start () {
        size = transform.lossyScale.x;
        realPosition = transform.position;
        StartCoroutine(Accelerate());
	}

    private IEnumerator Accelerate()
    {
        while (movementSpeed < 3)
        {
            movementSpeed += Time.deltaTime;
            yield return null;
        }
        movementSpeed = 3;
    }


	void FixedUpdate () {

        if (gameObject.GetComponent<SpawningBodies>().isDead)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    SceneManager.LoadScene(0);
                }
            }
            return;
        }

        if (Input.touchCount != 0)
        {
            if (Screen.width/2 - GetLatestTouch().position.x > 0)
            {
                transform.Rotate(0, -sideTurnSpeed * Time.fixedDeltaTime, 0);
            } else
            {
                transform.Rotate(0, sideTurnSpeed * Time.fixedDeltaTime, 0);
            }
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0,-sideTurnSpeed * Time.fixedDeltaTime,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, sideTurnSpeed * Time.fixedDeltaTime, 0);
        }
        
        if (RotateParallelToGround())
        {
            transform.Translate(transform.forward * movementSpeed * Time.fixedDeltaTime, Space.World);
        }
	}

    private bool RotateParallelToGround()
    {
        RaycastHit top, bottom;

        Physics.Raycast(transform.position + 0.5f * (transform.forward), -transform.up - transform.forward * 0.3f, out top, 50, 1 << 8);
        Physics.Raycast(transform.position + 0.5f * (-transform.forward), -transform.up + transform.forward * 0.3f, out bottom, 50, 1 << 8);
        
        if (Physics.Raycast(transform.position, -transform.up, out hit, onGroundDistance, 1<<8) //&& Mathf.Abs(Vector3.Dot(hit.normal, transform.forward)) < turningSafety
            //top.normal == bottom.normal
            && !inTurning
            )
        {
            transform.position = Vector3.Lerp(transform.position, hit.point + size/2 * transform.up, positionLerp);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, rotationLerp);
            //startedTurning = true;
            return true;
        } else
        {
            //edgeVector = FindEdgeVector();
            if (!inTurning)//startedTurning)
            {
                StartCoroutine(Rotate());
            }
            //transform.RotateAround(transform.position - transform.up * (size/2 + turningSafety), edgeVector, turnSpeed * Time.deltaTime);
            //startedTurning = false;
            return false;
        }
    }

    IEnumerator Rotate()
    {
        inTurning = true;
        edgeVector = FindEdgeVector();
        if (edgeVector == Vector3.zero)
        {
            inTurning = false;
            startedTurning = true;
            yield break;
        }
        for (float i = 0; i<90; i+=turnSpeed * Time.deltaTime)
        {
            transform.RotateAround(transform.position - transform.up * (size / 2 + turningSafety), edgeVector, turnSpeed * Time.deltaTime);
            yield return null;
        }
        inTurning = false;
        startedTurning = true;
    }

    private Vector3 FindEdgeVector()
    {
        #region EdgeFinder1

        /*
 
        Vector3 edge;

        if (Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.y) && Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.z))
        {
            if (Mathf.Abs(transform.position.y) > Mathf.Abs(transform.position.z))
            {
                edge = new Vector3(0, 0, 1);
            } else
            {
                edge = new Vector3(0, 1, 0);
            }
        } else if (Mathf.Abs(transform.position.y) > Mathf.Abs(transform.position.x) && Mathf.Abs(transform.position.y) > Mathf.Abs(transform.position.z))
        {
            if (Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.z))
            {
                edge = new Vector3(0, 0, 1);
            }
            else
            {
                edge = new Vector3(1, 0, 0);
            }
        } else // if (transform.position.z > transform.position.x && transform.position.z > transform.position.y)
        {
            if (Mathf.Abs(transform.position.x) > Mathf.Abs(transform.position.y))
            {
                edge = new Vector3(0, 1, 0);
            }
            else
            {
                edge = new Vector3(1, 0, 0);
            }
        }

        if (Vector3.Dot(transform.right, edge) > 0)
        {
            return edge;
        } else
        {
            return -edge;
        }

    */
        #endregion

        /*
        RaycastHit top, bottom, hRight, hLeft;
        bool hitTop, hitBottom;

        hitTop = Physics.Raycast(transform.position + 0.5f * (transform.forward), -transform.up -transform.forward * 0.3f, out top, 50, 1 << 8);
        hitBottom = Physics.Raycast(transform.position + 0.5f * (-transform.forward), -transform.up +transform.forward * 0.3f, out bottom, 50, 1 << 8);

        Debug.Log(hitTop + ":" + hitBottom);


        if (Vector3.Dot(top.normal, bottom.normal) == 0 && hitTop && hitBottom)
        {
            Debug.Log("ForwardRotation "+top.normal+" : "+bottom.normal+" ::: "+Vector3.Cross(top.normal,bottom.normal));
            return Vector3.Cross(top.normal, -bottom.normal);

        } else /*if (Physics.Raycast(transform.position + 0.5f * (transform.right), -transform.up - transform.right * 0.3f, out hRight, 50, 1 << 8)
            && Physics.Raycast(transform.position - 0.5f * (transform.right), -transform.up + transform.right * 0.3f, out hLeft, 50, 1 << 8)
            && Vector3.Dot(hRight.normal, hLeft.normal) == 0)
        {

            Debug.Log("StrafeRotation");
            return Vector3.zero; //Vector3.Cross(hRight.normal, hLeft.normal);

        } else 
        {

            Debug.Log("Fail");
            return Vector3.zero; //GetEdge(transform.forward);

        }
        */


        RaycastHit top;
        bool hitTop;

        hitTop = Physics.Raycast(transform.position + 0.5f * (transform.forward), -transform.up - transform.forward * 0.3f, out top, 50, 1 << 8);

        Debug.Log(hitTop);


        if (Vector3.Dot(top.normal, lastPlaneNormal) == 0 && hitTop)
        {
            Debug.Log("ForwardRotation " + top.normal  + " ::: " + Vector3.Cross(top.normal, lastPlaneNormal));
            Vector3 edge = Vector3.Cross(top.normal, -lastPlaneNormal);
            lastPlaneNormal = top.normal;
            return edge;

        }
        else 
        {

            Debug.Log("Fail");
            return Vector3.zero; //GetEdge(transform.forward);

        }


    }

    private Vector3 GetEdge(Vector3 guess)
    {
        float maxDotProduct = Mathf.Abs(Vector3.Dot(guess, Vector3.forward));
        Vector3 solution = Vector3.forward;

        if (Mathf.Abs(Vector3.Dot(guess, Vector3.left)) > maxDotProduct)
        {
            maxDotProduct = Mathf.Abs(Vector3.Dot(guess, Vector3.left));
            solution = Vector3.left;
        }

        if (Mathf.Abs(Vector3.Dot(guess, Vector3.up)) > maxDotProduct)
        {
            maxDotProduct = Mathf.Abs(Vector3.Dot(guess, Vector3.up));
            solution = Vector3.up;
        }

        if (Vector3.Dot(transform.forward, solution) < 0)
        {
            solution = -solution;                   //COULD BE BUGGED
        }

        Debug.Log(Vector3.Cross(transform.up, solution));
        return Vector3.Cross(transform.up, solution);

    }

    private Touch GetLatestTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                newestTouch = touch;
            }
        }

        if (Input.touchCount > 1)
        {
            return newestTouch;
        }
        else
        {
            return Input.GetTouch(0);
        }
    }    
}