using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	// This field would be private, but we want to view it in the Inspector
	public WeaponType    _type;
	SpriteRenderer rend;
	public Rigidbody2D rb;
	float initialX;
	float intiialTime;
	Vector3 initialRotation;
	public Sprite playerBulletSprite;
	public Sprite enemyBulletSprite;

	// This public property masks the field _type & takes action when it is set
	public WeaponType    type {
		get {
			return( _type );
		}
		set {
			SetType( value );
		}
	}

	public void SetType( WeaponType eType ) {
		// Set the _type
		_type = eType;
		WeaponDefinition def = Main.GetWeaponDefinition( _type );
		rend.material.color = def.projectileColor;
	}

	void Awake(){
		rend = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

	}

	// Use this for initialization
	void Start () {
		initialX = transform.position.x;
		intiialTime = Time.time;
		initialRotation = new Vector3 (0f, 0f, 0f);
		if (gameObject.tag == "ProjectileHero") {
			rend.sprite = playerBulletSprite;
		} else {
			rend.sprite = enemyBulletSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempPos = transform.position;
	
		switch (type) {
		case WeaponType.sinBlaster:
			tempPos.x = initialX+ Mathf.Sin ((Time.time-intiialTime) * 5f)*10f;
			transform.position = tempPos;
			break;
		case WeaponType.twistBlaster:
			initialRotation.z += 10f;
			transform.rotation = Quaternion.Euler (initialRotation);
			tempPos+=transform.up;
			transform.position = tempPos;
			break;
		}
		if (transform.position.y > 30 || transform.position.y < -30) {
			Destroy (gameObject);
		}
	}
}
