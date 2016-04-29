using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour {

	public GameObject gameOverRowPrefab;
	public GameObject gameOverMenu;
	public UnityEngine.UI.Text gameOverText;
	public UnityEngine.UI.Text firesExtinguished;


	public float animationDuration = .25f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HandleGameOver() {
		gameOverText.text = GameManager.gotBurned ? "YOU GOT BURNED!" : "YOU GOT TRAPPED!";
		firesExtinguished.text = "YOU EXTINGUISHED ";
		firesExtinguished.text += GameManager.numFiresExtinguished == 1 ? "1 FIRE" : GameManager.numFiresExtinguished + " FIRES";
		StartCoroutine("ShowGameOverMenu");
	}

	IEnumerator ShowGameOverMenu() {
		yield return new WaitForEndOfFrame();

		#region Game Over Animation
		int numRows = 11;
		float animationTotal = numRows * animationDuration/2 + animationDuration;

		List<GameObject> gameOverRows = new List<GameObject>();

		for (int i = 0; i < numRows; i++) {
			GameObject gameOverRow = Instantiate(gameOverRowPrefab) as GameObject;
			gameOverRow.transform.position = (Vector2)Camera.main.transform.position + Vector2.down * 6;

			if(GameManager.gotBurned) {
				gameOverRow.GetComponent<GameOverRow>().SetColor("Fire");
			} else {
				gameOverRow.GetComponent<GameOverRow>().SetColor("Water");
			}
			gameOverRows.Add(gameOverRow);
		}

		for (int i = 0; i < gameOverRows.Count; i++) {
			gameOverRows[i].MoveTo((Vector2)Camera.main.transform.position + new Vector2(0, -5 + i), animationDuration, i * animationDuration/2, EaseType.easeOutBack);
		}

		yield return new WaitForSeconds(animationTotal);
		#endregion

		gameOverMenu.MoveTo(Camera.main.transform.position, animationDuration, 0, EaseType.easeInOutQuart );
	}

	public void Restart() {
		Application.LoadLevel(Application.loadedLevel);
		GameManager.gameOver = false;
	}
}
