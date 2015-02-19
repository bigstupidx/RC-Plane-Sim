using UnityEngine;
using System.Collections;

public class AIShootController : MonoBehaviour 
{
	private WeaponLauncher[] wpnlncr;
	private FlightOnHit foh;

	public bool shoot;

	// Use this for initialization
	void Start () 
	{
		wpnlncr = GetComponentsInChildren<WeaponLauncher> ();
		foh = GetComponentInChildren<FlightOnHit> ();

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
	}

	// Update is called once per frame
	void Update () 
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");

		float distance;

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

	 	distance = Vector3.Distance(transform.position + (transform.forward * 250), obj.transform.position);

		foreach(WeaponLauncher wp in wpnlncr)
		{
			if(distance < 250)
			{
				wp.transform.LookAt(obj.transform.position);
				wp.Shoot(foh.Damage);
			}
			else
			{
				GameObject[] objs = GameObject.FindGameObjectsWithTag ("Enemy");
				
				foreach(GameObject o in objs)
				{
					distance = Vector3.Distance(transform.position + (transform.forward * 250), o.transform.position);

					if(distance < 500)
					{
						wp.transform.LookAt(o.transform.position);
						wp.Shoot(foh.Damage);
					}
				}
			}
		}
	}
}
