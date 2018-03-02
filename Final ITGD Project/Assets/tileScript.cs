using UnityEngine;
using System.Collections;

public class tileScript : MonoBehaviour {

	public bool hasEnemy=false;
	public bool hasPlayer=false;
	public bool isClicked = false;
	public GameObject playerOnTile;
	public GameObject enemyOnTile;
	GameObject enemyDescription;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
	void OnTriggerStay2D(Collider2D col){
		
		if (col.gameObject.tag == "enemy") {
			
			hasEnemy = true;
			enemyOnTile = col.gameObject;
		}
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<PlayerPieceScript> ().setOntile) {
				hasPlayer = true;
				gameObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
				playerOnTile = col.gameObject;
			}
		}

	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "enemy") {
			hasEnemy = false;
		}
		if (col.gameObject.tag == "Player") {
			
			hasPlayer = false;
			gameObject.layer = LayerMask.NameToLayer ("tileLayer");
		}

	}

	public Vector3 setEnemyDirection(char direction){
		Vector3 directionVec= new Vector3(0,0,0);
		switch (direction) {
		case 'R':
			directionVec = new Vector3 (-0.64f, 0f, 0f);
			break;
		case 'T':
			directionVec = new Vector3 (0, -0.64f, 0);
			break;
		case 'B':
			directionVec = new Vector3 (0, 0.64f, 0);
			break;
		}

		return directionVec;
	}
	void OnMouseDown(){
		if (Input.GetMouseButtonDown(0)) {
			isClicked = true;
		}

	}
	void OnMouseUp(){
		isClicked = false;
	}

	void OnMouseEnter(){
		if (hasEnemy) {
			if (enemyOnTile.GetComponent<SpriteRenderer> ().sprite.name == "mysteryEnemy") {
				enemyDescription = Instantiate (enemyOnTile.GetComponent<enemySctipt> ().mysteryDescription);
				enemyDescription.transform.position = new Vector3 (4f, 5f, 0);
			} else {
				enemyDescription = Instantiate (enemyOnTile.GetComponent<enemySctipt> ().enemyDescriptionPrefab);
				enemyDescription.transform.position = new Vector3 (4f, 5f, 0);
			}

		}
	}

	void OnMouseExit(){
		Destroy (enemyDescription);
	}
}
