using UnityEngine;
using System.Collections;

public class Enemy4 : Enemy {


	GameObject player;
	// Use this for initialization


	public override void Move ()
	{
		Vector3 tempPos = transform.position;
		tempPos.x = transform.position.x + Mathf.Sin (Time.time*speed);
		transform.position = tempPos;
		base.Move ();
	}


}
