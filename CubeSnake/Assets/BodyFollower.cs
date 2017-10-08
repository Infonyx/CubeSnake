using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollower : MonoBehaviour {

    public int bodyId;
    PositionSaver positions;
    private int idDelay = 10;

    private void Start()
    {
        positions = GameObject.FindGameObjectWithTag("Head").GetComponent<PositionSaver>();
    }

   /* void FixedUpdate () {
        transform.position = positions.positionList[Mathf.Clamp((1 + bodyId) * idDelay, 0, positions.positionList.Count - 1)];
        transform.rotation = positions.rotationList[Mathf.Clamp((1 + bodyId) * idDelay, 0, positions.rotationList.Count - 1)];
	}*/
}
