using UnityEngine;
using System.Collections;

public class FireAnimator : MonoBehaviour {

	int spriteIndex; 
	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;

	public Vector2 targetSpawnPos;
	public Vector2 randomPos;

	// Use this for initialization
	void Start () {
		spriteIndex = Random.Range(0, sprites.Length);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateSprite() {
		spriteIndex++;
		if(spriteIndex >= sprites.Length) {
			spriteIndex = 0;
		}

		spriteRenderer.sprite = sprites[spriteIndex];
	}

	void HandleDeath() {
		gameObject.ScaleTo(Vector2.zero, .25f, 0);
		Invoke("Die", .25f);
	}

	void Die() {
		Destroy(gameObject);
	}
}
