using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class levelLoaderScript : MonoBehaviour {

	public int currentLevel;
	public Button newGameButton;
	GameObject gameManager;

	// Use this for initialization
	void Start () {
		currentLevel = 1;
		gameManager = GameObject.Find ("GameManager");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeLevel(){
		currentLevel += 1;
		gameManager.GetComponent<gameManagerScript> ().resetLevel ();

	}

	public void setNewGameButtonText(bool wonGame){
		newGameButton.gameObject.SetActive (true);
		string newGametext;
		if (wonGame) {
			newGametext = "You Won! Play Again?";
		} else {
			newGametext = "You Lost.. Play Again?";
		}

		newGameButton.GetComponentInChildren<Text> ().text = newGametext;
		if (gameManager.GetComponent<gameManagerScript> ().enemyList.Count > 0) {
			foreach (GameObject enemy in gameManager.GetComponent<gameManagerScript>().enemyList) {
				if (enemy != null && enemy.GetComponent<enemySctipt> ().tileTouching != null) {
					enemy.GetComponent<enemySctipt> ().tileTouching.GetComponent<tileScript> ().hasEnemy = false;
					Destroy (enemy);
				} else {
					Destroy (enemy);
				}
			}
		}


	}

	public void newGameButtonClick(){
		gameManager.GetComponent<gameManagerScript> ().resetLevel ();
		gameManager.GetComponent<gameManagerScript> ().isGameOver = false;
		gameManager.GetComponent<gameManagerScript> ().playerHealth = 50;
		currentLevel = 1;
		newGameButton.gameObject.SetActive (false);


	}
}
