using UnityEngine;
using System.Collections;

public class WaterSpray : MonoBehaviour {

	public Vector2 startPos, direction;
	public int affectedSquares = 1;

	public float activeDuration = .2f;
	GameObject[] sprays = new GameObject[3];

	void Awake() {
		startPos = transform.position;
		for (int i = 0; i < transform.childCount; i++) {
			sprays[i] = transform.GetChild(i).gameObject;
		}
		Deactivate();
	}

	public void SprayWater() {
		RotateGraphics();

		for(int i = 0; i < affectedSquares; i++) {
			Vector2 targetPos = startPos + direction * affectedSquares;
			GameManager.GetInstance().HandleWaterSpray(targetPos);
		}
		Activate();
	}

	void Activate() {
		for (int i = 0; i < sprays.Length; i++) {
			sprays[i].SetActive(true);
			sprays[i].transform.localScale = Vector2.one;
			sprays[i].transform.localPosition *= 2f;
		}

//		if(disappear) {
//			Invoke("Deactivate", activeDuration);
//		} else {
			Invoke("ShrinkGraphics", activeDuration);
//		}
	}

	void Deactivate() {
		for (int i = 0; i < sprays.Length; i++) {
			sprays[i].SetActive(false);
		}
	}

	void ShrinkGraphics() {
		for(int i = 0; i < sprays.Length; i++) {
			sprays[i].transform.localScale = Vector3.one * .3f;
			sprays[i].transform.localPosition *= .25f;
		}
	}

	void RotateGraphics() {
		float rot = 0;

		if (direction.x != 0) {
			rot = direction.x > 0 ? -90 : 90;
		}

		if (direction.y != 0) {
			rot = direction.y > 0 ? 0 : 180;
		}

		transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
	}
}
