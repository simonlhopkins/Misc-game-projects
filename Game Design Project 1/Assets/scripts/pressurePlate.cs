using UnityEngine;
using System.Collections;

public class pressurePlate : MonoBehaviour {

	public bool touchingPlayer=false;
	public bool activeAtStart=true;
	[HideInInspector]
	public Vector3 compressionDownVector;
	[HideInInspector]
	public Vector3 compressionUpVector;


	// Use this for initialization
	void Start () {
		compressionDownVector = transform.position- (transform.up/30f);
		compressionUpVector = transform.position+ (transform.up/20f);
		gameObject.SetActive (activeAtStart);
	}

	// Update is called once per frame
	void Update () {
		pushDown ();
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.name == "player") {
			touchingPlayer = true;
		}

	}
	void OnTriggerExit(Collider other){
		touchingPlayer = false;
	}


	public virtual void pushDown(){
		
		if (touchingPlayer) {
			transform.position = Vector3.Lerp (transform.position, compressionDownVector, 0.2f);

		} else {
			transform.position = Vector3.Lerp (transform.position, compressionUpVector, 0.2f);

		}


	}
	void OnDisable() {
		touchingPlayer = false;
	}
}
