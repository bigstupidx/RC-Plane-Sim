﻿using UnityEngine;
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
			public float value;
			public int cost;
		}
		
		public enum Type
		{
			Machinegun,
			Rockets,
			Armour,
			Range,
			RocketsReload,
			GunFireTime,
			GunHeatTime
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

				planeModel = Instantiate (plane.gameObject, new Vector3 (-42f, 10f, -20f), Quaternion.identity) as GameObject;
				planeModel.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
				planeModel.transform.localEulerAngles = new Vector3 (0, 144f, 0);
				
				MonoBehaviour[] comps = planeModel.GetComponents<MonoBehaviour>();
				
				foreach(MonoBehaviour c in comps)
				{
					c.enabled = false;
				}

				planeModel.GetComponent<PlaneStatAdjustment>().enabled = true;
				
				planeModel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			}
		}
	}

	public void Click()
	{
		if (planeModel != null)
						Destroy (planeModel);

		currentStat = stat;
		currentMaterial = material;
		currentPlane = plane;

		planeModel = Instantiate (plane.gameObject, new Vector3 (-42f, 10f, -20f), Quaternion.identity) as GameObject;
		planeModel.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
		planeModel.transform.localEulerAngles = new Vector3 (0, 144f, 0);

		MonoBehaviour[] comps = planeModel.GetComponents<MonoBehaviour>();
		
		foreach(MonoBehaviour c in comps)
		{
			c.enabled = false;
		}

		planeModel.GetComponent<PlaneStatAdjustment>().enabled = true;

		planeModel.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}

	public static Stat FindStatType(Stat.Type type)
	{
		if (currentStat == null)
						return null;

		for(int i = 0; i < currentStat.Count; i++)
		{
			if(currentStat[i].type == type)
				return currentStat[i];
		}

		return null;
	}
}
