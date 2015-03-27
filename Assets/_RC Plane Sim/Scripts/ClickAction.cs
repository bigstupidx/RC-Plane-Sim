using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ChartboostSDK;
using Prime31;
using Prime31.WinPhoneStore;

public class ClickAction : MonoBehaviour 
{
	public enum ActionType
	{
		SinglePlayer,
		Tutorial,
		Settings,
		FacebookPage,
		MainMenu,
		Upgrades,
		CloseUpgrades,
		Shop,
		CloseShop,
		PopUpBuy,
		ClosePopUpBuy,
		PopUpSAS,
		ClosePopUpSAS,
		GamePlayLevel1,
        GamePlayLevel2,
		Pause,
		Resume,
		Retry,
		Exit,
		Eject,
		Map,
		PopUpLevel1,
        PopUpLevel2,
		ClosePopUpLevel,
		Fire1,
		Fire2,
        Boost,
		PopUpLevel3,
		GamePlayLevel3,
		PopUpSASYes,
		PopUpLevel4,
		GamePlayLevel4,
		PopUpLevel5,
		GamePlayLevel5,
		Gold1,
		Gold2,
		Gold3,
		Gold4,
		NextWave,
		Music,
		Sound,
		PopUpExit,
		ClosePopUpExit,
		ClosePopUpUnlock,
		ClosePopUpBuyPlane,
		GoldFree,
		Restore,
		Info,
		CloseInfo
	}
	//button action type
	public ActionType action;
	private bool isGun;
	//show ui screen helper
	private PanelType panel;
	//weapon button press time
	private float fireTime = 0;
	//weapon refresh time
	private float heatTime = 0;
	//player levels to unlock map levels
	private int[] unlockMap = new int[3]{2, 4, 6};

	void Update()
	{
		//ejecting parashute according device acceleration input
		if(action == ActionType.Eject && FlightView.eject && Input.acceleration.magnitude > 2f)
		{
			GameObject _parashute = GameObject.Instantiate(parashute) as GameObject;

			_parashute.transform.GetChild(0).gameObject.SetActive(true);
			_parashute.transform.GetChild(1).gameObject.SetActive(true);
			_parashute.transform.position = FlightView.Target.transform.position;

			FlightView.Target.GetComponent<PlayerManager>().plane.transform.SetParent(_parashute.transform.FindChild("Crate"));
			FlightView.Target.GetComponent<PlayerManager>().plane.transform.localPosition = Vector3.zero;
			_parashute.transform.FindChild("Crate").GetComponent<MeshRenderer>().enabled = false;
			
			Destroy(FlightView.Target);

			FlightView.Target = _parashute.transform.GetChild(0).gameObject;

			ProgressController.ejectAdd += 10;

            GameObject.Find("Button - Eject").GetComponentInChildren<UISprite>().enabled = false;
            GameObject.Find("Button - Eject").GetComponentInChildren<UILabel>().enabled = false;
            GameObject.Find("Button - Eject").GetComponentInChildren<TweenRotation>().enabled = false;

			FlightView.eject = false;
			Screen.orientation = ScreenOrientation.AutoRotation;
		}

		if(FlightView.Target == null || FlightView.Target.CompareTag("Parashute"))
		{
			return;
		}

		PlaneStatAdjustment stat;

		switch(action)
		{
			//fire minigun weapon button pressed
		case ActionType.Fire1:
			stat = FindObjectOfType<PlaneStatAdjustment>();
			if(isGun)
			{ 
				//fire minigun until reach heatTime or reload instead
				if(fireTime < Time.timeSinceLevelLoad)
				{
					heatTime = Time.timeSinceLevelLoad + stat.minigun.AmmoMax;
				}
				if(heatTime > Time.timeSinceLevelLoad)
				{
					GetComponent<UISprite>().fillAmount = 1 - (heatTime - Time.timeSinceLevelLoad) / stat.minigun.AmmoMax;
				}
				else
				{
					FlightView camera = FindObjectOfType<FlightView>();camera = FindObjectOfType<FlightView>();
					FlightView.Target.GetComponent<FlightSystem>().WeaponControl.LaunchWeapon(0);
				}
			}

			GetComponent<UISprite>().fillAmount = 1 - (heatTime - Time.timeSinceLevelLoad) / stat.minigun.AmmoMax;
			break;
			//rocket button reload animation
		case ActionType.Fire2:
			stat = FindObjectOfType<PlaneStatAdjustment>();
			GetComponent<UISprite>().fillAmount = 1 - (heatTime - Time.timeSinceLevelLoad) / stat.rocket.ReloadTime;
			break;
			//boost button reload animation
		case ActionType.Boost:
			GetComponent<UISprite>().fillAmount = 1 - (heatTime - Time.timeSinceLevelLoad) / 6f;
			if(GetComponent<UISprite>().fillAmount >= 0.7f)
			{
				GetComponent<AudioSource>().Stop();
			}
			break;
		}
	}

