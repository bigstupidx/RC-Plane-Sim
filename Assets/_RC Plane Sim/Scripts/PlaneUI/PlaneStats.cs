using UnityEngine;
using System.Collections;
using System;

public class PlaneStats : MonoBehaviour 
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
			Range,
			Firepower,
			Accuracy,
			Armour,
			Speed
		}

		public Type type;
		public int startLevel;
		public int currentLevel;
		public StatLevel[] levels;
	}

	public Stat[] stats;
}
