using UnityEngine;
using System.Collections;

public class PlayerDead : FlightOnDead
{
	// if player dead 
	public override void OnDead (GameObject killer)
	{
		GameManager gamemanager = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		gamemanager.GameOver ();
		base.OnDead (killer);

		Screen.lockCursor = false;
	}
}
