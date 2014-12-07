using UnityEngine;
using System.Collections;

public class AIShootController : MonoBehaviour 
{
	private WeaponLauncher[] wpnlncr;

	// Use this for initialization
	void Start () 
	{
		wpnlncr = GetComponentsInChildren<WeaponLauncher> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");

		if(obj == null)
		{
			return;
		}

		float distance = Vector3.Distance(transform.position + (transform.forward * 300), obj.transform.position);

		foreach(WeaponLauncher wp in wpnlncr)
		{
			if(distance < 300)
			{
				wp.transform.LookAt(obj.transform.position);
				wp.Shoot();
			}
		}
	}
}
