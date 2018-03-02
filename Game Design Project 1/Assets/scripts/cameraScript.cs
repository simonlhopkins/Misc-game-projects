using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class cameraScript : MonoBehaviour {

	public List<GameObject> camPositions;
	public int camPosIndex=0;
	public bool cameraChanging=false;
	// Use this for initialization
	void Start () {
		transform.position = camPositions[camPosIndex].transform.position;
		transform.rotation = camPositions[camPosIndex].transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		changeCameraAngle ();
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (camPosIndex < camPositions.Count-1) {
				camPosIndex += 1;

			} else {
				camPosIndex = 0;
			}
			cameraChanging = true;
		}

	}
	void changeCameraAngle(){
		if (cameraChanging) {
			Vector3 currentPos = transform.position;
			Quaternion currentRot = transform.rotation;
			Vector3 targetPos = camPositions [camPosIndex].transform.position;
			Quaternion targetRot = camPositions [camPosIndex].transform.rotation;
			Vector3 newPos = Vector3.Lerp (currentPos, targetPos, 0.1f);
			Quaternion newRot = Quaternion.Lerp (currentRot, targetRot, 0.1f);


			transform.position = newPos;
			transform.rotation = newRot;
			if ((targetPos - currentPos).magnitude<0.05f) {
				if ((targetRot.eulerAngles - currentRot.eulerAngles).magnitude < 0.05f) {
					
					cameraChanging = false;

				}

			}
		}
		if (!cameraChanging) {
			transform.position = camPositions [camPosIndex].transform.position;
			transform.rotation = camPositions [camPosIndex].transform.rotation;

		}
	}
	void LateUpdate(){
		
	}
}
