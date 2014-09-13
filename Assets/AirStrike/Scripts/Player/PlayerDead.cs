using UnityEngine;
using System.Collections;

public class PlayerDead : FlightOnDead
{
	private GameObject eject;
	void Awake ()
	{
		eject =	GameObject.FindGameObjectWithTag ("Eject");
		if(eject != null) eject.SetActive (false);
	}
	
	// if player dead 
	public override void OnDead (GameObject killer)
	{
		GameManager gamemanger = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		gamemanger.GameOver ();
		base.OnDead (killer);

		if(eject != null) eject.SetActive (true);
		Screen.lockCursor = false;
	}
}
