using UnityEngine;
using System.Collections;

public class EnemyDead : FlightOnDead {
	// giving score.
	public int ExpAdd = 250;
	
	void Start () {}
	
	// if Enemy on Dead
	public override void OnDead (GameObject killer)
	{
		if(killer){// check if killer is exist
			// check if PlayerManager are included.
			if(killer.gameObject.GetComponent<PlayerManager>()){
				// find gameMAnager and Add score
				GameManager score = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
				score.AddExp(ExpAdd);
			}
		}
		base.OnDead (killer);
	}
}
