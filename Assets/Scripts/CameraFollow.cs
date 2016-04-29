using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float lerpSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.transform.position + new Vector3(0, 0, -10);
		targetPos = new Vector3(0, targetPos.y, targetPos.z);
		transform.position = targetPos;
	}
}
