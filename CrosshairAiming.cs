using UnityEngine;
using System.Collections;

public class CrosshairAiming : MonoBehaviour {

	private GameObject crosshair;
	private Vector3 mousePos;
	private Vector3 worldPos;

	void Start () {
		Cursor.visible = false;

		crosshair = GameObject.Find("Crosshair");
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Input.mousePosition;
		mousePos.z = 2.0f;
		worldPos = Camera.main.ScreenToWorldPoint (mousePos);
		crosshair.transform.position = worldPos;
	}
}
