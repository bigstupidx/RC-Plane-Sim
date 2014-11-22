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

	public int unlockLevel;
	public int unlockPrice;
	public bool isOwned;
	public PlaneStatAdjustment plane;
	public int material;
	public List<Stat> stat = new List<Stat>();

	public static PlaneStatAdjustment currentPlane;
	public static int currentMaterial;
	public static List<Stat> currentStat;
	public static GameObject planeModel;
	
	void Awake()
	{
		if(name.Contains("Superbolt"))
		{
			currentStat = stat;
			currentMaterial = material;
			currentPlane = plane;
		}

		if(!isOwned)
			isOwned = unlockPrice == 0;
	}

	void OnLevelWasLoaded(int level) 
	{
		UpdatePlane (level);
	}

	public void UpdatePlane(int level)
	{
		if(level == 2)
		{
			if(name.Contains("Superbolt"))
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
			
			if(unlockLevel > ProgressController.GetPlayerLevel())
			{
				transform.FindChild("Label").GetComponent<UILabel>().text = "unlock on level " + unlockLevel;
				transform.FindChild("Sprite").GetComponent<UISprite>().color = Color.black;
				GetComponent<UIButton>().enabled = false;
			}
			else if(unlockLevel == 0 && !ProgressController.isSas)
			{
				transform.FindChild("Label").GetComponent<UILabel>().text = "join the sas to unlock";
				transform.FindChild("Sprite").GetComponent<UISprite>().color = Color.black;
				GetComponent<UIButton>().enabled = false;
			}
			else if(isOwned)
			{
				transform.FindChild("Lock").GetComponent<UISprite>().enabled = false;
				transform.FindChild("Label").GetComponent<UILabel>().enabled = false;
				transform.FindChild("Sprite").GetComponent<UISprite>().color = Color.white;
				GetComponent<UIButton>().enabled = true;
			}
			else
			{
				transform.FindChild("Lock").GetComponent<UISprite>().enabled = false;
				transform.FindChild("Label").GetComponent<UILabel>().text = unlockPrice.ToString();
				transform.FindChild("Sprite").GetComponent<UISprite>().color = Color.white;
				GetComponent<UIButton>().enabled = true;
			}
		}
	}

	public void Click()
	{
		if(!isOwned)
		{
			isOwned = true;
			transform.FindChild("Label").GetComponent<UILabel>().enabled = false;
			ProgressController.SaveProgress();
			return;
		}

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

		PanelType panel = UIController.GetPanel(PanelType.Type.Upgrade);
		panel.gameObject.SetActive(false);
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
