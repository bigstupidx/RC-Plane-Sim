using UnityEngine;
using System.Collections;

public class MoverMissile : WeaponBase
{
	public float Damping = 3;
	public float Speed = 80;
	public float SpeedMax = 80;
	public float SpeedMult = 1;
	public Vector3 Noise = new Vector3 (20, 20, 20);
	public float TargetLockDirection = 0.5f;
	public int DistanceLock = 70;
	public int DurationLock = 40;
	public bool Seeker;
	public float LifeTime = 5.0f;
	private bool locked;
	private int timetorock;
	private float timeCount = 0;

	private void Start ()
	{
		timeCount = Time.time;
		Destroy (gameObject, LifeTime);
	}
	
	private void FixedUpdate ()
	{
		GetComponent<Rigidbody>().velocity = new Vector3 (transform.forward.x * Speed * Time.fixedDeltaTime, transform.forward.y * Speed * Time.fixedDeltaTime, transform.forward.z * Speed * Time.fixedDeltaTime);
		GetComponent<Rigidbody>().velocity += new Vector3 (Random.Range (-Noise.x, Noise.x), Random.Range (-Noise.y, Noise.y), Random.Range (-Noise.z, Noise.z));
		
		if (Speed < SpeedMax) 
		{
			Speed += SpeedMult * Time.fixedDeltaTime;
		}
	}

	private void Update ()
	{
		if (Time.time >= (timeCount + LifeTime) - 0.5f) 
		{
			if (GetComponent<Damage> ()) 
			{
				GetComponent<Damage> ().Active ();
			}
		}

		if (Target) 
		{
			Quaternion rotation = Quaternion.LookRotation (Target.transform.position - transform.transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * Damping);
		}
		
		if (Seeker) 
		{
			for (int t = 0; t < TargetTag.Length; t++) 
			{
				if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) 
				{
					GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
					for (int i = 0; i < objs.Length; i++) 
					{
						if (objs [i]) 
						{
							Vector3 dir = (objs [i].transform.position - transform.position).normalized;
							float direction = Vector3.Dot (dir, transform.forward);
							
							if (direction >= 0.7f) 
							{
								float dis = Vector3.Distance (objs [i].transform.position, transform.position);
								
								if (DistanceLock > dis) 
								{
									// Draw the indicator
									Target = objs [i];
								}
							}
						}
					}
				}
			}

			if (Target) 
			{
				
			} 
			else 
			{
				locked = false;

			}
		}
	}

}
