using UnityEngine;
using System.Collections;

public class spawnerScript : tileScript {

	public char pos;
	// Use this for initialization
	void Start () {
		if (transform.position.x > 0) {
			pos = 'R';

		} else {
			if (transform.position.y > 0) {
				pos = 'T';
			} else {
				pos = 'B';
			}
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "enemy") {
			col.gameObject.GetComponent<enemySctipt>().moveDirection=setEnemyDirection (pos);

		}
	}
}
