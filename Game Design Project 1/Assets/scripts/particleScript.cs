using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour {
	public float particleSpeed;
	float height;
	float size;
	public float maxHeight;
	public float minHeight;
	public float maxSize;
	public float minSize;
	// Use this for initialization
	void Start () {
		height = (float)Random.Range (maxHeight, minHeight);
		size = (float)Random.Range (minSize, maxSize);
		transform.localScale = new Vector3 (size, size, size);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null && !transform.parent.GetComponentInChildren<goalScript>().touchingPlayer) {
			transform.position += (transform.parent.transform.up * Time.deltaTime * (particleSpeed/10f));
			if (transform.position.y > transform.parent.position.y+height) {
				Destroy (gameObject);
			}
			changeWhileMoving ();
			if (transform.localScale.x < size) {
				transform.localScale+=new Vector3 (Time.deltaTime, Time.deltaTime, Time.deltaTime);
			}
		}
	}

	void changeWhileMoving(){
		transform.rotation *= Quaternion.AngleAxis(Time.deltaTime*100f, Vector3.up);
		transform.rotation *= Quaternion.AngleAxis(Time.deltaTime*100f, Vector3.right);
		transform.rotation *= Quaternion.AngleAxis(Time.deltaTime*100f, Vector3.forward);
	}

}
