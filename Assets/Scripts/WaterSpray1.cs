using UnityEngine;
using System.Collections;

public class WaterSpray1 : MonoBehaviour {

	public float activeDuration = .2f;
	SpriteRenderer[] sprays;

	void Start() {
		sprays = transform.GetComponentsInChildren<SpriteRenderer>();
		Deactivate();
	}

	public void Activate() {
		for(int i = 0; i < sprays.Length; i++) {
			sprays[i].enabled = true;
		}
		Invoke("Deactivate", activeDuration);
	}

	void Deactivate() {
		for(int i = 0; i < sprays.Length; i++) {
			sprays[i].enabled = false;
		}
	}
}
