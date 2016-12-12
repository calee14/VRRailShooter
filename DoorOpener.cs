using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {

	public float direction;
	public float speed = 20.0f;
	private bool doorsOpening;
	private bool doorsClosing;

	void Start () {
		doorsOpening = false;
		doorsClosing = false;
	}
	
	
	void Update () {
		if (doorsOpening) {
			if (direction == 1) {
				if (transform.rotation.eulerAngles.y < 90) {
					transform.RotateAround (transform.position, Vector3.up, speed * direction * Time.deltaTime);
				} else {
					doorsOpening = false;
					doorsClosing = true;
				}
			} else {
				if (transform.rotation.eulerAngles.y > 270 || transform.rotation.eulerAngles.y == 0) {
					transform.RotateAround (transform.position, Vector3.up, speed * direction * Time.deltaTime);
				} else {
					doorsOpening =  false;   
					doorsClosing = true;  
			}
		}
	}
	if (doorsClosing) {
		if (direction == 1) {
			if (transform.rotation.eulerAngles.y > 0 && transform.rotation.eulerAngles.y < 359) {
				transform.RotateAround (transform.position, Vector3.up, speed * -direction * Time.deltaTime);
			} else {
				doorsClosing = false;
			}
		} else {
			if (transform.rotation.eulerAngles.y > 210) {
				transform.RotateAround (transform.position, Vector3.up, speed * -direction * Time.deltaTime);
			} else {
				doorsClosing = false;
			}
		}	
	}
}

	void OpenDoors() {
		transform.RotateAround (transform.position, Vector3.up, speed * direction *Time.deltaTime);
		doorsOpening = true;
	}

}