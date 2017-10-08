using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

    public GameObject food;
    public int spawningSize = 2;
    public int Sides = 6;
    private Vector2 planePosition;
    private Vector3 spawnPoint;
    private float distance = 2.6f;
    private Vector3 rotation;
    private SpawningBodies spawner;
    

	void Start () {
        spawner = GameObject.FindGameObjectWithTag("Head").GetComponent<SpawningBodies>();
        StartCoroutine(FoodIncrease());
        
        Debug.Log(spawner);
	}

    /*   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnFood();
        }
    } */

    IEnumerator FoodIncrease()
    {
        SpawnFood();
        yield return new WaitForSeconds(30f);
        SpawnFood();
    }

    public void SpawnFood()
    {
        planePosition = new Vector2(Random.Range(-spawningSize, spawningSize), Random.Range(-spawningSize, spawningSize));

        switch (Random.Range(0,6))
        {
            case 0:
                spawnPoint = new Vector3(planePosition.x,planePosition.y,distance);
                rotation = new Vector3(90, 0, 0);
                break;
            case 1:
                spawnPoint = new Vector3(planePosition.x,planePosition.y,-distance);
                rotation = new Vector3(-90, 0, 0);
                break;
            case 2:
                spawnPoint = new Vector3(planePosition.x,distance,planePosition.y);
                rotation = new Vector3(0, 0, 0);
                break;
            case 3:
                spawnPoint = new Vector3(planePosition.x,-distance,planePosition.y);
                rotation = new Vector3(180, 0, 0);
                break;
            case 4:
                spawnPoint = new Vector3(distance,planePosition.x,planePosition.y);
                rotation = new Vector3(0, 0, -90);
                break;
            default:
                spawnPoint = new Vector3(-distance,planePosition.x,planePosition.y);
                rotation = new Vector3(0, 0, 90);
                break;
        }

        GameObject lastSpawned = (GameObject)Instantiate(food, spawnPoint, Quaternion.Euler(rotation));
        spawner.ObjectList.Add(lastSpawned);
    }
}
