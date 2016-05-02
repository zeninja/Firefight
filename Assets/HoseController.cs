using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public enum HoseType { horizontal, vertical, corner, cross };

public class HoseController : MonoBehaviour {

	public GameObject hoseSegmentPrefab;
	public Sprite turnSprite;

	public static List<HoseInfo> hoseList = new List<HoseInfo>();
	public static Dictionary<Vector2, HoseInfo> hoseDictionary = new Dictionary<Vector2, HoseInfo>();

	public class HoseInfo {
		public HoseType type;
		public GameObject gameObject;
		public Vector2 moveDirection;
	}

	public void AddSegment(Vector2 position, Vector2 moveDirection) {
		HoseInfo nextSegmentInfo = new HoseInfo();

		GameObject hoseSegment = Instantiate(hoseSegmentPrefab) as GameObject;
		if (hoseList.Count > 0) {
			// Handle an empty space
			if (hoseList[0].moveDirection == moveDirection) {
				// Moving in the same direction as the last move
				nextSegmentInfo.type = moveDirection.x != 0 ? HoseType.horizontal : HoseType.vertical;

				// Rotate the segment
				if(moveDirection.x != 0) { hoseSegment.transform.rotation = Quaternion.Euler(0, 0, 90); }

			} else {
				// Turning
				nextSegmentInfo.type = HoseType.corner;
				hoseSegment.GetComponent<SpriteRenderer>().sprite = turnSprite;
				RotateTurnSegment(hoseSegment, moveDirection);
			}
		} else {
			nextSegmentInfo.type = moveDirection.x != 0 ? HoseType.horizontal : HoseType.vertical;
			if (moveDirection.x != 0) { hoseSegment.transform.rotation = Quaternion.Euler(0, 0, 90); }
		}

		hoseSegment.transform.position = position;

		nextSegmentInfo.gameObject = hoseSegment;
		nextSegmentInfo.moveDirection = moveDirection;

		hoseList.Insert(0, nextSegmentInfo);

		if(!hoseDictionary.ContainsKey(position)) {
			hoseDictionary.Add(position, nextSegmentInfo);
		} else {
			hoseDictionary[position].type = HoseType.cross;
			hoseDictionary[position] = nextSegmentInfo;
		}
	}

	void RotateTurnSegment(GameObject hoseSegment, Vector2 moveDirection) {
		switch((int)hoseList[0].moveDirection.x) {
			case 1:
				if(moveDirection.y > 0) {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
				} else {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
				}
				break;
			case -1:
				if(moveDirection.y > 0) {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
				} else {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 0));
				}
				break;
		}

		switch((int)hoseList[0].moveDirection.y) {
			case 1:
				if(moveDirection.x > 0) {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 0));
				} else {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 270));
				}
				break;
			case -1:
				if(moveDirection.x > 0) {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 90));
				} else {
					hoseSegment.transform.Rotate(new Vector3(0, 0, 180));
				}
				break;
		}
	}

	public void HandlePlayerMove() {
		for(int i = 0; i < hoseList.Count; i++) {
			if (hoseList[i].type != HoseType.corner && hoseList[i].type != HoseType.cross) {
				hoseList[i].gameObject.GetComponent<HoseSegment>().willSpray = i % 2 != 0;
				hoseList[i].gameObject.GetComponent<HoseSegment>().LeakHose();
			}
		}
	}
}