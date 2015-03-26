using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour 
{
	private UILabel label;

	// Use this for initialization
	void Start () 
	{
		label = GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(TypeAction.type == TypeAction.FREE_FOR_ALL)
		{
			label.text = "Kills: " + ProgressController.killAdd;
		}
		else
		{
			label.text = "Wave: " + ProgressController.killAdd;
		}
	}
}
