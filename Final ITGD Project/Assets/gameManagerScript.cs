using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour {

	public GameObject currentPlayerPiece;
	public List<GameObject> tilesToColor; 
	public bool isPlayerTurn;
	public bool isEnemyTurn;
	public int playerUnitsAvailable;
	public int movementPoints;
	int waveNumber;
	public int enemyIndexTurn=0;
	public List<GameObject> enemyList;
	public GameObject[] enemyTypes;
	public int playerHealth= 50;
	public GameObject currentEnemy;
	GameObject levelLoader;
	public bool isGameOver;
	public bool wonTheGame;
	// Use this for initialization
	void Start () {
		levelLoader = GameObject.Find ("levelLoader");
		resetLevel ();
		isGameOver = false;
		wonTheGame = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (levelLoader.GetComponent<levelLoaderScript> ().currentLevel > 5) {
			isGameOver = true;
			wonTheGame = true;
			levelLoader.GetComponent<levelLoaderScript> ().setNewGameButtonText (wonTheGame);
			return;
		} else if (playerHealth <= 0) {
			isGameOver = true;
			wonTheGame = false;
			levelLoader.GetComponent<levelLoaderScript> ().setNewGameButtonText (wonTheGame);
		} else {
			completeTurn ();

		}



	}

	public void setCurrentPlayerPiece(GameObject playerPiece){
		currentPlayerPiece = playerPiece;
	}

	public void colorTiles(){

		if (currentPlayerPiece == null) {
			return;
		}
		tilesToColor.Clear ();
		for(int i=0; i< GameObject.Find("tileContainer").transform.childCount; i++) {
			GameObject.Find ("tileContainer").transform.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
			foreach (GameObject tile in currentPlayerPiece.GetComponent<PlayerPieceScript>().potentialSpaces){
				if (tile == GameObject.Find ("tileContainer").transform.GetChild (i).gameObject) {
					tilesToColor.Add(GameObject.Find ("tileContainer").transform.GetChild (i).gameObject);
				}
			}
		}
		foreach (GameObject tile in tilesToColor) {
			tile.GetComponent<SpriteRenderer> ().color = Color.red;
		}
		
	}

	void completeTurn(){


		if (enemyList.Count == 0) {
			levelLoader.GetComponent<levelLoaderScript> ().changeLevel ();
			movementPoints += 8;
		}
		
		for (int i = 0; i < enemyList.Count; i++) {
			if (enemyList [i] == null) {
				enemyList.RemoveAt (i);
			}
		}

		for (int i = 0; i < GameObject.Find ("tileContainer").transform.childCount; i++) {
			if (GameObject.Find ("tileContainer").transform.GetChild (i).GetComponent<tileScript> ().hasPlayer && GameObject.Find ("tileContainer").transform.GetChild (i).GetComponent<tileScript> ().hasEnemy) {
				Attack (GameObject.Find ("tileContainer").transform.GetChild (i).GetComponent<tileScript> ().playerOnTile, GameObject.Find ("tileContainer").transform.GetChild (i).GetComponent<tileScript> ().enemyOnTile);

			}
		}

		if (isEnemyTurn) {
			isPlayerTurn = false;
			if (enemyIndexTurn < enemyList.Count) {
				currentEnemy = enemyList [enemyIndexTurn];
				if (currentEnemy != null) {
					currentEnemy.GetComponent<enemySctipt> ().Move ();
					if (enemyIndexTurn > enemyList.Count - 1) {
						movementPoints += 8;
					}
					if (currentEnemy.GetComponent<enemySctipt> ().tileTouching.tag == "playerSpawner" && !currentEnemy.GetComponent<enemySctipt> ().tileTouching.GetComponent<tileScript> ().hasPlayer) {
						if (currentEnemy.GetComponent<enemySctipt> ().enemyDoneMoving) {
							doDamage ();
							destroyEnemy (currentEnemy);
						}
					}
				}

			} else {
				if (currentEnemy == null && enemyList.Count>0) {
					currentEnemy = enemyList [0];
				}
			}
				
			clearSquares ();

		} else {
			
			if (enemyList.Count != 0) {
				currentEnemy = enemyList [0];
			}
		}

		if (currentEnemy != null) {
			if (currentEnemy.GetComponent<enemySctipt> ().enemyDoneMoving && isEnemyTurn) {
			
				if (enemyIndexTurn > enemyList.Count - 1) {
				

					isPlayerTurn = true;

					isEnemyTurn = false;
					playerUnitsAvailable = 2;

				}
			}
		}






	}

	public void Attack(GameObject playerPiece, GameObject enemyPiece){
		int playerRoll = Random.Range (1, 10);
		int enemyRoll = Random.Range (1, 10);
		int enemyModifier=0;
		int playerModifier = 0;

		if (playerPiece.GetComponent<PlayerPieceScript> ().moveSquare == false && enemyPiece.GetComponent<enemySctipt> ().enemyDoneMoving) {
			string fightTypeText;
			//suppossed to use intelligence
			if (enemyPiece.GetComponent<enemySctipt> ().intPoints >= enemyPiece.GetComponent<enemySctipt> ().strPoints) {

				if (!enemyPiece.GetComponent<enemySctipt> ().isMonked) {
					enemyModifier = enemyPiece.GetComponent<enemySctipt> ().intPoints;
					playerModifier = playerPiece.GetComponent<PlayerPieceScript> ().intPoints;
					fightTypeText="int";
				} else {
					enemyModifier = enemyPiece.GetComponent<enemySctipt> ().strPoints;
					playerModifier = playerPiece.GetComponent<PlayerPieceScript> ().strPoints;
					fightTypeText="str";
				}


			} 
			//supposed to use strength
			else {
				playerModifier = playerPiece.GetComponent<PlayerPieceScript> ().strPoints;
				if (!enemyPiece.GetComponent<enemySctipt> ().isMonked) {
					enemyModifier = enemyPiece.GetComponent<enemySctipt> ().strPoints;
					playerModifier = playerPiece.GetComponent<PlayerPieceScript> ().strPoints;
					fightTypeText="str";
				} else {
					enemyModifier = enemyPiece.GetComponent<enemySctipt> ().intPoints;
					playerModifier = playerPiece.GetComponent<PlayerPieceScript> ().intPoints;
					fightTypeText="int";

				}

			}

			GameObject.Find ("levelLoader").GetComponent<UIhandler> ().loadAttackRolls (
				playerModifier,
				enemyModifier,
				playerRoll,
				enemyRoll,
				fightTypeText);

			if ((enemyRoll + enemyModifier) > (playerRoll + playerModifier)) {
				
				if (playerPiece.gameObject.name == "monkTile(Clone)") {
					enemyPiece.GetComponent<enemySctipt> ().isMonked = true;
				}
				clearSquares ();
				playerPiece.GetComponent<PlayerPieceScript> ().tileTouching.GetComponent<tileScript> ().hasPlayer = false;
				playerPiece.GetComponent<PlayerPieceScript> ().tileTouching.layer = LayerMask.NameToLayer ("tileLayer");


				if (enemyPiece.GetComponent<enemySctipt> ().tileTouching.tag == "playerSpawner") {
					destroyEnemy (enemyPiece);
				}
				Destroy (playerPiece.gameObject);

			} else {
				destroyEnemy (enemyPiece);
			}
		}



	}

	void clearSquares(){
		for (int i = 0; i < GameObject.Find ("tileContainer").transform.childCount; i++) {
			GameObject.Find ("tileContainer").transform.GetChild (i).GetComponent<SpriteRenderer> ().color = Color.white;
		}
	}

	public void destroyEnemy(GameObject enemy){
		enemy.GetComponent<enemySctipt> ().tileTouching.GetComponent<tileScript> ().hasEnemy = false;
		Destroy (enemy.gameObject);
		if (isEnemyTurn) {
			enemyIndexTurn -= 1;
		}
	}

	public void resetLevel(){
		clearSquares ();
		enemyIndexTurn = 0;
		foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("enemySpawner")) {
			int enemyPicker = Random.Range (0, enemyTypes.Length); 
			GameObject enemy= Instantiate (enemyTypes.GetValue(enemyPicker) as GameObject);
			Vector3 tempPos = spawner.transform.position;
			enemy.transform.position = tempPos;
			enemyList.Add (enemy);
		}
		foreach (GameObject playerPiece in GameObject.FindGameObjectsWithTag("Player")) {
			playerPiece.GetComponent<PlayerPieceScript> ().tileTouching.GetComponent<tileScript> ().hasPlayer = false;
			Destroy (playerPiece);
		}
		for (int i = 0; i < GameObject.Find ("tileContainer").transform.childCount; i++) {
			GameObject.Find ("tileContainer").transform.GetChild (i).gameObject.gameObject.layer= LayerMask.NameToLayer ("tileLayer");
		}
		playerUnitsAvailable = 2;
		movementPoints = 0;
		isEnemyTurn = false;
		isPlayerTurn = true;


	}
	public void skipPlayerTurn(){
		if (isPlayerTurn) {
			isPlayerTurn = false;
			isEnemyTurn = true;
			enemyIndexTurn = 0;

		}
	}


	public void doDamage(){
		playerHealth -= 10;
	}
}
