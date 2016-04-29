using UnityEngine;
using System.Collections;

public class ExtinguisherSmoke : MonoBehaviour {

	public float activeDuration = .25f;
	public float scaleDownDuration = .15f;

	// Use this for initialization
	void Start () {
		Invoke("ScaleDown", activeDuration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ScaleDown() {
		gameObject.ScaleTo(Vector3.zero, scaleDownDuration, 0);
		Invoke("Die", scaleDownDuration);
	}

	void Die() {
		Destroy(gameObject);
	}
}
