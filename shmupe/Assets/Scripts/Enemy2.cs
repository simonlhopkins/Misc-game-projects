using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy {


	public float waveAmplitude= 5f;
	public float waveFrequency= 5f;
	float distanceToChange=2f;
	public GameObject[] wayPointList;
	public int currentWaypoint=0;
	// Use this for initialization


	public override void Move(){
		Vector3 tempPos = transform.position;

		if (currentWaypoint < wayPointList.Length) {
			if (wayPointList [currentWaypoint].transform.position != tempPos) {
				tempPos = Vector3.MoveTowards (tempPos, wayPointList [currentWaypoint].transform.position, speed * Time.deltaTime);
			} else {
				currentWaypoint += 1;
			}
		}
		transform.position = tempPos;
	}
		
}
