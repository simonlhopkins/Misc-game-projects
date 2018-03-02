using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour {

	public List<string> playerInventory;
	GameObject canvas;
	public string mainTextOption;
	public static gameManagerScript instance = null;

	// Use this for initialization

	void Awake(){
		Cursor.lockState = CursorLockMode.Confined;
		if (instance == null) {
			instance = this;
		} else {
			instance = null;
		}
	}
	void Start () {
		mainTextOption="mainText1";
		Object.DontDestroyOnLoad (gameObject);


	}

	public void addToInventory(string thingToAdd){
		if (!playerInventory.Contains (thingToAdd)) {
			playerInventory.Add (thingToAdd);
		}
	}
	public void removeFromInventory(string thingToRemove){
		if (playerInventory.Contains (thingToRemove)) {
			playerInventory.Remove (thingToRemove);
		}

	}
	void Update(){
		setMainText (mainTextOption);
	}
	public void setMainText(string textName){
		canvas = GameObject.Find ("Canvas");
		if (canvas != null) {
			for (int i = 0; i < canvas.transform.childCount; i++) {
				if (canvas.transform.GetChild (i).gameObject.tag == "mainTextOption") {
					if (canvas.transform.GetChild (i).gameObject.name == textName) {
						canvas.transform.GetChild (i).gameObject.SetActive (true);
					} else {
						canvas.transform.GetChild (i).gameObject.SetActive (false);
					}
				}
			}
		}
	}
	public void startButton(string button){
		if (button == "start") {
			SceneManager.LoadScene ("tutorial1");
		} else if (button == "exit") {
			Application.Quit();
			print ("exit");
		}
	}


}
