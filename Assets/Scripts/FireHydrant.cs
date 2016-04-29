using UnityEngine;
using System.Collections;

public class FireHydrant : MonoBehaviour {

	public int sprayDistance = 3;
	public GameObject waterSprayPrefab;
	public float waterScaleDuration = .25f;
	public Sprite waterEnd;

	void Start() {
		// TODO: REMOVE THIS. THIS IS ONLY HERE BECAUSE THE FIRE HYDRANTS ARE NOT BEING SPAWNED INTO THE GAME ATM
		/// DEBUGGGGGGGG
		GameManager.gridStatus.Add(transform.position, gameObject);
	}

	void Activate() {
		StartCoroutine("SprayWater");
//		SprayWater();
	}

	IEnumerator SprayWater() {
		Vector2 direction = PlayerController.moveOffset.normalized;

		for (int i = 0; i < sprayDistance; i++) {
			Vector2 targetPos = (Vector2)transform.position + direction * (i + 1);
			GameObject waterSpray = Instantiate(waterSprayPrefab) as GameObject;
			waterSpray.transform.position = targetPos;
			waterSpray.transform.localScale = Vector2.zero;
			waterSpray.ScaleTo(Vector2.one, waterScaleDuration, 0);
			waterSpray.ScaleTo(Vector2.zero, waterScaleDuration, waterScaleDuration * 1/(i + 1) + (sprayDistance - i) * waterScaleDuration);

			if (i == sprayDistance - 1) {
				waterSpray.GetComponent<SpriteRenderer>().sprite = waterEnd;
			}

			if (GameManager.gridStatus.ContainsKey(targetPos)) {
				if (GameManager.gridStatus[targetPos].CompareTag("Fire")) {
					GameManager.GetInstance().HandleWaterSpray(targetPos);
				}
				GameManager.gridStatus[targetPos].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
			yield return new WaitForSeconds(waterScaleDuration);
		}
	}

//	void SprayWater() {
//		Vector2 direction = PlayerController.moveOffset.normalized;
//
//		for (int i = 0; i < sprayDistance; i++) {
//			Vector2 targetPos = (Vector2)transform.position + direction * (i + 1);
//			GameObject waterSpray = Instantiate(waterSprayPrefab) as GameObject;
//			waterSpray.transform.position = targetPos;
//			waterSpray.transform.localScale = Vector2.zero;
//
//			waterSpray.ScaleTo(Vector2.one,  waterScaleDuration, waterScaleDuration * i);
//			waterSpray.ScaleTo(Vector2.zero, waterScaleDuration, 1/(i + 1)  waterScaleDuration * i);
//
//			if (GameManager.gridStatus.ContainsKey(targetPos)) {
//				if (GameManager.gridStatus[targetPos].CompareTag("Fire")) {
//					GameManager.GetInstance().ExtinguishFire(targetPos);
//				}
//			}
//		}
//	}
}
