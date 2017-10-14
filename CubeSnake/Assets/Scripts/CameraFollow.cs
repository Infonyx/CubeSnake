using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject cameraPosition;
    private float interpolationValue = 0.01f;
    private float time = 0;

	void Start () {
        cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition");
        StartCoroutine(Accelerate());
	}
	
    private IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(2f);
        interpolationValue = 0.05f;
    }

	void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.transform.position, interpolationValue);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraPosition.transform.rotation, interpolationValue);
	}
}
