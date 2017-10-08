using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 3f;
    public float turnSpeed = 250f;
    public float sideTurnSpeed = 70f;
    public float onGroundDistance = 1f;

    private float turningSafety = 0.1f;
    private float size;


    public float CubeSize = 5;

    private bool startedTurning = false;
    RaycastHit hit;
    RaycastHit previousHit;
    private Vector3 edgeVector;

    private float positionLerp = 0.1f;
    private float rotationLerp = 0.1f;

    private Touch newestTouch;
	
	void Start () {
        size = transform.lossyScale.x;
	}
	
	void Update () {

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
                transform.Rotate(0, -sideTurnSpeed * Time.deltaTime, 0);
            } else
            {
                transform.Rotate(0, sideTurnSpeed * Time.deltaTime, 0);
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0,-sideTurnSpeed * Time.deltaTime,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, sideTurnSpeed * Time.deltaTime, 0);
        }

        if (RotateParallelToGround())
        {
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime, Space.World);
        }
	}

    private bool RotateParallelToGround()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, onGroundDistance, 1<<8) && Mathf.Abs(Vector3.Dot(hit.normal, transform.forward)) < turningSafety)
        {
            transform.position = Vector3.Lerp(transform.position, hit.point + size/2 * transform.up, positionLerp);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, rotationLerp);
            startedTurning = false;
            return true;
        } else
        {
            edgeVector = FindEdgeVector(transform.forward, transform.up, transform.right);
            transform.RotateAround(transform.position - transform.up * (size/2 + turningSafety), edgeVector, turnSpeed * Time.deltaTime);
            startedTurning = true;
        }
        return false;
    }

    private Vector3 FindEdgeVector(Vector3 forward, Vector3 up, Vector3 right)
    {
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