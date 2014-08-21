using UnityEngine;
using System.Collections;

public class PlaneAction : MonoBehaviour 
{
	public PlaneStats planeStats;

	public static PlaneStats currentStats;

	void Awake()
	{
		if(name.Contains("1"))
		{
			currentStats = planeStats;
		}
	}

	public void Click()
	{
		currentStats = planeStats;
	}
}
