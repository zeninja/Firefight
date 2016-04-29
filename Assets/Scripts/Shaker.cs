using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {

	public float minScale = .8f, maxScale = 1.25f;
	public float minWait = .1f, maxWait = .5f;

	int sortingOrder;

	// Use this for initialization
	void Start () {
		sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		StartCoroutine("ShakeScale");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ShakeScale() {
		transform.localScale = Vector2.one * Random.Range(minScale, maxScale);
		GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + Random.Range(0, 5);
		yield return new WaitForSeconds(Random.Range(minWait, maxWait));
		StartCoroutine("ShakeScale");
	}
}
