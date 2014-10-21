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
        // Free for all
        var enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy.Length == 1)
            ProgressController.goldAdd += 50 * SwipeAction.levelDifficult;
        else if (enemy.Length == 2)
            ProgressController.goldAdd += 25 * SwipeAction.levelDifficult;
        else if (enemy.Length >= 3)
            ProgressController.goldAdd += 10 * SwipeAction.levelDifficult;

		GameManager gamemanager = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		gamemanager.GameOver ();
		base.OnDead (killer);

		if(eject != null) eject.SetActive (true);
		Screen.lockCursor = false;
	}
}
