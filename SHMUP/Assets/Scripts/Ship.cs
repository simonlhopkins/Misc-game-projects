using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {
    
    // These fields control the movement of the ship
    public float            speed = 30;
    public float            rollMult = -45;
    public float            pitchMult = 30;
	public Vector3[] weaponPositionArray;
	public int weaponNumber=1;
	public GameObject weaponPrefab;
	public List<GameObject> weaponList;
	public int health=100;
	public GameObject explosionPrefab;
	// Declare a new delegate type WeaponFireDelegate
	public delegate void WeaponFireDelegate();
	// Create a WeaponFireDelegate field named fireDelegate.
	public WeaponFireDelegate fireDelegate;
	void Start(){
		addWeapon ("B");
	}
    void Update () {
    	// Pull in information from the Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        
        // Change transform.position basec on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;	
        
        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0f);
		
		// Use the fireDelegate to fire Weapons
		// First, make sure the button is pressed Axis("Jump")
		// Then ensure that fireDelegate isn't null to avoid an error
		if (Input.GetAxis("Jump") == 1 && fireDelegate != null) {
			fireDelegate();
		}

		if (health <= 0) {
			health = 0;
			GameObject.Find ("GameManager").GetComponent<GameManagerScript> ().gameOver();
			GameObject explosion= Instantiate (explosionPrefab);
			explosion.transform.position = transform.position;
			Destroy (gameObject);

		}

    }

	public void addWeapon(string type){

		if (weaponNumber >= weaponPositionArray.Length) {
			weaponNumber = 0;

		}
		if (weaponNumber < weaponPositionArray.Length) {
			if (weaponList.Count < weaponPositionArray.Length) {
				GameObject newWeapon = Instantiate (weaponPrefab);

				newWeapon.transform.SetParent (transform);
				newWeapon.transform.localPosition = (Vector3)weaponPositionArray.GetValue (weaponNumber);
				newWeapon.transform.localRotation = Quaternion.identity;
				foreach (WeaponDefinition typeOfWeapon in Camera.main.GetComponent<Main>().weaponDefinitions) {
					if (type == typeOfWeapon.letter) {
						newWeapon.GetComponent<Weapon> ()._type = typeOfWeapon.type;
					}
				}
				weaponList.Add (newWeapon);
			} else {
				foreach (WeaponDefinition typeOfWeapon in Camera.main.GetComponent<Main>().weaponDefinitions) {
					if (type == typeOfWeapon.letter) {
						weaponList[weaponNumber].GetComponent<Weapon> ().SetType(typeOfWeapon.type);
					}
				}
			}
		}
		weaponNumber += 1;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ProjectileEnemy") {
			foreach (WeaponDefinition weapontype in Camera.main.GetComponent<Main>().weaponDefinitions) {
				if (col.GetComponent<Projectile> ()._type == weapontype.type) {
					health -= (int)weapontype.damageOnHit;
				}
			}
			Destroy (col.gameObject);
		}
	}

	
}
