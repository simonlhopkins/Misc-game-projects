using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	// Use this for initialization
	void Awake(){
		if (gameManagerScript.instance == null) {
			Instantiate (gameManager);
		}
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
