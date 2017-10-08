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
    private Text scoreText;

    private float onSpawnScore = 19;
    public bool isDead = false;

    private void Start()
    {
        lastSpawned = gameObject;
        Spawn();
        Spawn();
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
            Spawn();
        }

        time += Time.deltaTime;
        if (time>3)
        {
            time -= 3;
            Spawn();
        }

        score += Time.deltaTime * 5;
        scoreText.text = (int) score + "";
	}

    void Spawn()
    {
        lastSpawned = Instantiate(body, lastSpawned.transform.position, lastSpawned.transform.rotation);
        lastSpawned.GetComponent<BodyFollower>().bodyId = lastId;
        lastId++;
        score += onSpawnScore; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding with " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<BodyFollower>() != null)
        {
            if (collision.gameObject.GetComponent<BodyFollower>().bodyId > 2)
            {
                isDead = true;
            }
        }
    }
}
