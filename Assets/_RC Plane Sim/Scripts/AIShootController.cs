using UnityEngine;
using System.Collections;

public class AIShootController : MonoBehaviour 
{
	private WeaponLauncher[] wpnlncr;
	private FlightOnHit foh;

	public bool shoot;

	private float rocketDelay;

	// Use this for initialization
	void Start () 
	{
		wpnlncr = GetComponentsInChildren<WeaponLauncher> ();
		foh = GetComponentInChildren<FlightOnHit> ();

		//change ai minigun stat according to game difficulty
		foreach(WeaponLauncher wp in wpnlncr)
		{
			if(SwipeAction.levelDifficult == 1)
			{
				wp.Spread = 20;
				wp.FireRate = 0.1f;
			}
			else if(SwipeAction.levelDifficult == 2)
			{
				wp.Spread = 15;
				wp.FireRate = 0.07f;
			}
			else if(SwipeAction.levelDifficult == 3)
			{
				wp.Spread = 13;
				wp.FireRate = 0.05f;
			}
			else if(SwipeAction.levelDifficult == 4)
			{
				wp.Spread = 11;
				wp.FireRate = 0.03f;
			}
			else if(SwipeAction.levelDifficult == 5)
			{
				wp.Spread = 8;
				wp.FireRate = 0.02f;
			}
		}

        rocketDelay = Time.timeSinceLevelLoad + Random.Range(60f / (float)SwipeAction.levelDifficult, 80f / (float)SwipeAction.levelDifficult);
	}

	// Update is called once per frame
	void Update () 
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");

		float distance;

		//endless shooting if player was not present in the game (for main menu)
		if(obj == null)
		{
			if(shoot)
			{
				foreach(WeaponLauncher wp in wpnlncr)
				{
					wp.Shoot(foh.Damage);
				}
			}

			return;
		}
		//find distance to player
	 	distance = Vector3.Distance(transform.position + (transform.forward * 250), obj.transform.position);

		foreach(WeaponLauncher wp in wpnlncr)
		{
			if(distance < 250)
			{
				//shoot if player close enough
				if(wp.Missile.name.Contains("rocket"))
				{
					if(rocketDelay < Time.timeSinceLevelLoad && SwipeAction.levelDifficult != 1)
					{
						//shoot rocket
						rocketDelay = Time.timeSinceLevelLoad + Random.Range(60f / (float)SwipeAction.levelDifficult , 80f / (float)SwipeAction.levelDifficult);
						wp.transform.LookAt(obj.transform.position);
						wp.Shoot(foh.Damage);
					}
				}
				else
				{
					//shoot minigun
					wp.transform.LookAt(obj.transform.position);
					wp.Shoot(foh.Damage);
				}
			}
			else
			{
				//if player not reachable, then shoot other planes
				GameObject[] objs = GameObject.FindGameObjectsWithTag ("Enemy");
				
				foreach(GameObject o in objs)
				{
					distance = Vector3.Distance(transform.position + (transform.forward * 250), o.transform.position);

					if(distance < 500)
					{
						if(wp.Missile.name.Contains("rocket"))
						{

						}
						else
						{
							wp.transform.LookAt(obj.transform.position);
							wp.Shoot(foh.Damage);
						}
					}
				}
			}
		}
	}
}
