using UnityEngine;
using System.Collections;

public class GameOverRow : MonoBehaviour {

	public Color[] reds;
	public Color[] blues;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetColor(string causeOfDeath) {
		Color[] colorArray;

		if (causeOfDeath == "Fire") {
			colorArray = reds;
		} else {
			colorArray = blues;
		}

		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorArray[Random.Range(0, colorArray.Length)];
		}
	}
}
