using UnityEngine;
using System.Collections;

public class playerSpawnerScript : tileScript {

	GameObject gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
	}
	
	// Update is called once per frame
	void Update () {

	}


}
