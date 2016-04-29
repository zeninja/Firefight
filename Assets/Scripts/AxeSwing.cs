using UnityEngine;
using System.Collections;

public class AxeSwing : MonoBehaviour {

	GameObject axe;
	public float activeDuration;

	// Use this for initialization
	void Start () {
		axe = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Activate();
		}
	}

	public void Activate() {
		axe.GetComponent<SpriteRenderer>().enabled = true;
		axe.transform.localPosition = new Vector2(.3f, 1);
//		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector2(-.5f, 0), "time", activeDuration, "easeType", EaseType.easeOutExpo, "isLocal", true));
		axe.MoveTo((Vector2)transform.position + new Vector2(-.3f, 1), activeDuration/5, activeDuration * 3/5, EaseType.easeOutExpo);
		Invoke("Deactivate", activeDuration);
	}

	void Deactivate() {
		axe.GetComponent<SpriteRenderer>().enabled = false;
	}
}
