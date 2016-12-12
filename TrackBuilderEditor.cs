using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(TrackBuilder))]
public class ObjectBuilderEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		
		TrackBuilder myScript = (TrackBuilder)target;
		if(GUILayout.Button("Generate Track")) {
			myScript.BuildTrack();
		}
	}
}
#endif