	void OnEnable()
	{
		//action needed on button appearance
		switch(action)
		{
		case ActionType.GoldFree:
			//check if rewarded video avaliable
            #if UNITY_IPHONE || UNITY_ANDROID
			Chartboost.cacheRewardedVideo(CBLocation.Default);
            #else
            gameObject.SetActive(false);
			#endif
			
			break;
		case ActionType.Eject:
			//check if eject indicator needed
			if(!FlightView.eject)
			{
				GetComponentInChildren<UISprite>().enabled = false;
				GetComponentInChildren<UILabel>().enabled = false;
			}
			break;
		case ActionType.Music:
			//adjust pause menu music button
			if(VolumeController.instance.music == 0)
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection_grey";
			}
			else
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection";
			}
			break;
		case ActionType.Sound:
			//adjust pause menu sound button
			if(VolumeController.instance.sound == 0)
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection_grey";
			}
			else
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection";
			}
			break;
		case ActionType.Fire1:
			//minigun button start settings
			isGun = false;
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Fire2:
			//check if rocket button avaliable
			if(PlaneAction.currentPlane == null)
				return;
			if(PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets).playerLevel == 1)
			{
				transform.localScale = Vector3.zero;
			}
			else
			{
				transform.localScale = Vector3.one;
			}
			//check rocket or laser button type
			if(PlaneAction.currentPlane.name.Contains("Dragon"))
			{
				GetComponent<UISprite>().spriteName = "laser_fire";
				transform.GetChild(0).GetComponent<UISprite>().spriteName = "laser_realoading";
			}
			else
			{
				GetComponent<UISprite>().spriteName = "rocket_fire";
				transform.GetChild(0).GetComponent<UISprite>().spriteName = "rocket_fire_stopped";
			}
			//rocket button start settings
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Boost:
			//boost button start settings
			heatTime = 0;
			fireTime = 0;
			GetComponent<AudioSource>().Stop();
			break;
		case ActionType.PopUpLevel2:
			//start state of map level button (if avaliable, lock text)
			MapLevelAdjuster(ProgressController.GetPlayerLevel() >= unlockMap[0], "unlock on lvl " + unlockMap[0]);
			break;
		case ActionType.PopUpLevel3:
			//start state of map level button (if avaliable, lock text)
			MapLevelAdjuster(ProgressController.GetPlayerLevel() >= unlockMap[1], "unlock on lvl " + unlockMap[1]);
			break;
		case ActionType.PopUpLevel4:
			//start state of map level button (if avaliable, lock text)
			MapLevelAdjuster(ProgressController.GetPlayerLevel() >= unlockMap[2], "unlock on lvl " + unlockMap[2]);
			break;
		case ActionType.PopUpLevel5:
			//start state of map level button (if avaliable, lock text)
			MapLevelAdjuster(ProgressController.isSas, "join the sas to unlock");
			break;
		case ActionType.PopUpSAS:
			//check if join sas button needed
			if(ProgressController.isSas)
			{
				if(Application.loadedLevel > 2 && UIController.current.panelType == PanelType.Type.WinSAS)
				{
					GameObject.Find("SAS").SetActive(false);
					var level = GameObject.Find("Level").transform;
					level.localPosition = new Vector3(level.localPosition.x, -925, level.localPosition.y); 
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			break;
		case ActionType.NextWave:
			//overview screen settings on free for all completition
			if(TypeAction.type == TypeAction.FREE_FOR_ALL)
			{
				//setup leaderboard
				int[] points = new int[5];

				for(int i = 0; i < points.Length - 1; i++)
				{
					points[i] = UnityEngine.Random.Range(0, 25);
				}

				points[4] = ProgressController.killAdd;

				Array.Sort(points);

				List<string> names = new List<string>();
				names.Add("Warthog");
				names.Add("Golden Eagle");
				names.Add("War Angel");
				names.Add("Predator");
				names.Add("Superbolt");

				int j = 4;
				bool isPlayer = false;
                var ldr = GameObject.Find("LeaderBoard").GetComponentsInChildren<UILabel>();
                for (int i = 0; i < ldr.Length / 2; i++)
                {
                    if (points[j] != ProgressController.killAdd || isPlayer)
                    {
                        int index = UnityEngine.Random.Range(0, names.Count);
                        ldr[i].text = names[index] + ":";
						ldr[i].color = Color.white;
                        ldr[i + 5].text = points[j] + " Kills";
						ldr[i + 5].color = Color.white;
                        names.RemoveAt(index);
                    }
                    else
                    {
                        isPlayer = true;
                        ldr[i].text = "Player:";
						ldr[i].color = Color.yellow;
                        ldr[i + 5].text = points[j] + " Kills";
						ldr[i + 5].color = Color.yellow;
                    }
                    j--;
                }
				//setup other buttons and labels
				GameObject.Find("LeaderBoard").transform.localScale = Vector3.one;
				GameObject.Find("KillsLabelText").GetComponent<UILabel>().text = "";
				GameObject.Find("killAddLabel").GetComponent<UILabel>().text = "";

				GameObject.Find("Button - Next").GetComponent<UISprite>().enabled = false;
				GameObject.Find("Button - Ok").GetComponent<UISprite>().enabled = true;
				GameObject.Find("Button - Retry").GetComponent<UISprite>().enabled = true;
				
				GameObject.Find("Button - Next").GetComponentInChildren<UILabel>().text = "";
				GameObject.Find("Button - Ok").GetComponentInChildren<UILabel>().text = "OK";
				GameObject.Find("Button - Retry").GetComponentInChildren<UILabel>().text = "RETRY";
			}
			//overview screen settings on wave clear
			else if(TypeAction.type == TypeAction.SURVIVAL)
			{
				GameObject.Find("KillsLabelText").GetComponent<UILabel>().text = "WAVES CLEARED:";
				GameObject.Find("killAddLabel").GetComponent<UILabel>().text = ProgressController.killAdd.ToString ();
				GameObject.Find("LeaderBoard").transform.localScale = Vector3.zero;
				if(FlightView.Target == null || FlightView.Target.CompareTag("Parashute"))
				{
					GameObject.Find("Button - Next").GetComponent<UISprite>().enabled = false;
					GameObject.Find("Button - Ok").GetComponent<UISprite>().enabled = true;
					GameObject.Find("Button - Retry").GetComponent<UISprite>().enabled = true;

					GameObject.Find("Button - Next").GetComponentInChildren<UILabel>().text = "";
					GameObject.Find("Button - Ok").GetComponentInChildren<UILabel>().text = "OK";
					GameObject.Find("Button - Retry").GetComponentInChildren<UILabel>().text = "RETRY";
				}
				else
				{
					GameObject.Find("Button - Next").GetComponent<UISprite>().enabled = true;
					GameObject.Find("Button - Ok").GetComponent<UISprite>().enabled = false;
					GameObject.Find("Button - Retry").GetComponent<UISprite>().enabled = false;

					GameObject.Find("Button - Next").GetComponentInChildren<UILabel>().text = "NEXT";
					GameObject.Find("Button - Ok").GetComponentInChildren<UILabel>().text = "";
					GameObject.Find("Button - Retry").GetComponentInChildren<UILabel>().text = "";
				}
			}
			break;
		}
	}

	public void Press()
	{
		FlightView camera;
		PlaneStatAdjustment stat;
		switch(action)
		{
		//minigun button pressed
		case ActionType.Fire1:
			stat = FindObjectOfType<PlaneStatAdjustment>();

			if(heatTime > Time.timeSinceLevelLoad)
				return;

			if(fireTime == 0)
			{
				fireTime = Time.timeSinceLevelLoad + stat.minigun.ReloadTime;
			}

			isGun = true;
			break;
			//rocket button pressed
		case ActionType.Fire2:
			if(heatTime < Time.timeSinceLevelLoad)
			{
				stat = FindObjectOfType<PlaneStatAdjustment>();
				heatTime = Time.timeSinceLevelLoad + stat.rocket.ReloadTime;
			}
			camera = FindObjectOfType<FlightView>();
			FlightView.Target.GetComponent<FlightSystem>().WeaponControl.LaunchWeapon(1);
			break;
			//boost button pressed
        case ActionType.Boost:
			if(heatTime <Time.timeSinceLevelLoad)
			{
	            camera = FindObjectOfType<FlightView>();
				FlightView.Target.GetComponent<FlightSystem>().Boost();
				heatTime = Time.timeSinceLevelLoad + 6f;
				GetComponent<AudioSource>().volume = VolumeController.instance.sound;
				GetComponent<AudioSource>().Play();
			}
            break;
		}
	}

	public void MapLevelAdjuster(bool avaliable, string lockText)
	{
		//enable level if avaliable
		if(avaliable)
		{
			transform.GetComponentInChildren<UILabel>().enabled = false;
			if(GetComponent<TweenColor>() != null)
			{
				GetComponent<TweenColor>().PlayReverse();
			}
			GetComponent<Collider>().enabled = true;
		}
		//lock level and show lock text
		else
		{
			transform.GetComponentInChildren<UILabel>().text = lockText;
			GetComponent<Collider>().enabled = false;
		}
	}

	public void Reveal()
	{
		//minigun, rocket or boost button up
		fireTime = 0;
		isGun = false;
	}

	private void CloseLevels()
	{
		//close map level windows
		panel = UIController.GetPanel(PanelType.Type.PopUpLevel1);
		panel.gameObject.SetActive(false);
		panel = UIController.GetPanel(PanelType.Type.PopUpLevel2);
		panel.gameObject.SetActive(false);
		panel = UIController.GetPanel(PanelType.Type.PopUpLevel3);
		panel.gameObject.SetActive(false);
		panel = UIController.GetPanel(PanelType.Type.PopUpLevel4);
		panel.gameObject.SetActive(false);
		panel = UIController.GetPanel(PanelType.Type.PopUpLevel5);
		panel.gameObject.SetActive(false);
	}

	void OnClick()
	{
		//play click sound
		if(UIController.current.panelType != PanelType.Type.GameMenu)
		{
			VolumeController.click.Play ();
		}

		switch(action)
		{
			//close Panel - Info
		case ActionType.CloseInfo:
			panel = UIController.GetPanel(PanelType.Type.Info);
			panel.gameObject.SetActive(false);
			break;
			//open Panel - Info
		case ActionType.Info:
			panel = UIController.GetPanel(PanelType.Type.Info);
			panel.gameObject.SetActive(true);
			break;
			//restore previous purchases
		case ActionType.Restore:
			AppleIAP.Instance().RestoreCompletedTransactions();
			break;
			//toggle music
		case ActionType.Music:
			if(VolumeController.instance.music == 0)
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection";
				VolumeController.instance.ChangeMusic(1);
			}
			else
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection_grey";
				VolumeController.instance.ChangeMusic(0);
			}
			break;
			//toggle sound
		case ActionType.Sound:
			if(VolumeController.instance.sound == 0)
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection";
				VolumeController.instance.ChangeSound(1);
			}
			else
			{
				GetComponent<UISprite>().spriteName = "Button_BG_reflection_grey";
				VolumeController.instance.ChangeSound(0);
			}
			break;
			//fire purchase event based on platform
		case ActionType.Gold1:
			#if UNITY_ANDROID
			GoogleIAB.Instance().Purchase("com.lunagames.RCW3");
			#elif UNITY_IPHONE
			AppleIAP.Instance().Purchase("com.lunagames.RCW3");
            #else
            Store.requestProductPurchase("com.lunagames.RCW3", (receipt, error) =>
            {
                if (receipt != null)
                {
                    Debug.Log("purchase completed with receipt: " + receipt);
                    Store.reportProductFulfillment("com.lunagames.RCW3");
                    ProgressController.gold += 4000;
                    ProgressController.SaveProgress();
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif
			break;
			//fire purchase event based on platform
		case ActionType.Gold2:
			#if UNITY_ANDROID
			GoogleIAB.Instance().Purchase("com.lunagames.RCW4");
			#elif UNITY_IPHONE
			AppleIAP.Instance().Purchase("com.lunagames.RCW4");
            #else
            Store.requestProductPurchase("com.lunagames.RCW4", (receipt, error) =>
            {
                if (receipt != null)
                {
                    Debug.Log("purchase completed with receipt: " + receipt);
                    Store.reportProductFulfillment("com.lunagames.RCW4");
                    ProgressController.gold += 10000;
                    ProgressController.SaveProgress();
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif
			break;
			//fire purchase event based on platform
		case ActionType.Gold3:
			#if UNITY_ANDROID
			GoogleIAB.Instance().Purchase("com.lunagames.RCW5");
			#elif UNITY_IPHONE
			AppleIAP.Instance().Purchase("com.lunagames.RCW5");
            #else
            Store.requestProductPurchase("com.lunagames.RCW5", (receipt, error) =>
            {
                if (receipt != null)
                {
                    Debug.Log("purchase completed with receipt: " + receipt);
                    Store.reportProductFulfillment("com.lunagames.RCW5");
                    ProgressController.gold += 30000;
                    ProgressController.SaveProgress();
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif
			break;
			//fire purchase event based on platform
		case ActionType.Gold4:
			#if UNITY_ANDROID
			GoogleIAB.Instance().Purchase("com.lunagames.RCW2");
			#elif UNITY_IPHONE
			AppleIAP.Instance().Purchase("com.lunagames.RCW2");
            #else
            Store.requestProductPurchase("com.lunagames.RCW2", (receipt, error) =>
            {
                if (receipt != null)
                {
                    Debug.Log("purchase completed with receipt: " + receipt);
                    ProgressController.adsNeeded = true;
                    ProgressController.SaveProgress();
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif
			break;
			//show rewarded video
		case ActionType.GoldFree:
			Chartboost.showRewardedVideo(CBLocation.Default);
			break;
			//load garage screen
		case ActionType.SinglePlayer:
			FlightView camera = FindObjectOfType<FlightView>();

			if(camera != null)
			{
				camera.enabled = false;
			}

			UIController.HidePanels();
			Time.timeScale = 1;
			if(Application.loadedLevel != 2)  LevelsLoader.LoadLevel(2);
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Garage);
			UIController.current.gameObject.SetActive(true);
			break;
			//open settings window
		case ActionType.Settings:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Settings);
			UIController.current.gameObject.SetActive(true);
			break;
			//open facebook page;
		case ActionType.FacebookPage:
			Application.OpenURL("https://www.facebook.com/lunagames");
			break;
			//return to main menu
		case ActionType.MainMenu:
			UIController.HidePanels();
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			if(Application.loadedLevel != 1) LevelsLoader.LoadLevel(1);
			break;
			//open map window
		case ActionType.Map:
			UIController.HidePanels();
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Map);
			UIController.current.gameObject.SetActive(true);
			break;
			//open level 1 window
		case ActionType.PopUpLevel1:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel1);
			panel.gameObject.SetActive(true);
			break;
			//open level 2 window
        case ActionType.PopUpLevel2:
			CloseLevels();
            panel = UIController.GetPanel(PanelType.Type.PopUpLevel2);
            panel.gameObject.SetActive(true);
            break;
			//open level 3 window
		case ActionType.PopUpLevel3:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel3);
			panel.gameObject.SetActive(true);
			break;
			//open level 4 window
		case ActionType.PopUpLevel4:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel4);
			panel.gameObject.SetActive(true);
			break;
			//open level 5 window
		case ActionType.PopUpLevel5:
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel5);
			panel.gameObject.SetActive(true);
			break;
			//close all map level windows
		case ActionType.ClosePopUpLevel:
			CloseLevels();
			break;
			//open plane upgrade popup window
		case ActionType.Upgrades:
			panel = UIController.GetPanel(PanelType.Type.Upgrade);
			panel.gameObject.SetActive(true);
			break;
			//close plane upgrade popup window
		case ActionType.CloseUpgrades:
			panel = UIController.GetPanel(PanelType.Type.Upgrade);
			panel.gameObject.SetActive(false);
			break;
			//open shop window
		case ActionType.Shop:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(false);
			panel = UIController.GetPanel(PanelType.Type.Shop);
			panel.gameObject.SetActive(true);
			break;
			//close shop window
		case ActionType.CloseShop:
			panel = UIController.GetPanel(PanelType.Type.Shop);
			panel.gameObject.SetActive(false);
			break;
			//open not enough coins popup
		case ActionType.PopUpBuy:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(true);
			break;
			//close not enough coins popup
		case ActionType.ClosePopUpBuy:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(false);
			break;
			//open buy sas popup
		case ActionType.PopUpSAS:
			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(true);
			break;
			//close buy sas popup
		case ActionType.ClosePopUpSAS:
			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(false);
			break;
			//open map level exit confirmation from pause window
		case ActionType.PopUpExit:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Exit);
			UIController.current.gameObject.SetActive(true);
			break;
			//close map level exit confirmation from pause window
		case ActionType.ClosePopUpExit:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
			break;
			//close popup about unlocked content by player level
		case ActionType.ClosePopUpUnlock:
			panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(false);
			break;
			//close buy plane confirmation popup
		case ActionType.ClosePopUpBuyPlane:
			panel = UIController.GetPanel(PanelType.Type.BuyPlane);
			panel.gameObject.SetActive(false);
			break;
			//load map level 1
		case ActionType.GamePlayLevel1:
			if(Application.loadedLevel != 3)  LevelsLoader.LoadLevel(3);
			break;
			//load map level 2
        case ActionType.GamePlayLevel2:
            if (Application.loadedLevel != 4) LevelsLoader.LoadLevel(4);
            break;
			//load map level 3
		case ActionType.GamePlayLevel3:
			if (Application.loadedLevel != 5) LevelsLoader.LoadLevel(5);
			break;
			//load map level 4
		case ActionType.GamePlayLevel4:
			if (Application.loadedLevel != 6) LevelsLoader.LoadLevel(6);
			break;
			//load map level 5
		case ActionType.GamePlayLevel5:
			if (Application.loadedLevel != 7) LevelsLoader.LoadLevel(7);
			break;
			//fire sas buy event with platform dependencies
		case ActionType.PopUpSASYes:
			#if UNITY_ANDROID
			GoogleIAB.Instance().Purchase("com.lunagames.RCW1");
			#elif UNITY_IPHONE
			AppleIAP.Instance().Purchase("com.lunagames.RCW1");
            #else
            Store.requestProductPurchase("com.lunagames.RCW1", (receipt, error) =>
            {
                if (receipt != null)
                {
                    Debug.Log("purchase completed with receipt: " + receipt);
                    ProgressController.isSas = true;
                    var go = GameObject.Find("Dragonfly");
                    if (go != null)
                    {
                        go.GetComponent<PlaneAction>().UpdatePlane(2);
                        GameObject.Find("SAS Button").SetActive(false);
                    }

                    ProgressController.SaveProgress();

					if(UIController.current.panelType == PanelType.Type.WinSAS)
					{
						UIController.current.gameObject.SetActive(false);
						UIController.previous = UIController.current;
						UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
						UIController.current.gameObject.SetActive(true);
					}
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif

			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(false);
			break;
			//open pause window and pause game
		case ActionType.Pause:
			Time.timeScale = 0;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
			break;
			//resume game and open game ui
		case ActionType.Resume:
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			break;
			//reload current map level
		case ActionType.Retry:
            LevelsLoader.LoadLevel(Application.loadedLevel);
			break;
			//exit to main menu
		case ActionType.Exit:
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			LevelsLoader.LoadLevel(1);
			break;
			//start next wave
		case ActionType.NextWave:
			ProgressController.expAdd = 0;

			StartCoroutine(GameObject.FindObjectOfType<FlightView>().StartNextWave());

			ProgressController.goldAdd = 0;
			ProgressController.ejectAdd = 0;
			ProgressController.expAdd = 0;

			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			break;
		}
	}

	public GameObject parashute;
}
