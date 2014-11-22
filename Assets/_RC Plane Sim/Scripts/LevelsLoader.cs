using UnityEngine;
using System.Collections;

public class LevelsLoader : MonoBehaviour 
{
	public GUIStyle front;
	public GUIStyle loader;

	private float angle;

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
			angle++;
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", front);
			Rect guiRect = new Rect((Screen.width - Screen.width * 0.11f) / 2, Screen.height - Screen.width * 0.115f, Screen.width * 0.11f, Screen.width * 0.1f);
			float xValue = ((guiRect.x + guiRect.width / 2.0f));
			float yValue = ((guiRect.y + guiRect.height * 0.65f));
			Vector2 Pivot = new Vector2(xValue, yValue);
			GUIUtility.RotateAroundPivot(angle, Pivot);
			GUI.Box(guiRect, "", loader);
			GUI.matrix = Matrix4x4.identity;
		}
	}
}
