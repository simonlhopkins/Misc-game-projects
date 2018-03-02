using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buttonScript : pressurePlate {
	public List <GameObject> connectedFeatures;
	bool canBeTouched=false;
	bool buttonTouched=false;
	Collider col;
	GameObject player;
	bool playerFalling = false;
	Vector3 startingPos;
	AudioSource buttonSound;
	// Use this for initialization
	void Awake () {
		
		buttonSound = GetComponent<AudioSource> ();
		compressionDownVector = transform.position- (transform.up/30f);
		compressionUpVector = transform.position+ (transform.up/30f);

		player = GameObject.Find ("player");

		/*
		foreach (GameObject feature in connectedFeatures) {
			if (feature.gameObject.tag == "floor") {
				feature.SetActive (feature.GetComponent<floorTileScript> ().activeAtStart);
			} else {
				feature.SetActive (feature.GetComponent<pressurePlate> ().activeAtStart);

			}
		}
		*/

	}
	
	// Update is called once per frame
	void Update () {
		pushDown ();
		if(Mathf.Round(player.GetComponent<Rigidbody>().velocity.y*100f)/100f<0){
			playerFalling=true;
		} else{
			playerFalling=false;
		}

	}

	void changeAllFeatures(){
		
		if (!Camera.main.GetComponent<cameraScript> ().cameraChanging) {
			foreach (GameObject feature in connectedFeatures) {
				bool newActive = !feature.activeInHierarchy;
				feature.SetActive (newActive);

			}
		}
	}



	void OnTriggerEnter(Collider other){
		/*if (other.gameObject.name == "player" && !playerFalling) {
			buttonTouched = true;

		}
		*/

		/*
		if (other.gameObject.name == "player" && !playerFalling) {
			if ((transform.position - compressionDownVector).magnitude > (compressionUpVector - compressionDownVector).magnitude-0.01) {
				canBeTouched = true;

			} else {
				canBeTouched = false;

			}
		}
		*/
	}

	bool isGoingDown = false;

	public override void pushDown(){
		if (transform.position == compressionUpVector && touchingPlayer) {
			isGoingDown = true;
			buttonSound.Play ();
		}
		if (touchingPlayer && isGoingDown && !playerFalling && !Camera.main.GetComponent<cameraScript>().cameraChanging) {
			//button going down

			transform.position = Vector3.Lerp (transform.position, compressionDownVector, 0.4f);
			isGoingDown = true;
			if (transform.position == compressionDownVector) {
				isGoingDown = false;
				changeAllFeatures ();
			}
	
		
		} else if(!touchingPlayer && isGoingDown){
			transform.position = Vector3.Lerp (transform.position, compressionDownVector, 0.4f);
			isGoingDown = true;
			if (transform.position == compressionDownVector) {
				isGoingDown = false;
				changeAllFeatures ();
			}
			
		}else if (!touchingPlayer && !isGoingDown) {
			isGoingDown = false;
			transform.position = Vector3.Lerp (transform.position, compressionUpVector, 0.4f);

		}


		/*
		if (canBeTouched) {
			
			transform.position = Vector3.Lerp (transform.position, compressionDownVector, 0.4f);
			if (transform.position == compressionDownVector) {
				changeAllFeatures ();
				canBeTouched = false;
			}

		}else {
			transform.position = Vector3.Lerp (transform.position, compressionUpVector, 0.5f);
			canBeTouched = false;

		}
		*/
	}
}
