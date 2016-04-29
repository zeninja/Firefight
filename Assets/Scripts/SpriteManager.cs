using UnityEngine;
using System.Collections;

public class SpriteManager : MonoBehaviour {

	public enum Direction { Up, Down, Left, Right };

	int spriteIndex = 0;

	public Sprite[] front;
	public Sprite[] back;
	public Sprite[] left;
	public Sprite[] right;

	Sprite[] currentArray;

	// Use this for initialization
	void Start () {
		currentArray = new Sprite[4];
	}

	public void SetDirection(Direction newDirection) {
		switch(newDirection) {
			case Direction.Up:
				currentArray = front;
				break;
			case Direction.Down:
				currentArray = back;
				break;
			case Direction.Left:
				currentArray = left;
				break;
			case Direction.Right:
				currentArray = right;
				break;
		}
		UpdateSprite();
	}

	void UpdateSprite() {
		spriteIndex++;
		if (spriteIndex == 4) {
			spriteIndex = 0;
		}

		GetComponent<SpriteRenderer>().sprite = currentArray[spriteIndex];
	}
}
