using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public float speed = 5f;
	public int hp = 10;
	public int score = 100;
	public int damage= 5;
	public float chanceToSpawn = 0.3f;
	GameObject GameManager;
	public GameObject powerUpPrefab;
	float spawnPowerUpChance;
	public GameObject explosionPrefab;
	// Use this for initialization
	void Start () {
		GameManager = GameObject.Find ("GameManager");
		spawnPowerUpChance = Random.value;
	}	
	
	// Update is called once per frame
	void Update () {
		Move ();

		if (transform.position.y < -28.5f) {
			destroyShip ();
		}

		GetComponentInChildren<Weapon> ().Fire ();
	}

	void OnTriggerEnter2D(Collider2D col){
		
		if (col.tag == "ProjectileHero") {
			foreach (WeaponDefinition weapontype in Camera.main.GetComponent<Main>().weaponDefinitions) {
				if (col.GetComponent<Projectile> ()._type == weapontype.type) {
					hp -= (int)weapontype.damageOnHit;
				}
			}
			//destroyShip ();
			if(hp<=0){
				if (spawnPowerUpChance <= chanceToSpawn) {
					spawnPowerUp ();
				}
				destroyShip ();

			}
			Destroy (col.gameObject);
		}

		if (col.tag == "Player") {
			destroyShip ();
			col.GetComponentInParent<Ship> ().health -= 10;

		}
	}

	public virtual void Move(){
		Vector3 tempPos = transform.position;
		tempPos.y -= speed * Time.deltaTime;
		transform.position = tempPos;
	}

	public void destroyShip(){
		GameObject explosion= Instantiate (explosionPrefab);
		explosion.transform.position = transform.position;
		Destroy (gameObject);
		if (transform.parent != null) {
			Destroy (transform.parent.gameObject);
		}
	}

	void spawnPowerUp(){
		GameObject powerUp = Instantiate (powerUpPrefab);
		powerUp.transform.position = transform.position;
	}
}
