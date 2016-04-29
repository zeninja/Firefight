using UnityEngine;
using System.Collections;

public class FireExtinguisher : MonoBehaviour {

	public bool allDirections;
	public GameObject smokePrefab;
	
	// Use this for initialization
	void Start () {
		GameManager.gridStatus.Add(transform.position, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Activate() {
		Vector2 startPos = transform.position;

		GameObject smoke1 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke2 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke3 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke4 = Instantiate(smokePrefab) as GameObject;

		smoke1.transform.position = startPos + new Vector2( 1, 0);
		smoke2.transform.position = startPos + new Vector2(-1, 0);
		smoke3.transform.position = startPos + new Vector2(0,  1);
		smoke4.transform.position = startPos + new Vector2(0, -1);

		smoke1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
		smoke2.transform.rotation = Quaternion.Euler(new Vector3(0, 0,  90));
		smoke3.transform.rotation = Quaternion.Euler(new Vector3(0, 0,   0));
		smoke4.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

//		if(allDirections) {
//			for (int x = 0; x < 2; x++) {
//				for (int y = 0; y < 2; y++) {
//					int xCorner = x * 2 - 1;
//					int yCorner = y * 2 - 1;
//
//					GameObject smokeCorner = Instantiate(smokePrefab) as GameObject;
//					smokeCorner.transform.position = startPos + new Vector2(xCorner, yCorner);
//					smokeCorner.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45 * (x+1 + y+1)));
//				}
//			}
//		}

		GameObject smoke5 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke6 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke7 = Instantiate(smokePrefab) as GameObject;
		GameObject smoke8 = Instantiate(smokePrefab) as GameObject;

		smoke5.transform.position = startPos + new Vector2( 1,  1);
		smoke6.transform.position = startPos + new Vector2( 1, -1);
		smoke7.transform.position = startPos + new Vector2(-1, -1);
		smoke8.transform.position = startPos + new Vector2(-1,  1);

		smoke5.transform.rotation = Quaternion.Euler(new Vector3(0, 0,  -45));
		smoke6.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -135));
		smoke7.transform.rotation = Quaternion.Euler(new Vector3(0, 0,  135));
		smoke8.transform.rotation = Quaternion.Euler(new Vector3(0, 0,   45));

		GameManager.GetInstance().HandleWaterSpray(smoke1.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke2.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke3.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke4.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke5.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke6.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke7.transform.position);
		GameManager.GetInstance().HandleWaterSpray(smoke8.transform.position);
	}

}
