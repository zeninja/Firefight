using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameOverManager gameOverManager;

	public static int xBound = 4;
	public GameObject firePrefab;
//	public GameObject treePrefab;

//	public GameObject[] obstaclePrefabs;
	public int numTilesInRow = 7;

	int numPlayerSteps;
	int maxPlayerHeight = 0;
	public int spawnThreshold = 2;

	public GameObject groundTilePrefab;
	public Sprite[] groundSprites;
	Dictionary<Vector2, GameObject> groundTiles = new Dictionary<Vector2, GameObject>();
	GameObject selectedTile;

	public static Dictionary<Vector2, GameObject> gridStatus = new Dictionary<Vector2, GameObject>();
	List<GameObject> fires = new List<GameObject>();
	Vector2 fireSpawnPos;

	public static bool gameOver = false;
	public static bool gotBurned = false;

	public static int numFiresExtinguished;

	private static GameManager instance;
	private static bool instantiated;

	public static GameManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(GameManager)) as GameManager;
			if (!instance)
				Debug.Log("No GameManager!!");		
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
		GenerateStartingGround();
	}

	void GenerateStartingGround() {
		int numCols = 11;

		for (int j = 0; j < numCols; j++) {
			for (int i = 0; i < numTilesInRow; i++) {
				GameObject groundTile = Instantiate(groundTilePrefab) as GameObject;
				groundTile.transform.position = new Vector2(i - numTilesInRow/2, j - numCols/2);
				groundTile.GetComponent<SpriteRenderer>().sprite = groundSprites[Random.Range(0, groundSprites.Length)];
				groundTile.transform.parent = transform;
				groundTiles.Add(groundTile.transform.position, groundTile);
			}
		}
	}

	void SpawnGround() {
		int yPos = maxPlayerHeight + 5;

		for (int i = 0; i < numTilesInRow; i++) {
			GameObject groundTile = Instantiate(groundTilePrefab) as GameObject;
			groundTile.transform.position = new Vector2(i - numTilesInRow/2, yPos);
			groundTile.GetComponent<SpriteRenderer>().sprite = groundSprites[Random.Range(0, groundSprites.Length)];
			groundTile.transform.parent = transform;
			groundTiles.Add(groundTile.transform.position, groundTile);
		}
	}

	void FindNextFireLocation() {
		fireSpawnPos = RandomizeSpawnPos();

		while(gridStatus.ContainsKey(fireSpawnPos)) {
			fireSpawnPos = RandomizeSpawnPos();
		}
		RotateTile(fireSpawnPos);
	}

	void SpawnFire() {
		GameObject fire = Instantiate(firePrefab) as GameObject;
		fire.transform.position = fireSpawnPos;
		gridStatus.Add(fireSpawnPos, fire);
		fires.Add(fire);
	}

	void RotateTile(Vector2 tilePos) {
		selectedTile = groundTiles[tilePos];
		selectedTile.GetComponent<GroundTile>().StartRotation();
	}

	Vector2 RandomizeSpawnPos() {		
	// TODO: This may need to be reworked/adjusted later so that the game doesn't crash if/when it runs out of spaces to place a fire in
		Vector2 spawnPos;
		spawnPos.x = Random.Range(-xBound + 1, xBound);
		spawnPos.y = Random.Range(3, 6) + player.transform.position.y;
		return spawnPos;
	}

	public void HandleWaterSpray(Vector2 position) {
		if (gridStatus.ContainsKey(position)) {
			if (gridStatus[position].CompareTag("Fire")) {
				fires.Remove(gridStatus[position]);
				gridStatus[position].SendMessage("HandleDeath");
				gridStatus.Remove(position);
				numFiresExtinguished++;
			} else {
				gridStatus[position].SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void HandlePlayerMove() {
		for(int i = 0; i < fires.Count; i++) {
			fires[i].GetComponent<FireAnimator>().UpdateSprite();
		}

		if (gridStatus.ContainsKey(player.transform.position)) {
			if (gridStatus[player.transform.position].CompareTag("Fire")){
				gotBurned = true;
				HandleGameOver();
			}
		}

		// If the player has moved higher than 'maxPlayerHeight', increase it to reflect the new height
		if (player.transform.position.y > maxPlayerHeight) {
			maxPlayerHeight = (int)player.transform.position.y;
			SpawnGround();
		}

		numPlayerSteps++;

		if (numPlayerSteps == spawnThreshold) {
			FindNextFireLocation();
		}

		if (numPlayerSteps > spawnThreshold) {
			SpawnFire();
//			GameObject obstaclePrefab;
//			obstaclePrefab = obstaclePre
//			SpawnObstacle(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);

//			selectedTile.GetComponent<GroundTile>().Cancel();
			numPlayerSteps = 0;
		}
	}

	public void HandleGameOver() {
		Debug.Log("YOU GOT BURNED!!!!");
		gameOverManager.HandleGameOver();
		gameOver = true;
		gridStatus.Clear();
	}
}
