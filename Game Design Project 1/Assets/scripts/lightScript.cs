using UnityEngine;
using System.Collections;

public class lightScript : MonoBehaviour {
	GameObject player;
	Vector3 pointAtPosition;
	public float lightMoveSpeed;
	public float lightRotateSpeed;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position,Camera.main.transform.position,lightMoveSpeed);

		//transform.LookAt(player.transform.position);

		transform.rotation = Quaternion.Lerp (transform.rotation, 
			Quaternion.LookRotation (player.transform.position - transform.position), 
			lightRotateSpeed);

	}
}
