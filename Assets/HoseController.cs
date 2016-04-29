using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HoseType { horizontal, vertical, corner, cross };

public class HoseController : MonoBehaviour {

	public static List<GameObject> hoseList = new List<GameObject>();

	static int alternatingLeakIndex = 0;

	public static void LeakHose() {

		for(int i = hoseList.Count - 1; i >= 0; i--) {
			if (i % 2 == alternatingLeakIndex) {
				hoseList[i].SendMessage("LeakHose", SendMessageOptions.DontRequireReceiver);
			}
		}
		alternatingLeakIndex++;

		if (alternatingLeakIndex > 1) {
			alternatingLeakIndex = 0;
		}
	}
}
