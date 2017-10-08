using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject cameraPosition;
    private float interpolationValue = 0.1f;

	void Start () {
        cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition");
	}
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.transform.position, interpolationValue);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraPosition.transform.rotation, interpolationValue);
	}
}
