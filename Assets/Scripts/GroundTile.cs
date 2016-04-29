using UnityEngine;
using System.Collections;

public class GroundTile : MonoBehaviour {

	public float spinDuration = 1;
	public float timeBetweenSpins = 1;

	int randomizer;

	public Color burnedColor;

	// Use this for initialization
	void Start () {
		randomizer = Random.Range(0, 2) * 2 - 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartRotation() {
		StartCoroutine("Rotate");
	}

	IEnumerator Rotate() {
		// Randomly choose either 1 or -1
		gameObject.RotateTo(new Vector3(0, 0, 180 * randomizer), spinDuration, 0, EaseType.easeOutBack);
		yield return new WaitForSeconds(timeBetweenSpins);
		StartCoroutine("Rotate");
	}

	public void Cancel() {
		StopCoroutine("Rotate");
	}

	public void GetBurned() {
		GetComponent<SpriteRenderer>().color = burnedColor;
	}
}
