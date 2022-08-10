using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	float XOffset;
	float ZOffset;

	Quaternion rot;

	void Start(){

		XOffset = transform.position.x - target.position.x;
		ZOffset = transform.position.z - target.position.z;

	}

	void Update () {

		transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x+XOffset,transform.position.y,target.position.z+ZOffset),Time.deltaTime * 20f);

	}
}