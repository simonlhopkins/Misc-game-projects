using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class goalScript : pressurePlate {

	public Text descriptionText; 
	public string newLevelName;
	public enum textNameOptions
	{
		mainText1,
		mainText2,
		mainText3
	};
	public textNameOptions writingOption;
	public string requiredItemToShow;
	public string addedInventoryItem;
	public string removedFromInventory;
	GameObject gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("gameManager");
		if(requiredItemToShow!=""){
			if (gameManager.GetComponent<gameManagerScript> ().playerInventory.Contains (requiredItemToShow)) {
				gameObject.SetActive (true);
			} else {
				gameObject.SetActive (false);
			}
		}
		descriptionText.gameObject.SetActive (false);
		compressionDownVector = transform.position- (transform.up/30f);
		compressionUpVector = transform.position+ (transform.up/20f);
	}
	void OnDisable(){
		if(descriptionText!=null){
			if (descriptionText.gameObject.activeInHierarchy) {
				descriptionText.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		pushDown ();
		if (touchingPlayer) {
			descriptionText.gameObject.SetActive (true);

		} else {
			descriptionText.gameObject.SetActive (false);

		}
		if (Input.GetKeyDown (KeyCode.E) && touchingPlayer) {
			if (addedInventoryItem != "") {
				print("add");
				gameManager.GetComponent<gameManagerScript> ().addToInventory (addedInventoryItem);

			}
			if (removedFromInventory != "") {
				gameManager.GetComponent<gameManagerScript> ().removeFromInventory (removedFromInventory);
			}
			SceneManager.LoadScene (newLevelName);

			gameManager.GetComponent<gameManagerScript> ().mainTextOption = writingOption.ToString();
			if (newLevelName != SceneManager.GetActiveScene ().name) {
				return;
			}

		}

	}
}
