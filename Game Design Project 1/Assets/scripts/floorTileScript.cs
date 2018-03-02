using UnityEngine;
using System.Collections;

public class floorTileScript : MonoBehaviour {

	public bool activeAtStart;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (activeAtStart);
		if (transform.childCount > 0) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).gameObject.tag == "button") {
					return;
				}
			}
		}
		randomRotation ();

	}

	void randomRotation(){
		
		float startDir = (int)Random.Range (0, 4) * 90f;
		float scaleNumX = Random.Range (0, 2);
		float scaleNumZ = Random.Range (0, 2);
		transform.rotation = Quaternion.Euler (new Vector3 (0, startDir, 0));
		Vector3 newRot = new Vector3 (1, 1, 1);
		if (scaleNumX < 1f) {
			newRot = Vector3.Scale(newRot, new Vector3 (-1f, 1f, 1f));
		}
		if (scaleNumZ < 1f) {
			newRot = Vector3.Scale(newRot, new Vector3 (1f, 1f, -1f));
		}
		transform.localScale = newRot;
	}
}
