using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoseSegment : MonoBehaviour {

	public HoseType myType;

	// PROBABLY AN EASIER WAY TO DO THIS???
//	[System.NonSerialized]
	public PlayerController player;
	WaterSpray waterSpray;

	public bool willSpray;

	public void LeakHose() {
		waterSpray = transform.GetChild(0).GetComponent<WaterSpray>();

		if (HoseController.hoseDictionary[transform.position].type != HoseType.corner &&
			HoseController.hoseDictionary[transform.position].type != HoseType.cross) {

		    waterSpray.startPos = transform.position;

			if(HoseController.hoseDictionary[transform.position].type == HoseType.horizontal) {
				waterSpray.direction = Vector2.up;
		    }

			if(HoseController.hoseDictionary[transform.position].type == HoseType.vertical) {
				waterSpray.direction = Vector2.right;
		    }

			waterSpray.SprayWater(willSpray);
		}
	}
}