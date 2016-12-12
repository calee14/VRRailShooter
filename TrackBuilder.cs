using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBuilder : MonoBehaviour {
	public GameObject SplineRoot;
	public bool AutoClose;
	public GameObject LeftRailPrefab;
	public GameObject RightRailPrefab;
	public GameObject crossBeamPrefab;
	public float beamDistance;
	public float resolution;



	/// <summary>
	/// Returns children transforms in the order they appear inside of the editor in the parent.
	/// Returns null if SplineRoot == null
	/// </summary>
	Transform[] GetTransforms() {
		// Guard against not having a spline root
		if (SplineRoot == null) { return null; }

		List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
		List<Transform> transforms = components.ConvertAll(c => (Transform)c);
		
		transforms.Remove(SplineRoot.transform);

		return transforms.ToArray();
	}

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans) {
		interp.Reset();

		int c;
		for (c = 0; c < trans.Length; c++) {
			interp.AddPoint(trans[c].position, trans[c].rotation, c, new Vector2(0, 1));
		}
		
		if (AutoClose) {
			interp.SetAutoCloseMode (c);
		}
	}

	public void BuildTrack() {
		// Delete all of the children of the track holding game object
		List<Component> childComponents = new List<Component>(GetComponentsInChildren(typeof(Transform)));
		List<Transform> childTransforms = childComponents.ConvertAll(c => (Transform)c);
		childTransforms.Remove(transform);
		foreach (Transform childTransform in childTransforms) {
			if (childTransform.gameObject.name == "left rail" || childTransform.gameObject.name == "right rail") {
				DestroyImmediate(childTransform.gameObject.GetComponent<MeshFilter>().sharedMesh);
			}
			DestroyImmediate(childTransform.gameObject);
		}

		// Get all of the spline node information from the splineRoot
		Transform[] splineNodeTransforms = GetTransforms();
		if (splineNodeTransforms.Length < 2) { return; }

		// Build the spline interpolator object
		SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
		SetupSplineInterpolator(interp, splineNodeTransforms);
		interp.StartInterpolation(null, eRotationMode.PATH_ANGLE, eWrapMode.ONCE);

		// Build a list of affine transformation matricies that represent the track sections
		List<Matrix4x4> leftTrackPolyline = new List<Matrix4x4>();
		List<Matrix4x4> rightTrackPolyline = new List<Matrix4x4>();

		float tMax = AutoClose ? splineNodeTransforms.Length : splineNodeTransforms.Length - 1;
		tMax += 2 * resolution;

		for (float t = 0; t < tMax; t += resolution) {
			Transform  trans = new GameObject().transform;

			trans.position = interp.GetHermiteAtTime(t);
			trans.rotation = interp.GetPathAngleAtTime(t);

			leftTrackPolyline.Add(trans.localToWorldMatrix * LeftRailPrefab.transform.localToWorldMatrix);
			rightTrackPolyline.Add(trans.localToWorldMatrix * RightRailPrefab.transform.localToWorldMatrix);

			//Debug.Log(trans.localToWorldMatrix);
			DestroyImmediate(trans.gameObject);
		}

		// Generate the rails
		GameObject leftRail = new GameObject ();
		Mesh leftMesh = new Mesh();
		leftRail.name = "left rail";
		leftRail.transform.parent = transform;
		leftRail.AddComponent<MeshFilter>();
		leftRail.GetComponent<MeshFilter>().sharedMesh = leftMesh;
		leftRail.AddComponent<MeshRenderer>();
		leftRail.GetComponent<MeshRenderer> ().sharedMaterial = LeftRailPrefab.GetComponent<MeshRenderer>().sharedMaterial;
		MeshExtrusion.ExtrudeMesh (LeftRailPrefab.GetComponent<MeshFilter>().sharedMesh, leftRail.GetComponent<MeshFilter>().sharedMesh, leftTrackPolyline.ToArray(), false);

		GameObject rightRail = new GameObject ();
		Mesh rightMesh = new Mesh();
		rightRail.name = "right rail";
		rightRail.transform.parent = transform;
		rightRail.AddComponent<MeshFilter>();
		rightRail.GetComponent<MeshFilter>().sharedMesh = rightMesh;
		rightRail.AddComponent<MeshRenderer>();
		rightRail.GetComponent<MeshRenderer> ().sharedMaterial = RightRailPrefab.GetComponent<MeshRenderer>().sharedMaterial;
		MeshExtrusion.ExtrudeMesh (RightRailPrefab.GetComponent<MeshFilter>().sharedMesh, rightRail.GetComponent<MeshFilter>().sharedMesh, rightTrackPolyline.ToArray(), false);

		// Generate the cross bars
		float distSinceLastCrossbar = 0;
		float cbRes = resolution / 5.0f;
		for (float t = cbRes; t < tMax; t += cbRes) {
			Vector3 dP = interp.GetHermiteAtTime(t) - interp.GetHermiteAtTime(t-cbRes);
			distSinceLastCrossbar += dP.magnitude;
			if (distSinceLastCrossbar >= beamDistance) {
				GameObject crossbar = Instantiate(crossBeamPrefab);
				crossbar.transform.parent = transform;
				crossbar.transform.position = interp.GetHermiteAtTime(t);
				crossbar.transform.rotation = interp.GetPathAngleAtTime(t);

				crossbar.transform.position += crossbar.transform.right*crossBeamPrefab.transform.position.x;
				crossbar.transform.position += crossbar.transform.up*crossBeamPrefab.transform.position.y;
				crossbar.transform.position += crossbar.transform.forward*crossBeamPrefab.transform.position.z;

				crossbar.transform.rotation *= crossBeamPrefab.transform.rotation;

				distSinceLastCrossbar -= beamDistance;
			}
		}
	}
}
