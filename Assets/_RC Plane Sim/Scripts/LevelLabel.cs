using UnityEngine;
using System.Collections;

public class LevelLabel : MonoBehaviour 
{

	// Use this for initialization
	void OnEnable () 
	{
		GetComponent<UILabel> ().text = "LVL " + ProgressController.GetPlayerLevel ();
	}
}
