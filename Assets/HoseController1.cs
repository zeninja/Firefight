using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum HoseType { horizontal, vertical, corner, cross };

public class HoseController1 : MonoBehaviour {

	public static List<GameObject> hoseList = new List<GameObject>();

	static int alternatingLeakIndex = 0;

	public static void HandlePlayerMove() {
		for (int i = 0; i < hoseList.Count; i++) {
			hoseList[i].GetComponent<HoseSegment>().willSpray = i % 2 == 0;
		}
	}

	public static void LeakHose() {
		for (int i = 0; i < hoseList.Count; i++) {
			hoseList[i].GetComponent<HoseSegment>().LeakHose();
		}
	}
}
