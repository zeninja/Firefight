using UnityEngine;
using System.Collections;

public class TreeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.gridStatus.Add(transform.position, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Activate() {
		GameManager.gridStatus.Remove(transform.position);
		GetComponent<SpriteRenderer>().color = Color.white;
	}
}
