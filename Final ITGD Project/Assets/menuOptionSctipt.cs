using UnityEngine;
using System.Collections;

public class menuOptionSctipt : MonoBehaviour {

	public GameObject unitTypePrefab;
	public GameObject unitDescriptionPrefab;
	GameObject unitDescription;
	float descriptionHeight;
	float descriptionWidth;

	// Use this for initialization
	void Start () {
		descriptionWidth = GetComponent<SpriteRenderer> ().bounds.size.x;
		descriptionHeight = GetComponent<SpriteRenderer> ().bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (unitDescription != null) {
			followMouse (unitDescription);
		}
	}

	void OnMouseDown(){
		if (GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().isPlayerTurn && GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().playerUnitsAvailable>0) {
			Instantiate (unitTypePrefab);
		}
	}

	void OnMouseEnter(){
		unitDescription = Instantiate (unitDescriptionPrefab);
		unitDescription.transform.position = new Vector3(Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
	}
	void OnMouseExit(){
		Destroy (unitDescription);
	}

	void followMouse(GameObject item){
		item.transform.position = new Vector3(Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);

	}
}
