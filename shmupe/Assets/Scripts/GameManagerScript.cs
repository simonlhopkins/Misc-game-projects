using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public int currentWave;
	public List<GameObject[]> waveList= new List<GameObject[]>();
	public GameObject[] wave1;
	public GameObject[] wave2;
	public GameObject[] wave3;
	public GameObject[] wave4;
	public GameObject[] wave5;
	public GameObject[] wave6;
	public GameObject[] wave7;
	public GameObject[] wave8;
	public GameObject[] wave9;
	public GameObject[] wave10;
	public GameObject playerPrefab;
	Vector3 startingPosition;
	public Text healthText;
	public float startTime;
	public float timeInBetweenWaves;
	public Text timeInBetweenWavesText;
	public float waveTimer;
	public bool isGameOver;
	GameObject newGameButton;
	// Use this for initialization
	void Start () {
		newGameButton = GameObject.Find ("NewGameButton");
		newGameButton.SetActive (false);
		startTime = Time.time;
		waveTimer = timeInBetweenWaves-startTime;
		currentWave = 0;
		startingPosition= new Vector3(0f, 32f, 0f);
		waveList.Add (wave1);
		waveList.Add (wave2);
		waveList.Add (wave3);
		waveList.Add (wave4);
		waveList.Add (wave5);
		waveList.Add (wave6);
		waveList.Add (wave7);
		waveList.Add (wave8);
		waveList.Add (wave9);
		waveList.Add (wave10);
		startGame ();

	}
	
	// Update is called once per frame
	void Update () {


		if (!isGameOver) {
			if(GameObject.FindGameObjectWithTag("Player").activeInHierarchy){
				healthText.text = "Health: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Ship> ().health;

			}
			timeInBetweenWavesText.text = "Time Until Next Wave: " + Mathf.Ceil(waveTimer);
		}
		if (currentWave >= waveList.Count-1) {
			currentWave = 0;
		}
		if (waveTimer <= 0 && !isGameOver) {
			
			waveTimer = timeInBetweenWaves;
			startingPosition = new Vector3 (0f, 32f, 0f);
			currentWave += 1;
			startTime = Time.time;

		}
		waveTimer = timeInBetweenWaves-(Time.time - startTime);
		if (waveTimer == timeInBetweenWaves) {
			
			int loopCounter = 0;
			foreach (GameObject shipPrefab in waveList[currentWave]) {
				loopCounter += 1;
				Vector3 tempPos = startingPosition;
				GameObject ship = Instantiate (shipPrefab);
				ship.transform.position = tempPos;
				if (loopCounter % 2 == 0) {
					startingPosition.x += (10f * loopCounter);
				} else {
					startingPosition.x -= (10f * loopCounter);
				}
			}

		}
		foreach (GameObject explosion in GameObject.FindGameObjectsWithTag("Explosion")) {
			if (explosion.GetComponent<SpriteRenderer>().sprite != null) {
				if (explosion.GetComponent<SpriteRenderer> ().sprite.name == "spritesheet1_73") {
					Destroy (explosion);
				}
			}

		}


	}

	public void gameOver(){
		isGameOver = true;
		currentWave = 0;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Ship> ().health = 0;
		healthText.text = "Health: 0";
		timeInBetweenWavesText.text = "timeInBetweenWaves: 0";
		Destroy (GameObject.FindGameObjectWithTag("Player").gameObject);
		newGameButton.SetActive (true);
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			if (enemy.GetComponent<Enemy> () == null) {
				enemy.GetComponentInChildren<Enemy> ().destroyShip ();
			} else {
				enemy.GetComponent<Enemy> ().destroyShip ();
			}
		}

		foreach (GameObject powerUp in GameObject.FindGameObjectsWithTag("PowerUp")) {
			Destroy (powerUp);
		}
		foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("ProjectileEnemy")) {
			Destroy (projectile);
		}
		foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("ProjectileHero")) {
			Destroy (projectile);
		}

	}

	public void startGame(){
		startingPosition= new Vector3(0f, 32f, 0f);
		currentWave = 0;
		GameObject player= Instantiate (playerPrefab);
		isGameOver = false;
		player.transform.position = new Vector3 (0, 0, 0);
		startTime = Time.time;
		newGameButton.SetActive (false);
	}
}
