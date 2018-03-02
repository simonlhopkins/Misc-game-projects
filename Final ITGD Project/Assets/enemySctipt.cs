using UnityEngine;
using System.Collections;

public class enemySctipt : MonoBehaviour {

	public Vector3 moveDirection;
	public bool enemyDoneMoving;
	public GameObject targetSquare;
	GameObject startingSquare;
	public GameObject tileTouching;
	GameObject tileContainer;
	public int intPoints;
	public int strPoints;
	public Sprite mysterySprite;
	public Sprite actualSprite;
	public GameObject enemyDescriptionPrefab;
	public GameObject mysteryDescription;
	public bool isMonked;
	float flashCounter=0;

	// Use this for initialization
	void Start () {
		isMonked = false;
		enemyDoneMoving = true;
		tileContainer = GameObject.Find ("tileContainer");
		GetComponent<SpriteRenderer> ().sprite = mysterySprite;

	}
	
	// Update is called once per frame
	void Update () {
		monkFlash (Time.deltaTime*3600);

	}


	public void Move(){
		findTouchingTile();

		if (enemyDoneMoving == true) {
			findStartingSquare ();
			findTargetSquare ();
		}

		if (transform.position != targetSquare.transform.position) {
			
			Vector3 tempPos = transform.position;

			tempPos.x = Mathf.Lerp (transform.position.x, targetSquare.transform.position.x, 0.2f);
			tempPos.y = Mathf.Lerp (transform.position.y, targetSquare.transform.position.y, 0.2f);
			transform.position = tempPos;
			enemyDoneMoving = false;
		} else {
			GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().enemyIndexTurn += 1;
			enemyDoneMoving = true;

		}

	}

	void findTargetSquare(){
		for (int i=0; i<GameObject.Find("tileContainer").transform.childCount;i++) {
			if (GameObject.Find ("tileContainer").transform.GetChild(i).GetComponent<Collider2D> ().bounds.Contains (transform.position + moveDirection)) {
				targetSquare = GameObject.Find ("tileContainer").transform.GetChild (i).gameObject;
			}
		}
	}
	void findStartingSquare(){
		for (int i=0; i<GameObject.Find("tileContainer").transform.childCount;i++) {
			if (GameObject.Find ("tileContainer").transform.GetChild(i).GetComponent<Collider2D> ().bounds.Contains (transform.position)) {
				startingSquare = GameObject.Find ("tileContainer").transform.GetChild (i).gameObject;
			}
		}
	}
	void findTouchingTile(){
		
		for (int i = 0; i < tileContainer.transform.childCount; i++) {
			if (tileContainer.transform.GetChild (i).GetComponent<Collider2D> ().bounds.Contains (transform.position)) {
				tileTouching = tileContainer.transform.GetChild (i).gameObject;
				return;
			}

		}
		tileTouching = null;
	}

	void monkFlash(float delay){
		if (isMonked) {
			if (GetComponent<SpriteRenderer> ().enabled) {
				if (flashCounter > delay) {
					flashCounter = 0;
					GetComponent<SpriteRenderer> ().enabled = !GetComponent<SpriteRenderer> ().enabled;
				} else {
					flashCounter++;
				}
			} else {
				if (flashCounter > delay/16) {
					flashCounter = 0;
					GetComponent<SpriteRenderer> ().enabled = !GetComponent<SpriteRenderer> ().enabled;
				} else {
					flashCounter++;
				}
			}
		}
	}

}	
