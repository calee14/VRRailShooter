using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eOrientationMode { NODE = 0, TANGENT }

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
	public GameObject SplineRoot;
	public float Duration = 10;
	public eOrientationMode OrientationMode = eOrientationMode.NODE;
	public eWrapMode WrapMode = eWrapMode.ONCE;
	public bool AutoStart = true;
	public bool AutoClose = true;
	public bool HideOnExecute = true;
	public bool isGeneratingTrack = true;
	public GameObject crossBeam;
	private bool following = false;
	private float beamTimer;
	public float easeVal = 1.0f;


	SplineInterpolator mSplineInterp;
	Transform[] mTransforms;

	void OnDrawGizmos() {
		Transform[] trans = GetTransforms();
		if (trans.Length < 2) { return; }


		SplineInterpolator interp = GetComponent (typeof(SplineInterpolator)) as SplineInterpolator;
		if (!Application.isPlaying) {
			SetupSplineInterpolator (interp, trans);
			interp.StartInterpolation (null, eRotationMode.PATH_ANGLE, WrapMode);
		}


		Vector3 prevPos = trans[0].position;
		//Quaternion prevRot = trans [0].rotation;
		int numberOfLines = 500;
		for (int c = 1; c <= numberOfLines; c++) {
			float currTime = c * Duration / numberOfLines;
			Vector3 currPos = interp.GetHermiteAtTime(currTime);
			Quaternion currRot = interp.GetRotationAtTime(currTime);
			//float mag = (currPos-prevPos).magnitude * 2;
			Gizmos.color = new Color(0, 0, 1, 1);
			Gizmos.DrawLine(prevPos, currPos);

			Vector3 dP = currPos-prevPos;

			Transform _transform = new GameObject().transform;
			_transform.rotation = currRot;

			Quaternion armDir = Quaternion.LookRotation(dP, _transform.up);
			Vector3 rightArm = armDir * Vector3.right;
			Vector3 leftArm = armDir * Vector3.left;

			Gizmos.color = new Color(0, 1, 0, 1);

			Gizmos.DrawRay(currPos, rightArm);
			Gizmos.DrawRay(currPos, leftArm);

			prevPos = currPos;
			//prevRot = currRot;

			DestroyImmediate(_transform.gameObject);
		}
	}


	void Start()
	{
		mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

		mTransforms = GetTransforms();

		if (HideOnExecute)
			DisableTransforms();

		if (AutoStart) {
			FollowSpline ();
			following = true;
		}
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.Space) && !following) {
			FollowSpline ();
			following = true;
			beamTimer = Time.time + 1.0f;
		}

		if (following && isGeneratingTrack && beamTimer > Time.time) {
			beamTimer = Time.time + 1.0f;
			Instantiate (crossBeam, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 90)));
		}
	}

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		//THIS IS THE LINE!!!
		interp.Reset();

		float step = (AutoClose) ? Duration / trans.Length :
			Duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
			if (OrientationMode == eOrientationMode.NODE)
			{
				interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(Mathf.Sqrt(1 - easeVal*easeVal), easeVal).normalized);
			}
			else if (OrientationMode == eOrientationMode.TANGENT)
			{
				Quaternion rot;
				if (c != trans.Length - 1)
					rot = Quaternion.LookRotation(trans[c + 1].position - trans[c].position, trans[c].up);
				else if (AutoClose)
					rot = Quaternion.LookRotation(trans[0].position - trans[c].position, trans[c].up);
				else
					rot = trans[c].rotation;

				interp.AddPoint(trans[c].position, rot, step * c, new Vector2(Mathf.Sqrt(1 - easeVal*easeVal), easeVal).normalized);
			}
		}

		if (AutoClose)
			interp.SetAutoCloseMode(step * c);
	}


	/// <summary>
	/// Returns children transforms, sorted by name.
	/// </summary>
	Transform[] GetTransforms()
	{
		if (SplineRoot != null)
		{
			List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
			List<Transform> transforms = components.ConvertAll(c => (Transform)c);

			transforms.Remove(SplineRoot.transform);


			return transforms.ToArray();
		}

		return null;
	}

	/// <summary>
	/// Disables the spline objects, we don't need them outside design-time.
	/// </summary>
	void DisableTransforms()
	{
		if (SplineRoot != null)
		{
			SplineRoot.SetActive(false);
		}
	}


	/// <summary>
	/// Starts the interpolation
	/// </summary>
	void FollowSpline()
	{
		if (mTransforms.Length > 0) {
			SetupSplineInterpolator (mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation (null, eRotationMode.PATH_ANGLE, WrapMode);
		}
	}

	void EndOfSpline() 
	{
		if (isGeneratingTrack) 
		{
			Camera.main.SendMessage ("trackFinished");
			isGeneratingTrack = false;
		} 

		following = false;
	}

	public void GenerateTrack()
	{
//		if (isGeneratingTrack) 
//		{
//			mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
//			
//			mTransforms = GetTransforms();
//
//			if (HideOnExecute)
//				DisableTransforms();
//
//			FollowSpline();
//			following = true;
//		}

		GameObject LeftRail = GameObject.Find ("Left Rail");

		LeftRail.transform.position = new Vector3 (50, 50, 50) + LeftRail.transform.position;

//		GameObject go = GameObject.Find("somegameobjectname");
//		ScriptB other = (ScriptB) go.GetComponent(typeof(ScriptB));
//		other.DoSomething();
	}



}