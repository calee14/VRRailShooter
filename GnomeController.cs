using UnityEngine;
using System.Collections;

public class GnomeController : MonoBehaviour {

	private GameObject player;
	private float dist;
	private bool rising = false;
	private bool falling =  false;
	private Vector3 init;
	void Start () {
		player = GameObject.Find ("SplineFollower");
		init = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z);
	}
	
	
	void Update () {
		dist = Vector3.Distance (transform.position, player.transform.position);
		if (dist <= 100.0f && !rising) {
			print("Contact");
			rising = true;
		}
		if (rising && transform.position.y <= init.y) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 0.2f, transform.position.z);
		}
		if (falling && (transform.rotation.eulerAngles.x > 270 || transform.rotation.eulerAngles.x == 0)){
			transform.Rotate (new Vector3(1,0,0) * Time.deltaTime * -45);
		}
	}

	void GnomeHit() {
		GetComponent<AudioSource>().Play();
		if (!falling) {
			falling = true;
			player.SendMessage ("IncreaseScore");
		}
	}
}
