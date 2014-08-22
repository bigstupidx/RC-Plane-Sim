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
	
	public List<Stat> stat = new List<Stat>();

	public static List<Stat> currentStat;

	void Awake()
	{
		if(name.Contains("1"))
		{
			currentStat = stat;
		}
	}

	public void Click()
	{
		currentStat = stat;
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
