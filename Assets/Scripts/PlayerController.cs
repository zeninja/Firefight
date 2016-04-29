using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public Transform water;
	public Transform axe;

	[System.NonSerialized]
	public static Vector2 moveOffset;

	Vector2 targetPos;
	float inputHorizontal;
	float inputVertical;
	bool canMove = true;

	public HoseDisplay hose;
	[System.Serializable]
	public class HoseDisplay {
		public GameObject segmentPrefab;
		public Sprite straight;
		public Sprite turn;

		[System.NonSerialized]
		public Vector2 lastMove = new Vector2(0, 1);

		public Dictionary<Vector2, HoseType> hoseDirectory = new Dictionary<Vector2, HoseType>();
	}

	// Use this for initialization
	void Start () {
		
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

		if(!GameManager.gridStatus.ContainsKey(targetPos) || GameManager.gridStatus[targetPos].CompareTag("Fire") || GameManager.gridStatus[targetPos].CompareTag("Hose")) {
			GameObject hoseSegment = Instantiate(hose.segmentPrefab) as GameObject;
			hoseSegment.transform.position = transform.position;

			if (hose.hoseDirectory.ContainsKey(targetPos)) {
				// Target position is occuppied, meaning we are crossing over the hose
				GameObject jumpSegment = Instantiate(hose.segmentPrefab) as GameObject;
				jumpSegment.transform.position = targetPos;

	//			hose.hoseDirectory[targetPos] = HoseType.cross;

				#region Horizontal Jumping
				switch((int)moveOffset.x) {
					case 1:
						// Jumping RIGHT
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Right);
						jumpSegment.transform.Rotate(new Vector3(0, 0, 90));
						hose.hoseDirectory[targetPos] = HoseType.horizontal;


						if(hose.lastMove.x > 0) {
							// Last move was also RIGHT
							hose.hoseDirectory[transform.position] = HoseType.horizontal;	
							hoseSegment.transform.Rotate(new Vector3(0, 0, 90));

						} else {
							// Last move was VERTICAL, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.y > 0) {
								// No need to rotate, this is the default rotation;
							} else {
								hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
							}
						}
						break;
					case -1:
						// Jumping LEFT
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Left);
						jumpSegment.transform.Rotate(new Vector3(0, 0, 90));
						hose.hoseDirectory[targetPos] = HoseType.horizontal;

						if(hose.lastMove.x < 0) {
							// Last move was also LEFT
							hose.hoseDirectory[transform.position] = HoseType.horizontal;	
							hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
						} else {
							// Last move was VERTICAL, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.y > 0) {
								hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
							} else {
								hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
							}
						}
						break;
				}
				#endregion

				#region Vertical Jumping
				switch((int)moveOffset.y) {
					case 1:
						// Moving UP
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Up);
						hose.hoseDirectory[targetPos] = HoseType.vertical;

						if(hose.lastMove.y > 0) {
							// Last move was also UP
							// Do nothing, this is the default rotation
							hose.hoseDirectory[transform.position] = HoseType.vertical;
						} else if(hose.lastMove.x != 0) {
							// Last move was horizontal, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;	
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.x > 0) {
								// Turning RIGHT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
							} else {
								// Turning LEFT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
							}
						}
						break;

					case -1:
						// Moving DOWN
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Down);
						hose.hoseDirectory[targetPos] = HoseType.vertical;

						if(hose.lastMove.y < 0) {
							// Last move was also DOWN
							// Do nothing, this is the default rotation
							hose.hoseDirectory[transform.position] = HoseType.vertical;
						} else if(hose.lastMove.x != 0) {
							// Last move was horizontal, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.x > 0) {
								// Turning RIGHT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
							} else {
								// Turning LEFT
								// Do nothing, this is the default rotation
							}
						}
						break;
				}
				#endregion

				// Increment the move offset so that we "JUMP", this is pretty janky
				GameManager.gridStatus[(Vector2)transform.position + moveOffset] = jumpSegment;
				targetMovePos = (Vector2)transform.position + moveOffset * 2;

				// Set up the reference so that the hose leaks
				jumpSegment.GetComponent<HoseSegment>().player = this;
				StartCoroutine("AddJumpSegment", jumpSegment);
			} else {
				// Target position is empty
				
				#region Horizontal Movement
				switch((int)moveOffset.x) {
					case 1:
						// Moving RIGHT
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Right);

						if(hose.lastMove.x > 0) {
							// Last move was also RIGHT
							hose.hoseDirectory[transform.position] = HoseType.horizontal;	
							hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
						} else if(hose.lastMove.y != 0) {
							// Last move was VERTICAL, were TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;	
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.y > 0) {
								// No need to rotate, this is the default rotation;

							} else {
								hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
							}
						}
						break;

					case -1:
						// Moving LEFT
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Left);

						if(hose.lastMove.x < 0) {
							// Last move was also LEFT
							hose.hoseDirectory[transform.position] = HoseType.horizontal;	
							hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
						} else if(hose.lastMove.y != 0) {
							// Last move was VERTICAL, were TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;	
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.y > 0) {
								// Turning UP
								hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
							} else {
								// Turning DOWN
								hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
							}
						}

						break;
				}
				#endregion

				#region Vertical Movement
				switch((int)moveOffset.y) {
					case 1:
						// Moving UP
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Up);

						if(hose.lastMove.y > 0) {
							// Last move was also UP
							// Do nothing, this is the default rotation
							hose.hoseDirectory[transform.position] = HoseType.vertical;
						} else if(hose.lastMove.x != 0) {
							// Last move was horizontal, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;	
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.x > 0) {
								// Turning RIGHT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
							} else {
								// Turning LEFT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
							}
						}
						break;

					case -1:
						// Moving DOWN
						GetComponent<SpriteManager>().SetDirection(SpriteManager.Direction.Down);

						if(hose.lastMove.y < 0) {
							// Last move was also DOWN
							// Do nothing, this is the default rotation
							hose.hoseDirectory[transform.position] = HoseType.vertical;
						} else if(hose.lastMove.x != 0) {
							// Last move was horizontal, we are TURNING
							hose.hoseDirectory[transform.position] = HoseType.corner;
							hoseSegment.GetComponent<SpriteRenderer>().sprite = hose.turn;

							if(hose.lastMove.x > 0) {
								// Turning RIGHT
								hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
							} else {
								// Turning LEFT
								// Do nothing, this is the default rotation
							}
						}
						break;
				}
				#endregion

				// Move the player
				targetMovePos = (Vector2)transform.position + moveOffset;
			}

			// Set up the reference so that the hose leaks
			hoseSegment.GetComponent<HoseSegment>().player = this;
			HoseController.hoseList.Add(hoseSegment);
			hose.lastMove = moveOffset;

			// Update the Game manager's grid, and replace an object if necessary
			if(!GameManager.gridStatus.ContainsKey(transform.position)) {
				GameManager.gridStatus.Add(transform.position, hoseSegment);
			} else {
				// This should ONLY replace other hose segments
				// TODO: replace with a CROSS instead of just the new segment
				GameManager.gridStatus[transform.position] = hoseSegment;
			}

		} else {
			// Trigger obstacles etc.
			GameManager.gridStatus[targetPos].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
		}

		transform.position = targetMovePos;
		CheckAvailableMoves();
	}

	IEnumerator AddJumpSegment(GameObject jumpSegment) {
		// Making this a coroutine (and waiting til the end of the frame) so that it adds the segments in the correct order
		// Could probably use an invoke?? or something else?
		yield return new WaitForEndOfFrame();
		HoseController.hoseList.Add(jumpSegment);
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
		water.GetComponent<WaterSpray>().SprayWater();
		HoseController.LeakHose();
	}

	#region Helper Functions
	bool OutOfBounds(Vector2 testDirection) {
		Vector2 testPos = (Vector2)transform.position + testDirection;
		if (hose.hoseDirectory.ContainsKey(testPos)) {

			if ((testDirection.x != 0 && hose.hoseDirectory[testPos] == HoseType.vertical) || 
				(testDirection.y != 0 && hose.hoseDirectory[testPos] == HoseType.horizontal)) {
				testPos += testDirection;
			}
		}

		return Mathf.Abs(testPos.x) >= GameManager.xBound;
	}

	bool TargetPosInvalid(Vector2 testDirection) {
		Vector2 testPos = (Vector2)transform.position + testDirection;

		if(hose.hoseDirectory.ContainsKey(testPos)) {
			// We can only jump across spaces of the opposite type
			if (testDirection.x != 0 && hose.hoseDirectory[testPos] != HoseType.vertical) {
//				Debug.Log(testDirection + ": TargetPosInvalid1");
				return true;
			}

			if (testDirection.y != 0 && hose.hoseDirectory[testPos] != HoseType.horizontal) {
//				Debug.Log(testDirection + ": TargetPosInvalid2");
				return true;
			}

			if (hose.hoseDirectory.ContainsKey(testPos) && hose.hoseDirectory.ContainsKey(testPos + testDirection)) {
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