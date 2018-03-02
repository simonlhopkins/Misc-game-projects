using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPieceScript : MonoBehaviour {

	bool isTouchingTile;
	public bool isHeldClicked;
	public bool isFirstSpawn;
	public bool setOntile;
	public GameObject tileTouching;
	GameObject tileContainer;
	Vector3 mousePos;
	public bool moveSquare = false;
	GameObject targetSpace;
	public List<Vector3> distanceCoordinates;
	public List<GameObject> potentialSpaces;
	public bool isCurrentPlayer;
	public bool showTiles=true;
	public int intPoints;
	public int strPoints;
	public int requiredMovementPoints;
	// Use this for initialization
	void Awake(){
		tileContainer = GameObject.Find ("tileContainer");
	}
	void Start () {
		isFirstSpawn = true;
		setOntile = false;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().isPlayerTurn) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (Input.GetMouseButton (0)) {
				isHeldClicked = true;


			} else {
				isHeldClicked = false;
			}
			if (gameObject == GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().currentPlayerPiece) {
				isCurrentPlayer = true;
				GetComponent<SpriteRenderer> ().color = Color.yellow;

			} else {
				isCurrentPlayer = false;
				GetComponent<SpriteRenderer> ().color = Color.white;
			}
			if (isFirstSpawn) {
				followMouse ();
			}
			if (!setOntile) {
				findTouchingTile ();
			}
			if (isCurrentPlayer) {
				if (GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().movementPoints > 0) {
					movePlayer ();
				}
				if (!moveSquare) {
					foreach (GameObject playerPiece in GameObject.FindGameObjectsWithTag("Player")) {
						playerPiece.GetComponent<PlayerPieceScript> ().findMovementSpaces ();

					}
				}

			}
		}

	}

	void followMouse(){
		Vector3 tempPos = transform.position;
		tempPos = mousePos;
		tempPos.z = 0f;
		transform.position = tempPos;
		layTileDown ();
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
	void layTileDown(){
		if (!isHeldClicked) {
			if (tileTouching != null) {
				if (isFirstSpawn) {
					if (tileTouching.tag == "playerSpawner" && tileTouching.gameObject.GetComponent<tileScript>().hasPlayer==false) {
						GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().setCurrentPlayerPiece (gameObject);
						transform.position = tileTouching.transform.position;
						setOntile = true;
						isFirstSpawn = false;
						tileTouching.GetComponent<tileScript> ().hasPlayer = true;
						findMovementSpaces ();
						GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().playerUnitsAvailable -= 1;

					} else {
						Destroy (gameObject);
					}
				} else {
					transform.position = tileTouching.transform.position;
					setOntile = true;
					isFirstSpawn = false;
					tileTouching.GetComponent<tileScript> ().hasPlayer = true;
					GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().playerUnitsAvailable -= 1;

				}
			} else {
				Destroy (gameObject);
			}
		}
	}

	public void findMovementSpaces(){
		foreach (GameObject space in potentialSpaces) {
			space.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		potentialSpaces.Clear ();
		distanceCoordinates.Clear ();
		distanceCoordinates.Add (new Vector3 (transform.position.x + 0.33f,transform.position.y,0));
		distanceCoordinates.Add (new Vector3 (transform.position.x - 0.33f,transform.position.y,0));
		distanceCoordinates.Add (new Vector3 (transform.position.x,transform.position.y + 0.33f,0));
		distanceCoordinates.Add (new Vector3 (transform.position.x,transform.position.y - 0.33f,0));
		foreach (Vector3 position in distanceCoordinates) {
			for (int i = 0; i < tileContainer.transform.childCount; i++) {
				if (tileContainer.transform.GetChild (i).GetComponent<Collider2D> ().bounds.Contains (position) && tileContainer.transform.GetChild (i).GetComponent<tileScript>().hasPlayer==false) {
					potentialSpaces.Add (tileContainer.transform.GetChild (i).gameObject);
					if (tileContainer.transform.GetChild (i).GetComponent<tileScript> ().hasEnemy && setOntile) {
						tileContainer.transform.GetChild (i).GetComponent<tileScript> ().enemyOnTile.GetComponent<SpriteRenderer> ().sprite = tileContainer.transform.GetChild (i).GetComponent<tileScript> ().enemyOnTile.GetComponent<enemySctipt> ().actualSprite;
					}

				}

			}
		}
		GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().colorTiles ();

	}

	void movePlayer(){
		findTouchingTile ();
		if (setOntile) {
			foreach (GameObject potSpace in potentialSpaces) {
				if (potSpace.GetComponent<tileScript> ().isClicked) {
					moveSquare = true;
					targetSpace = potSpace;
				}
			}
		}
		if (moveSquare && GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().movementPoints >= requiredMovementPoints) {
			Vector3 tempPos = transform.position;
			tempPos.x = Mathf.Lerp (transform.position.x, targetSpace.transform.position.x, 0.2f);
			tempPos.y = Mathf.Lerp (transform.position.y, targetSpace.transform.position.y, 0.2f);
			transform.position = tempPos;
			if (transform.position == targetSpace.transform.position) {
				moveSquare = false;
				GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().movementPoints -= requiredMovementPoints;


			}
		} else {
			moveSquare = false;
		}
	}
		
	void OnMouseDown(){
		GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().setCurrentPlayerPiece (gameObject);
		GameObject.Find ("GameManager").GetComponent<gameManagerScript> ().colorTiles ();
	}
		
}
