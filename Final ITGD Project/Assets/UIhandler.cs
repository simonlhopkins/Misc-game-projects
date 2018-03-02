using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour {
	public Text movementPointsText;
	public Text playerRollText;
	public Text enemyRollText;
	public Text unitsRemainingText;
	public Text castleHeathText;
	public Text waveText;
	GameObject gameManager;
	int currentWave;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");

	}
	
	// Update is called once per frame
	void Update () {
		movementPointsText.text = "Movement Points: " + gameManager.GetComponent<gameManagerScript> ().movementPoints;
		unitsRemainingText.text = "Units Remaining: " + gameManager.GetComponent<gameManagerScript> ().playerUnitsAvailable;
		castleHeathText.text = "Castle Health: " + gameManager.GetComponent<gameManagerScript> ().playerHealth;
		if (GetComponent<levelLoaderScript> ().currentLevel <= 5) {
			waveText.text = "Current Wave: " + GetComponent<levelLoaderScript> ().currentLevel;
		} else {
			waveText.text = "Current Wave: 5";
		}

	}

	public void loadAttackRolls(int playerMod, int enemyMod, int playerRoll, int enemyRoll, string fightTypeText){

		playerRollText.text = "Player Roll: " + playerRoll + " + " + fightTypeText+ " mod " + playerMod + " = " + (playerMod + playerRoll).ToString ();
		enemyRollText.text = "Enemy Roll: " + enemyRoll + " + " + fightTypeText+ " mod " + enemyMod + " = " + (enemyRoll + enemyMod).ToString ();
	}
}
