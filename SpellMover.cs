using UnityEngine;
using System.Collections;

public class SpellMover : MonoBehaviour {

	public float speed = 1.0f;

	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Target") {
			other.gameObject.SendMessage("GnomeHit");
			Destroy(this.gameObject);
		}
		if (other.gameObject.tag == "Boss") {
			other.gameObject.SendMessage("GnomeLess");
			Destroy(this.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		print("Other");
		if (other.gameObject.tag == "Boundary") {
			print("Other destroyed");
			Destroy (this.gameObject);
		}
	}
}
