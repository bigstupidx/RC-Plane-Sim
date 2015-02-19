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
	public static int ejectAdd;
    public static int expAdd;
	public static int killAdd;
	public static bool isSas;
	public static int sasBonus = 2;
	public static bool[] planeLocked;
	private static List<List<PlaneAction.Stat>> stats;
	public static int[] scoreFree = new int[5];
	public static int[] scoreWave = new int[5];
	public static int level;

	void Awake () 
    {
        LoadProgress();
	}

	public static int GetPlayerLevel()
	{
		int i = 0;
		int index = 0;
		for(; i < UILevelController.exps.Length; i++)
		{
			if(exp + expAdd  > UILevelController.exps[i])
			{
				index = i;
			}
		}

		return 10;//index + 1;
	}

    public static void SaveProgress() 
    {
		GameObject goldLabel = GameObject.Find ("Gold Label");
		if(goldLabel != null)
		{
			goldLabel.GetComponent<UILabel>().text = gold.ToString();
		}

        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Exp", exp);
		PlayerPrefs.SetInt ("SAS", Convert.ToInt16 (isSas));

		if(Application.loadedLevel == 2)
		{
			var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
			planeLocked = new bool[planesArray.Length];
			stats = new List<List<PlaneAction.Stat>>();
			for(int i = 0; i < planesArray.Length; i++)
			{
				planeLocked[i] = planesArray[i].isOwned;
				stats.Add(planesArray[i].stat);
			}

			PlayerPrefs.SetString("Planes", Serialize(stats));
			PlayerPrefs.SetString("Locked", Serialize(planeLocked));
		}

		PlayerPrefs.SetString ("ScoreFree", Serialize (scoreFree));
		PlayerPrefs.SetString ("ScoreWave", Serialize (scoreWave));
    }

	public static void LoadProgress()
    {
        gold = PlayerPrefs.GetInt("Gold");
        exp = PlayerPrefs.GetInt("Exp");
		isSas = Convert.ToBoolean(PlayerPrefs.GetInt ("SAS"));
		stats = Deserialize(PlayerPrefs.GetString ("Planes")) as List<List<PlaneAction.Stat>>;
		planeLocked = Deserialize (PlayerPrefs.GetString ("Locked")) as bool[];

		var _free = Deserialize (PlayerPrefs.GetString ("ScoreFree")) as int[];
		var _wave = Deserialize (PlayerPrefs.GetString ("ScoreWave")) as int[];

		if(_free != null)
		{
			scoreFree = _free;
		}

		if(_wave != null)
		{
			scoreWave = _wave;
		}

		if(stats != null)
		{
			var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
			for(int i = 0; i < planesArray.Length; i++)
			{
				planesArray[i].stat = stats[i];
				planesArray[i].isOwned = planeLocked[i];
			}
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
