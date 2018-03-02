using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

	Rigidbody rb;
	public float speed;
	GameObject spawnPoint;
	public float fallSpeedDeath;
	float maxSpeed;
	float minSpeed;
	public float maxMoveDistance;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		spawnPoint = GameObject.FindGameObjectWithTag ("spawnPoint");
		maxSpeed = speed;
		minSpeed = speed / 4f;
		transform.position = spawnPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
		if (Input.GetKeyDown (KeyCode.R)) {
			restartLevel ();
		}
	}

	void Movement(){
		//need to make sure you can do either key presses or mouse click not both
		rb.rotation = Quaternion.Euler(new Vector3(0,
			Camera.main.transform.eulerAngles.y-135f,
			0));
		if (Input.GetMouseButton (0)) {
			Vector3 mousePos = Input.mousePosition;
			Vector3 playerScreenPos = Camera.main.WorldToScreenPoint (transform.position);
			mousePos.z = 0;
			playerScreenPos.z = 0;

			if (!Camera.main.GetComponent<cameraScript> ().cameraChanging) {

				Vector3 screenSize = new Vector3 ((float)Screen.width, (float)Screen.height, 0);
				Vector3 mouseToPlayerVector = mousePos - playerScreenPos;
				Vector3 movementVector = new Vector3 (mouseToPlayerVector.x / screenSize.x, mouseToPlayerVector.y / screenSize.y, 0f);
				float moveDistance = movementVector.magnitude * speed;
				if (moveDistance > maxMoveDistance) {
					moveDistance = maxMoveDistance;
				}
				if (mousePos.y >= playerScreenPos.y && mousePos.x > playerScreenPos.x) {
					rb.position -= (Mathf.Abs (moveDistance)) * transform.forward * Time.deltaTime * speed;
				} else if (mousePos.y < playerScreenPos.y && mousePos.x <= playerScreenPos.x) {
					rb.position += (Mathf.Abs (moveDistance)) * transform.forward * Time.deltaTime * speed;
				} else if (mousePos.y < playerScreenPos.y && mousePos.x >= playerScreenPos.x) {
					rb.position -= (Mathf.Abs (moveDistance)) * transform.right * Time.deltaTime * speed;
				} else if (mousePos.y > playerScreenPos.y && mousePos.x < playerScreenPos.x) {
					rb.position += (Mathf.Abs (moveDistance)) * transform.right * Time.deltaTime * speed;

				}
			}
		} else if(Input.anyKey) {
			
			float v = Input.GetAxis ("Vertical");
			float h = Input.GetAxis ("Horizontal");
			/*rb.rotation = Quaternion.Euler(new Vector3(0,
				Camera.main.transform.eulerAngles.y-135f,
				0));
				*/

			if (!Camera.main.GetComponent<cameraScript> ().cameraChanging) {
				rb.position -= h * transform.forward * Time.deltaTime*maxMoveDistance;
				rb.position += v * transform.right * Time.deltaTime*maxMoveDistance;
			}

		}
		if (rb.position.y < -4f) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.rotation = Quaternion.Euler(new Vector3 (0, 180f, 0));
			transform.position = spawnPoint.transform.position;
			speed = maxSpeed;

		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "transporter") {
			speed = minSpeed;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "transporter") {
			speed = maxSpeed;
		}
	}
	void restartLevel(){
		foreach (GameObject GO in Resources.FindObjectsOfTypeAll(typeof(GameObject))) {
			if (GO.tag == "floor") {
				GO.SetActive (GO.GetComponent<floorTileScript>().activeAtStart);
			}


		}
		foreach (GameObject button in GameObject.FindGameObjectsWithTag("button")) {
			foreach (GameObject feature in button.GetComponent<buttonScript>().connectedFeatures) {
				if (feature.gameObject.tag == "floor") {
					feature.SetActive (feature.GetComponent<floorTileScript> ().activeAtStart);
				} else {
					feature.SetActive (feature.GetComponent<pressurePlate> ().activeAtStart);

				}
			}
		}
		transform.position = spawnPoint.transform.position;
	}

}
