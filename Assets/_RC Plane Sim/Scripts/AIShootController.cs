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
		foreach(WeaponLauncher wp in wpnlncr)
		{
			wp.Shoot();
		}
	}
}
