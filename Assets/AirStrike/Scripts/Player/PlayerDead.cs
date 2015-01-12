using UnityEngine;
using System.Collections;

public class PlayerDead : FlightOnDead
{
	// if player dead 
	public override void OnDead (GameObject killer)
	{
		if(TypeAction.type == TypeAction.FREE_FOR_ALL)
		{
			ProgressController.expAdd += 100 * SwipeAction.levelDifficult;
		}

		GameManager gamemanager = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		gamemanager.GameOver ();
		base.OnDead (killer);

		Screen.lockCursor = false;
	}
}
