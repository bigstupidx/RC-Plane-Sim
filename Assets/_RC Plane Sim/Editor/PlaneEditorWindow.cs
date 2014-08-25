using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class PlaneEditorWindow : EditorWindow 
{
	private Vector2 mScrollView = new Vector2();
	private bool[] fold = new bool[7];
	
	void OnGUI()
	{
		var target = FindObjectsOfType<PlaneAction>();

		mScrollView = GUILayout.BeginScrollView(mScrollView);
		for(int i = 0; i < target.Length; i++)
		{
			GUI.color = Color.yellow;
			fold[i] = EditorGUILayout.Foldout(fold[i], target[i].gameObject.name);
			GUI.color = Color.white;
			if(fold[i])
				Draw(target[i] as PlaneAction);
		}
		GUILayout.EndScrollView ();
	}

	[MenuItem("Planes/Planes Editor")]
	public static void ShowPlaneEditor()
	{
		EditorWindow.GetWindow<PlaneEditorWindow>("Planes Editor", true).Show();
	}

	private void Draw(PlaneAction target)
	{
		target.plane = EditorGUILayout.ObjectField ("Plane", target.plane, typeof(PlaneStatAdjustment)) as PlaneStatAdjustment;

		if(target.stat.Count < 4)
		{
			target.stat.Add(new PlaneAction.Stat());
			target.stat.Add(new PlaneAction.Stat());
			target.stat.Add(new PlaneAction.Stat());
			target.stat.Add(new PlaneAction.Stat());
		}

		PlaneAction.Stat stat;

		for(int j = 0; j < target.stat.Count; j++)
		{
			stat = target.stat[j];
			stat.type = (PlaneAction.Stat.Type)EditorGUILayout.EnumPopup(stat.type);
			EditorGUILayout.Separator ();
			stat.startLevel = EditorGUILayout.IntField ("Start Level", Mathf.Min(4, stat.startLevel));
			if(stat.type == PlaneAction.Stat.Type.Rockets)
				stat.playerLevel = EditorGUILayout.IntField ("Unlock Player Level", stat.playerLevel);
			stat.fold = EditorGUILayout.Foldout(stat.fold, "Levels");
			if(stat.fold)
			{
				for(int i = 0; i < stat.levels.Count; i++)
				{
					EditorGUILayout.LabelField("\tLevel" + (i + 1));
					stat.levels[i].value = EditorGUILayout.IntField ("\t\tValue", stat.levels[i].value);
					stat.levels[i].cost = EditorGUILayout.IntField ("\t\tCost", stat.levels[i].cost);
				}
				if(stat.levels.Count < 4)
				{
					if (GUILayout.Button("Add Level"))
					{
						stat.levels.Add(new PlaneAction.Stat.StatLevel());
					}
				}
			}
			EditorGUILayout.Separator ();
		}
	}
}
