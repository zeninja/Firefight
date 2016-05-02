using UnityEngine;
using System.Collections;

public class WaterShaker : MonoBehaviour {

	public float minX = .1f, maxX = .3f;
	public float minY = .75f, maxY = 1f;
	public float minWait = .02f, maxWait = .05f;

	SpriteRenderer spriteRenderer;
	public Sprite[] waterSprites;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine("ShakeScale");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator ShakeScale() {
//		Debug.Log("Shaking Scale");
//		transform.localScale = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
		spriteRenderer.sprite = waterSprites[Random.Range(0, waterSprites.Length)];
		yield return new WaitForSeconds(Random.Range(minWait, maxWait));
		StartCoroutine("ShakeScale");
	}
}
