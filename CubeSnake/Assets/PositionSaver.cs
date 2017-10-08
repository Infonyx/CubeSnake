using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSaver : MonoBehaviour {

    public List<Vector3> positionList = new List<Vector3>();
    public List<Quaternion> rotationList = new List<Quaternion>();

    void Update () {
        if (!gameObject.GetComponent<SpawningBodies>().isDead)
        {
            positionList.Insert(0, transform.position);
            if (positionList.Count > 10000)
            {
                positionList.RemoveAt(9999);
            }
            rotationList.Insert(0, transform.rotation);
            if (rotationList.Count > 10000)
            {
                rotationList.RemoveAt(9999);
            }
        }   
	}
}
