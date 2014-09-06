using UnityEngine;
using System.Collections;

public class LevelsLoader : MonoBehaviour 
{
	public GUIStyle front;

	public static bool isBlack;

	void OnLevelWasLoaded(int level) 
	{
		isBlack = false;
	}

	public static void LoadLevel(int level)
	{
		isBlack = true;
		Application.LoadLevelAsync(level);
	}

	void OnGUI()
	{
		if(isBlack) 
		{
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", front);
		}
	}
}
