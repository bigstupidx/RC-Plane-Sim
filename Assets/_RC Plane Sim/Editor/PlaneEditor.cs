using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(PlaneAction))]
public class PlaneEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		if(GUILayout.Button("EDITOR"))
		{
			EditorWindow.GetWindow<PlaneEditorWindow>("Planes Editor", true).Show();
			return;
		}
	}
}
