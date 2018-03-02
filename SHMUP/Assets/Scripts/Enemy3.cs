using UnityEngine;
using System.Collections;

public class Enemy3 : Enemy {


	GameObject player;
	// Use this for initialization

	
	public override void Move ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed*Time.deltaTime);

	}

		
}
