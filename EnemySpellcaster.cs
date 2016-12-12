using UnityEngine;
using System.Collections;

public class EnemySpellcaster : MonoBehaviour {

	public Transform spellEffect;
	public float timeBetweenShots = 0.25f;
	private float shootTime;
	private Vector3 crosshairPos;
	private GameObject crosshair;
	private Vector3 mousePos;
	private Transform anchor;

	void Start () {
		Cursor.visible = false;
		shootTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale > 0) {
			if (Time.time > shootTime) {
				shootTime = Time.time + timeBetweenShots;
				Instantiate(spellEffect, transform.position, transform.rotation);
			}
		}
	}
}
