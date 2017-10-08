using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawningBodies : MonoBehaviour {

    public GameObject body;
    private int lastId = 0;
    private GameObject lastSpawned;
    private float time = 0;

    private float score = 0;
    private float scoreChange = 1;
    private Text scoreText;

    private float onSpawnScore = 19;
    public bool isDead = false;

    float maxCollisionDistance = 0.5f;

    FoodController foodController;
    public List<GameObject> ObjectList = new List<GameObject>();

    private void Start()
    {
        foodController = GameObject.FindGameObjectWithTag("FoodController").GetComponent<FoodController>();
        lastSpawned = gameObject;
        Spawn(2);
        score = 0;
        scoreText = GameObject.Find("Text").GetComponent<Text>();
    }

    void Update () {
        if (isDead)
        {
            return;
        }

		if (Input.GetKeyDown(KeyCode.E))
        {
            Spawn(1);
        }

        scoreChange -= Time.deltaTime;
        score += scoreChange * Time.deltaTime;
        scoreText.text = (int) score + "";

        CheckCollision();
	}

    void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            lastSpawned = Instantiate(body, lastSpawned.transform.position, lastSpawned.transform.rotation);
            lastSpawned.GetComponent<PositionSaver>().bodyId = lastId;
            PositionSaver.bodyCount = lastId;
            if (lastId > 4)
            {
                ObjectList.Add(lastSpawned);
            }
            lastId++;

            if (scoreChange <= 0)
            {
                scoreChange = 3;
            } else
            {
                scoreChange += 2;
            }
        }
    }

    private void CheckCollision()
    {
        foreach (GameObject obj in ObjectList)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < maxCollisionDistance)
            {
                if (obj.GetComponent<BodyFollower>() != null)
                {
                    
                    isDead = true;
                    
                }
                else
                {
                    ObjectList.Remove(obj);
                    Destroy(obj);
                    Spawn(Random.Range(1, 4));
                    foodController.SpawnFood();
                }

                break;
            }
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BodyFollower>() != null)
        {
            if (collision.gameObject.GetComponent<BodyFollower>().bodyId > 4)
            {
                isDead = true;
            }
        } else if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
            Spawn(Random.Range(1, 4));
            foodController.SpawnFood();
        }
    }*/
}
