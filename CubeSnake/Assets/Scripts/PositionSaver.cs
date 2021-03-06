﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSaver : MonoBehaviour {

    public static List<Vector3> positionList = new List<Vector3>();
    public static List<Quaternion> rotationList = new List<Quaternion>();

    private int idDelay = 10;
    public int bodyId;
    public static int bodyCount = 0;

    void FixedUpdate () {
        if (gameObject.GetComponent<SpawningBodies>() != null)
        {
            if (!gameObject.GetComponent<SpawningBodies>().isDead)
            {
                positionList.Insert(0, transform.position);
                rotationList.Insert(0, transform.rotation);
                if (rotationList.Count > idDelay * (bodyCount))
                {
                    rotationList.RemoveAt(rotationList.Count - 1);
                    positionList.RemoveAt(positionList.Count - 1);
                }
            }
        }
         

        if (gameObject.tag == "Body")
        {
            transform.position = positionList[Mathf.Clamp((1 + bodyId) * idDelay, 0, positionList.Count-1)];
            transform.rotation = rotationList[Mathf.Clamp((1 + bodyId) * idDelay, 0, rotationList.Count-1)];
        }
	}
}
