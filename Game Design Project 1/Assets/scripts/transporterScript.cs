using UnityEngine;
using System.Collections;

public class transporterScript : pressurePlate {

	public GameObject destination;

	void Update(){
		pushDown ();
		if (transform.position == compressionDownVector) {
			GameObject.Find("player").transform.position = destination.transform.position + (destination.transform.up/2f);
		}

	}
}