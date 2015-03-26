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
		
		public Type type;
		public int playerLevel;
		public int startLevel;
		public int currentLevel;
		public bool fold;
		public StatLevel[] levels;
	}

	public int unlockLevel;
	public int unlockPrice;
	public bool isOwned;
	public PlaneStatAdjustment plane;
	public int material;
	public Stat[] stat;
	
	public static PlaneAction currentPlane;
	public static GameObject planeModel;
	
	private static UISprite sprite;
	private static string spriteName;

	void Awake()
	{
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
		PanelType panel;
		if(!isOwned)
		{
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
