using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class ProgressController : MonoBehaviour 
{
    public static int gold;
    public static int exp;
    public static int goldAdd;
    public static int expAdd;
	private static List<List<PlaneAction.Stat>> stats;

	void Awake () 
    {
        LoadProgress();
	}

    public static void SaveProgress() 
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Exp", exp);

		if(Application.loadedLevel == 2)
		{
			var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
			stats = new List<List<PlaneAction.Stat>>();
			for(int i = 0; i < planesArray.Length; i++)
			{
				stats.Add(planesArray[i].stat);
			}

			PlayerPrefs.SetString("Planes", Serialize(stats));
		}
    }

	public static void LoadProgress()
    {
        gold = PlayerPrefs.GetInt("Gold");
        exp = PlayerPrefs.GetInt("Exp");
		stats = Deserialize(PlayerPrefs.GetString ("Planes")) as List<List<PlaneAction.Stat>>;

		var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
		for(int i = 0; i < planesArray.Length; i++)
		{
			planesArray[i].stat = stats[i];
		}
    }

	private static string Serialize(object obj)
	{
		var binary = new BinaryFormatter();
		var memory = new MemoryStream();
		binary.Serialize(memory, obj);
		return Convert.ToBase64String(memory.GetBuffer());
	}
	
	private static object Deserialize(string data)
	{
		try
		{
			var binary = new BinaryFormatter();
			var memory = new MemoryStream(Convert.FromBase64String(data));
			return binary.Deserialize(memory);
		}
		catch(Exception)
		{
			
		}
		return null;
	}
}
