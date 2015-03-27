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
			GunHeatTime,
			Speed
		}
		//type of stat
		public Type type;
		//player level needed to unlock stat
		public int playerLevel;
		//start level of the stat
		public int startLevel;
		//current level of the stat
		public int currentLevel;

		public bool fold;
		//cost and value on each stat level
		public StatLevel[] levels;
	}
	//player level needed to unlock plane
	public int unlockLevel;
	//plane unlock price
	public int unlockPrice;
	//is player owned current plane
	public bool isOwned;
	//stats on the plane model
	public PlaneStatAdjustment plane;
	//index of the current plane texture
	public int material;
	//all of the plane stats
	public Stat[] stat;
	//current selected plane
	public static PlaneAction currentPlane;
	//current plane model
	public static GameObject planeModel;
	//current plane icon
	private static UISprite sprite;
	//current plane icon name
	private static string spriteName;

	void Awake()
	{
		//select first plane on garage level selected
		if(name.Contains("Superbolt") && currentPlane == null)
		{
			currentPlane = this;
			sprite = transform.GetChild(0).GetComponent<UISprite>();
			spriteName = sprite.spriteName;
			sprite.spriteName = spriteName + "_highlighted";
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
			//update plane icon at the bottom garage ui and place selected plane in garage
			if(name.Contains("Superbolt"))
			{
				if(currentPlane == null)
				{
					currentPlane = this;
				}
				
				planeModel = Instantiate (currentPlane.plane.gameObject, new Vector3 (-42f, 10f, -20f), Quaternion.identity) as GameObject;
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
                transform.FindChild("Label").GetComponent<UILabel>().text = unlockPrice + "\ncoins";
				transform.FindChild("Sprite").GetComponent<UISprite>().color = Color.white;
				GetComponent<UIButton>().enabled = true;
			}
		}
	}

	public void Click()
	{
		//select new plane at the bottom garage scroll menu
		PanelType panel;
		if(!isOwned)
		{
			//show buy popup if plane locked but avaliable to purchasing
			BuyPlane.plane = this;
			UIController.GetPanel(PanelType.Type.BuyPlane).gameObject.SetActive(false);
			UIController.GetPanel(PanelType.Type.BuyPlane).gameObject.SetActive(true);
			return;
		}

		if (planeModel != null)
						Destroy (planeModel);

		sprite.spriteName = spriteName;

		currentPlane = this;
		sprite = transform.GetChild(0).GetComponent<UISprite>();
		spriteName = sprite.spriteName;
		sprite.spriteName = spriteName + "_highlighted";

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

		panel = UIController.GetPanel(PanelType.Type.Upgrade);
		panel.gameObject.SetActive(false);
	}

	public static Stat FindStatType(Stat.Type type)
	{
		//return stat by type
		if (currentPlane == null)
						return null;

		for(int i = 0; i < currentPlane.stat.Length; i++)
		{
			if(currentPlane.stat[i].type == type)
				return currentPlane.stat[i];
		}

		return null;
	}
}
