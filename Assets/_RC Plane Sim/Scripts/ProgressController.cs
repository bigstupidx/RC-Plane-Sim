using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_IPHONE || UNITY_ANDROID
using System.Runtime.Serialization.Formatters.Binary;
#else
using Newtonsoft.Json;
#endif

public class ProgressController : MonoBehaviour 
{
	//current gold
    public static int gold;
	//current experience
    public static int exp;
	//map level earned gold
    public static int goldAdd;
	//map level parachute eject gold bonus earned
	public static int ejectAdd;
	//map level earned experience
    public static int expAdd;
	//player kill points
	public static int killAdd;
	//player sas status
	public static bool isSas;
	//sas gold and exp bonus
	public static int sasBonus = 2;
	//plane availableness status
	public static bool[] planeLocked;
	//current plane stats (speed, damage health)
	private static PlaneAction.Stat[,] stats;
	//kills count on every level and difficulty
	public static int[,] scoreFree = new int[5, 5];
	//death count on every level and difficulty
	public static int[,] scoreDeath = new int[5, 5];
	//max wave progression on each level
	public static int[] scoreWave = new int[5];
	//current map level
	public static int level;
	//if player buy full version
    public static bool adsNeeded;
	//current texture on planes
	private static int[] materials = new int[6];

	void Awake () 
    {
		#if UNITY_IPHONE   
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif

        LoadProgress();
	}

	public static int GetPlayerLevel()
	{
		//get player level according current exp
		int i = 0;
		int index = 0;
		for(; i < UILevelController.exps.Length; i++)
		{
			if(exp + expAdd  > UILevelController.exps[i])
			{
				index = i;
			}
		}

		return index + 1;
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
        PlayerPrefs.SetInt("Ads", Convert.ToInt16(adsNeeded));

		if(Application.loadedLevel == 2)
		{
			var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
			planeLocked = new bool[planesArray.Length];
			stats = new PlaneAction.Stat[planesArray.Length, 8];
			for(int i = 0; i < planesArray.Length; i++)
			{
				planeLocked[i] = planesArray[i].isOwned;
				materials[i] = planesArray[i].material;
				for(int j = 0; j < planesArray[i].stat.Length; j++)
				{
					stats[i, j] = planesArray[i].stat[j];
				}
			}

			#if UNITY_IPHONE || UNITY_ANDROID
			PlayerPrefs.SetString("Materials", Serialize(materials));
			PlayerPrefs.SetString("Planes", Serialize(stats));
			PlayerPrefs.SetString("Locked", Serialize(planeLocked));
			#else
			PlayerPrefs.SetString("Materials", JsonConvert.SerializeObject(materials));
			PlayerPrefs.SetString("Planes", JsonConvert.SerializeObject(stats));
			PlayerPrefs.SetString("Locked", JsonConvert.SerializeObject(planeLocked));
			#endif


		}
		#if UNITY_IPHONE || UNITY_ANDROID
		PlayerPrefs.SetString ("ScoreFree", Serialize(scoreFree));
		PlayerPrefs.SetString ("ScoreWave", Serialize(scoreWave));
		PlayerPrefs.SetString ("ScoreDeath", Serialize(scoreDeath));
		#else
		PlayerPrefs.SetString ("ScoreFree", JsonConvert.SerializeObject (scoreFree));
		PlayerPrefs.SetString ("ScoreWave", JsonConvert.SerializeObject (scoreWave));
		PlayerPrefs.SetString ("ScoreDeath", JsonConvert.SerializeObject (scoreDeath));
		#endif
    }

	#if UNITY_IPHONE || UNITY_ANDROID
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
	#endif

	public static void LoadProgress()
    {
        gold = PlayerPrefs.GetInt("Gold");
		exp = PlayerPrefs.GetInt("Exp");
		isSas = Convert.ToBoolean(PlayerPrefs.GetInt ("SAS"));
        adsNeeded = Convert.ToBoolean(PlayerPrefs.GetInt("Ads"));
		#if UNITY_IPHONE || UNITY_ANDROID
		stats = Deserialize(PlayerPrefs.GetString ("Planes")) as PlaneAction.Stat[,];
		materials = Deserialize(PlayerPrefs.GetString ("Materials")) as int[];
		planeLocked = Deserialize(PlayerPrefs.GetString ("Locked")) as bool[];
		
		var _free = Deserialize(PlayerPrefs.GetString ("ScoreFree")) as int[,];
		var _death = Deserialize(PlayerPrefs.GetString ("ScoreDeath")) as int[,];
		var _wave = Deserialize(PlayerPrefs.GetString ("ScoreWave")) as int[];
		#else
		stats = JsonConvert.DeserializeObject<PlaneAction.Stat[,]>(PlayerPrefs.GetString ("Planes"));
		materials = JsonConvert.DeserializeObject<int[]>(PlayerPrefs.GetString ("Materials"));
		planeLocked = JsonConvert.DeserializeObject<bool[]>(PlayerPrefs.GetString ("Locked"));
		
		var _free = JsonConvert.DeserializeObject<int[,]> (PlayerPrefs.GetString ("ScoreFree"));
		var _death = JsonConvert.DeserializeObject<int[,]> (PlayerPrefs.GetString ("ScoreDeath"));
		var _wave = JsonConvert.DeserializeObject<int[]> (PlayerPrefs.GetString ("ScoreWave"));
		#endif

		if(_free != null)
		{
			scoreFree = _free;
		}

		if(_wave != null)
		{
			scoreWave = _wave;
		}

		if(_death != null)
		{
			scoreDeath = _death;
		}

		if(materials == null)
		{
			materials = new int[6];
		}

		if(stats != null)
		{
			var planesArray = GameObject.FindObjectsOfType<PlaneAction> ();
			for(int i = 0; i < planesArray.Length; i++)
			{
				for(int j = 0; j < 8; j++)
				{
					planesArray[i].stat[j] = stats[i, j];
				}

				planesArray[i].material = materials[i];
				planesArray[i].isOwned = planeLocked[i];
			}
		}
    }
}
