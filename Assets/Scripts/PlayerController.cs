using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	HoseController hoseController;
	public Transform water;
	public Transform axe;

	[System.NonSerialized]
	public static Vector2 moveOffset;

	Vector2 targetPos;
	float inputHorizontal;
	float inputVertical;
	bool canMove = true;

	// Use this for initialization
	void Start () {
		hoseController = GetComponent<HoseController>();
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();
	}

	void ManageInput() {
		inputHorizontal = Input.GetAxisRaw("Horizontal");
		inputVertical   = Input.GetAxisRaw("Vertical");

		if (canMove && !GameManager.gameOver) {
			if (inputHorizontal != 0 && inputVertical == 0) {
				moveOffset = new Vector2(Mathf.Sign(inputHorizontal), 0);
			}

			if (inputVertical != 0 && inputHorizontal == 0) {
				moveOffset = new Vector2(0, Mathf.Sign(inputVertical));
			}

			if (moveOffset != Vector2.zero) {
				Move();
			}
		}

		if (inputHorizontal == 0 && inputVertical == 0) {
			canMove = true;
			moveOffset = Vector2.zero;
		}
	}

	#region Movement
	void Move() {
		targetPos = (Vector2)transform.position + moveOffset;

		if(OutOfBounds(moveOffset) || TargetPosInvalid(moveOffset))  {
			return;
		}

		MoveAndPlaceHose();
		SprayWater();
		GameManager.GetInstance().HandlePlayerMove();
		canMove = false;
	}

	void MoveAndPlaceHose() {
		Vector2 targetMovePos = transform.position;
		if (!GameManager.gridStatus.ContainsKey(targetPos) || GameManager.gridStatus[targetPos].CompareTag("Fire") || GameManager.gridStatus[targetPos].CompareTag("Hose")) {
			if (!HoseController.hoseDictionary.ContainsKey(targetMovePos + moveOffset)) {
				// Target move position is empty
				hoseController.AddSegment(transform.position, moveOffset);
				targetMovePos = (Vector2)transform.position + moveOffset;
			} else {
				// Target move position contains a hose
				if (!HoseController.hoseDictionary.ContainsKey((Vector2)transform.position + moveOffset * 2)) {

					hoseController.AddSegment(transform.position, moveOffset);
					hoseController.AddSegment(targetPos, moveOffset);
					targetMovePos = (Vector2)transform.position + moveOffset * 2;
				}
			}
		} else {
			// Trigger obstacles etc.
			GameManager.gridStatus[targetPos].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
		}
		transform.position = targetMovePos;
		CheckAvailableMoves();
		hoseController.HandlePlayerMove();
	}
	
	void CheckAvailableMoves() {
		int numAvailableMoves = 0;

		numAvailableMoves += TargetPosInvalid(Vector2.up) 	 || OutOfBounds(Vector2.up)	   ? 0 : 1;
		numAvailableMoves += TargetPosInvalid(Vector2.down)  || OutOfBounds(Vector2.down)  ? 0 : 1;
		numAvailableMoves += TargetPosInvalid(Vector2.left)  || OutOfBounds(Vector2.left)  ? 0 : 1;
		numAvailableMoves += TargetPosInvalid(Vector2.right) || OutOfBounds(Vector2.right) ? 0 : 1;

		if (numAvailableMoves == 0) {
			GameManager.GetInstance().HandleGameOver();	
		}
	}
	#endregion

	void SprayWater() {
		// Spray water to extinguish fires and trigger obstacles
		water.GetComponent<WaterSpray>().startPos = transform.position;
		water.GetComponent<WaterSpray>().direction = moveOffset;
		water.GetComponent<WaterSpray>().SprayWater(true);
	}

	#region Helper Functions
	bool OutOfBounds(Vector2 testDirection) {
		Vector2 testPos = (Vector2)transform.position + testDirection;
		if (HoseController.hoseDictionary.ContainsKey(testPos)) {
			if ((testDirection.x != 0 && HoseController.hoseDictionary[testPos].type == HoseType.vertical) || 
				(testDirection.y != 0 && HoseController.hoseDictionary[testPos].type == HoseType.horizontal)) {
				testPos += testDirection;
			}
		}

		return Mathf.Abs(testPos.x) >= GameManager.xBound;
	}

	bool TargetPosInvalid(Vector2 testDirection) {
		Vector2 testPos = (Vector2)transform.position + testDirection;

		if(HoseController.hoseDictionary.ContainsKey(testPos)) {
			// We can only jump across spaces of the opposite type
			if (testDirection.x != 0 && HoseController.hoseDictionary[testPos].type != HoseType.vertical) {
//				Debug.Log(testDirection + ": TargetPosInvalid1");
				return true;
			}

			if (testDirection.y != 0 && HoseController.hoseDictionary[testPos].type != HoseType.horizontal) {
//				Debug.Log(testDirection + ": TargetPosInvalid2");
				return true;
			}

			if (HoseController.hoseDictionary.ContainsKey(testPos) && HoseController.hoseDictionary.ContainsKey(testPos + testDirection)) {
				// We can't jump across two consecutive full spaces
//				Debug.Log(testDirection + ": TargetPosInvalid3.");
				return true;
			}
		}

		return false;
	}

	void OnDrawGizmos() {
		TestDirection(Vector2.up);
		TestDirection(Vector2.down);
		TestDirection(Vector2.left);
		TestDirection(Vector2.right);
	}

	void TestDirection(Vector2 testDirection) {
		Gizmos.color = TargetPosInvalid(testDirection) || OutOfBounds(testDirection) ? Color.red : Color.green;
		Gizmos.DrawWireSphere((Vector2)transform.position + testDirection, .25f);
	}
	#endregion
}