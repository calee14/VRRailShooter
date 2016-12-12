using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public int count;
	private Text scoreText;
	private GameObject Door1;
	private GameObject Door2;
	private GameObject endDoor1;
	private GameObject endDoor2;
	private GameObject bossGnome;

	void Start () {
		bossGnome = GameObject.Find("boss gnome");
		GetComponent<AudioSource>().Play();
		Door1 = GameObject.Find ("Door 1");
		Door2 = GameObject.Find ("Door 2");
		endDoor1 = GameObject.Find ("EndDoor1");
		endDoor2 = GameObject.Find ("EndDoor2");
		scoreText = GameObject.Find ("Score Text").GetComponent<Text>();
		count = 0;
	}

	void IncreaseScore() {
		count++;
		scoreText.text = count.ToString ();
		if (count >= 3) {
			scoreText.rectTransform.localPosition = Vector3.zero;
			scoreText.text = "ALL GNOMES DOWN!";
			bossGnome.SendMessage("EnterBoss");
		}
		if (count >= 3) {
			scoreText.SendMessage("GnomeRise");
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "StartDoorTrig") {
			Door1.SendMessage ("OpenDoors");
			Door2.SendMessage ("OpenDoors");
		}
		if (other.gameObject.tag == "EndDoorTrig") {
			endDoor1.SendMessage("OpenDoors");
			endDoor2.SendMessage("OpenDoors");
		}
	}
}
