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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LeakHose() {
		waterSpray = transform.GetChild(0).GetComponent<WaterSpray>();

		if (player.hose.hoseDirectory[transform.position] != HoseType.corner &&
		    player.hose.hoseDirectory[transform.position] != HoseType.cross) {

		    waterSpray.startPos = transform.position;

		    if(player.hose.hoseDirectory[transform.position] == HoseType.horizontal) {
				waterSpray.direction = Vector2.up;
		    }

			if(player.hose.hoseDirectory[transform.position] == HoseType.vertical) {
				waterSpray.direction = Vector2.right;
		    }

			waterSpray.SprayWater();
		}
	}
}