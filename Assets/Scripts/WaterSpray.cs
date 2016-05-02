using UnityEngine;
using System.Collections;

public class WaterSpray : MonoBehaviour {

	public Vector2 startPos, direction;
	public int affectedSquares = 1;

	public float activeDuration = .2f;

	bool willSpray;

	void Start() {
		startPos = transform.position;
		EndSpray();
	}

	public void SprayWater(bool spray) {
		willSpray = spray;
		RotateGraphics();

		if(willSpray) {
			gameObject.SetActive(true);
			transform.localScale = Vector2.one;

			for(int i = 0; i < affectedSquares; i++) {
				Vector2 targetPos = startPos + direction * (i + 1);
				GameManager.GetInstance().HandleWaterSpray(targetPos);
			}
		}

//		if(!transform.parent.CompareTag("Player")) {
			Invoke("EndSpray",  activeDuration);
//		}
	}

	void EndSpray() {
//		if (willSpray) {
			gameObject.SetActive(false);
//		} else {
//			transform.localScale = Vector2.one * .5f;
//		}
	}

	void RotateGraphics() {
		float rot = 0;
		if (direction.x != 0) { rot = direction.x > 0 ? -90 : 90; }
		if (direction.y != 0) { rot = direction.y > 0 ?   0 : 180; }
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
	}
}
