using UnityEngine;
using System.Collections;

public class BossGnome : MonoBehaviour {

	private GameObject player;
	private float dist;
	private bool rising = false;
	private bool falling =  false;
	private bool BossEnter = false;
	private Vector3 init;
	private int Lives = 100;
	void Start () {
		player = GameObject.Find ("SplineFollower");
		init = new Vector3(transform.position.x, transform.position.y + 400.0f, transform.position.z);
	}
	
	
	void Update () {
		dist = Vector3.Distance (transform.position, player.transform.position);
		if (BossEnter && !rising) {
			print("Contact");
			rising = true;
		}
		if (Lives == 0) {
			falling = true;
		}
		if (rising && transform.position.y <= init.y) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z);
		}
		if (falling && (transform.rotation.eulerAngles.x > 270 || transform.rotation.eulerAngles.x == 0)){
			transform.Rotate (new Vector3(1,0,0) * Time.deltaTime * -45);
		}
	}

	void GnomeLess() {
		print("losing");
		Lives--;
	}

	//void GnomeHit() {
	//	GetComponent<AudioSource>().Play();
	//	if (!falling) {
	//		falling = true;
	//		player.SendMessage ("IncreaseScore");
	//	}
	//}

	void EnterBoss() {
		BossEnter = true;
	}
}



/////////////////////////////////////// 
/*
public GameObject target;
private float speed = 5f;


void Start() {

} 

void Update() {
	transform.lookAt(target.transform);
	transform.Translate(Vector3.forward * speed * Time.deltaTime);
}



*/