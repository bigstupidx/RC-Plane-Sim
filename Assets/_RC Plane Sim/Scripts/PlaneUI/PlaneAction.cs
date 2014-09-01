using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class PlaneAction : MonoBehaviour 
{
	[Serializable]
	public class Stat
	{
		[Serializable]
		public class StatLevel
		{
			public int value;
			public int cost;
		}
		
		public enum Type
		{
			Machinegun,
			Rockets,
			Armour,
			Range
		}
		
		public Type type;
		public int playerLevel;
		public int startLevel;
		public int currentLevel;
		public bool fold;
		public List<StatLevel> levels = new List<StatLevel>();
	}

	public PlaneStatAdjustment plane;
	public int material;
	public List<Stat> stat = new List<Stat>();

	public static PlaneStatAdjustment currentPlane;
	public static int currentMaterial;
	public static List<Stat> currentStat;
	public static GameObject planeModel;
	
	void Awake()
	{
		if(name.Contains("Bloody Jane"))
		{
			currentStat = stat;
			currentMaterial = material;
			currentPlane = plane;
		}
	}

	void OnLevelWasLoaded(int level) 
	{
		if(level == 2)
		{
			if(name.Contains("Bloody Jane"))
			{
				currentStat = stat;
				currentMaterial = material;
				currentPlane = plane;
			}

			if (planeModel != null || currentPlane == null)
				return;

			planeModel = Instantiate (currentPlane.gameObject, new Vector3 (1360.706f, 1.376582f, 1360.081f), Quaternion.identity) as GameObject;
			planeModel.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			planeModel.transform.localEulerAngles = new Vector3 (0, 144f, 0);
			
			MonoBehaviour[] comps = planeModel.GetComponents<MonoBehaviour>();
			
			foreach(MonoBehaviour c in comps)
			{
				c.enabled = false;
			}
			
			planeModel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	public void Click()
	{
		if (planeModel != null)
						Destroy (planeModel);

		currentStat = stat;
		currentMaterial = material;
		currentPlane = plane;

		planeModel = Instantiate (plane.gameObject, new Vector3 (1360.706f, 1.376582f, 1360.081f), Quaternion.identity) as GameObject;
		planeModel.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		planeModel.transform.localEulerAngles = new Vector3 (0, 144f, 0);

		MonoBehaviour[] comps = planeModel.GetComponents<MonoBehaviour>();
		
		foreach(MonoBehaviour c in comps)
		{
			c.enabled = false;
		}

		planeModel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}

	public static Stat FindStatType(Stat.Type type)
	{
		for(int i = 0; i < currentStat.Count; i++)
		{
			if(currentStat[i].type == type)
				return currentStat[i];
		}

		return null;
	}
}
