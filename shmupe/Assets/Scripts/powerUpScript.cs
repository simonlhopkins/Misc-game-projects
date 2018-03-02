using UnityEngine;
using System.Collections;

public class powerUpScript : MonoBehaviour {

	// Use this for initialization
	TextMesh powerUpText;
	public float powerUpChooser;
	void Start () {
		powerUpText = transform.GetChild (0).GetComponent<TextMesh> ();
		powerUpChooser = Random.Range (0, Camera.main.GetComponent<Main> ().weaponDefinitions.Length);
		powerUpText.text = Camera.main.GetComponent<Main> ().weaponDefinitions [(int)powerUpChooser].letter;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempPos = transform.position;
		tempPos.y -= 5f * Time.deltaTime;
		transform.position = tempPos;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player") {
			col.GetComponentInParent<Ship> ().addWeapon (powerUpText.text);
			Destroy (gameObject);
			
		}
	}
}
