using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSpawnerScript : MonoBehaviour {

	public GameObject particlePrefab;
	public float maxWidth;
	float minWidth;
	float interval;
	public float minInterval;
	public float maxInterval;
	float startTime;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		interval = Random.Range (minInterval, maxInterval);
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.parent.GetComponentInChildren<goalScript> ().touchingPlayer) {
			spawnCube ();
		} else {
			destroyCubes ();
		}
	}

	void spawnCube(){
		if ((Time.time - startTime) > interval) {
			interval = Random.Range (minInterval, maxInterval);
			startTime = Time.time;
			GameObject particle=Instantiate (particlePrefab);
			particle.transform.SetParent (transform.parent);
			particle.transform.position += new Vector3 (
				transform.parent.transform.position.x+(float)Random.Range (-maxWidth, maxWidth),
				transform.parent.transform.position.y,
				transform.parent.transform.position.z+(float)Random.Range (-maxWidth, maxWidth));
			
			particle.transform.rotation = Quaternion.Euler (new Vector3 (
				Random.Range (-45f, 45f),
				Random.Range (-45f, 45f),
				Random.Range (-45f, 45f)));
			
		}
	}

	void destroyCubes(){
		for (int i = 0; i < transform.parent.childCount; i++) {
			if (transform.parent.GetChild (i).gameObject.tag == "partical") {
				transform.parent.GetChild (i).transform.localScale -= new Vector3 (Time.deltaTime/10f, Time.deltaTime/10f, Time.deltaTime/10f);
				if (transform.parent.GetChild (i).transform.localScale.x < 0) {
					Destroy (transform.parent.GetChild (i).gameObject);
				}
			}
		}
	}

}
