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

	public ActionType action;
	private bool isGun;
	private PanelType panel;
	private float fireTime = 0;
	private float heatTime = 0;

	private int[] unlockMap = new int[3]{2, 4, 6};

	void Update()
	{
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

		if (Input.GetKeyDown (KeyCode.Escape) && Application.loadedLevel > 2) 
		{
			Screen.lockCursor = false;
			Time.timeScale = 0;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
		}
		PlaneStatAdjustment stat;

		if(FlightView.Target == null || FlightView.Target.CompareTag("Parashute"))
		{
			return;
		}

		switch(action)
		{
		case ActionType.Fire1:
			stat = FindObjectOfType<PlaneStatAdjustment>();
			if(isGun)
			{ 
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
		case ActionType.Fire2:
			stat = FindObjectOfType<PlaneStatAdjustment>();
			GetComponent<UISprite>().fillAmount = 1 - (heatTime - Time.timeSinceLevelLoad) / stat.rocket.ReloadTime;
			break;
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
		switch(action)
		{
		case ActionType.GoldFree:
			Chartboost.cacheRewardedVideo(CBLocation.Default);
			break;
		case ActionType.Eject:
			if(!FlightView.eject)
			{
				GetComponentInChildren<UISprite>().enabled = false;
				GetComponentInChildren<UILabel>().enabled = false;
			}
			break;
		case ActionType.Music:
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
			isGun = false;
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Fire2:
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

			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Boost:
			heatTime = 0;
			fireTime = 0;
			GetComponent<AudioSource>().Stop();
			break;
		case ActionType.PopUpLevel2:
			if(ProgressController.GetPlayerLevel() >= unlockMap[0])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				if(GetComponent<TweenColor>() != null)
				{
					GetComponent<TweenColor>().PlayReverse();
				}
				GetComponent<Collider>().enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[0];
				GetComponent<Collider>().enabled = false;
			}
			break;
		case ActionType.PopUpLevel3:
			if(ProgressController.GetPlayerLevel() >= unlockMap[1])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				if(GetComponent<TweenColor>() != null)
				{
					GetComponent<TweenColor>().PlayReverse();
				}
				GetComponent<Collider>().enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[1];
				GetComponent<Collider>().enabled = false;
			}
			break;
		case ActionType.PopUpLevel4:
			if(ProgressController.GetPlayerLevel() >= unlockMap[2])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				if(GetComponent<TweenColor>() != null)
				{
					GetComponent<TweenColor>().PlayReverse();
				}
				GetComponent<Collider>().enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[2];
				GetComponent<Collider>().enabled = false;
			}
			break;
		case ActionType.PopUpLevel5:
			if(ProgressController.isSas)
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				if(GetComponent<TweenColor>() != null)
				{
					GetComponent<TweenColor>().PlayReverse();
				}
				GetComponent<Collider>().enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "join the sas to unlock";
				GetComponent<Collider>().enabled = false;
			}
			break;
		case ActionType.PopUpSAS:
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
			if(TypeAction.type == TypeAction.FREE_FOR_ALL)
			{
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
		case ActionType.Fire2:
			if(heatTime < Time.timeSinceLevelLoad)
			{
				stat = FindObjectOfType<PlaneStatAdjustment>();
				heatTime = Time.timeSinceLevelLoad + stat.rocket.ReloadTime;
			}
			camera = FindObjectOfType<FlightView>();
			FlightView.Target.GetComponent<FlightSystem>().WeaponControl.LaunchWeapon(1);
			break;
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

	public void Reveal()
	{
		fireTime = 0;
		isGun = false;
	}

	private void CloseLevels()
	{
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
		FlightView camera;
		VolumeController cntrl;

		if(UIController.current.panelType != PanelType.Type.GameMenu)
		{
			VolumeController.click.Play ();
		}

		switch(action)
		{
		case ActionType.CloseInfo:
			panel = UIController.GetPanel(PanelType.Type.Info);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.Info:
			panel = UIController.GetPanel(PanelType.Type.Info);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.Restore:
			AppleIAP.Instance().RestoreCompletedTransactions();
			break;
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
		case ActionType.GoldFree:
			Chartboost.showRewardedVideo(CBLocation.Default);
			break;
		case ActionType.SinglePlayer:
			camera = FindObjectOfType<FlightView>();

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
		case ActionType.Tutorial:
			break;
		case ActionType.Settings:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Settings);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.FacebookPage:
			Application.OpenURL("https://www.facebook.com/lunagames");
			break;
		case ActionType.MainMenu:
			UIController.HidePanels();
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			if(Application.loadedLevel != 1) LevelsLoader.LoadLevel(1);
			break;
		case ActionType.Map:
			UIController.HidePanels();
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Map);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.PopUpLevel1:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel1);
			panel.gameObject.SetActive(true);
			break;
        case ActionType.PopUpLevel2:
			CloseLevels();
            panel = UIController.GetPanel(PanelType.Type.PopUpLevel2);
            panel.gameObject.SetActive(true);
            break;
		case ActionType.PopUpLevel3:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel3);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.PopUpLevel4:
			CloseLevels();
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel4);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.PopUpLevel5:
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel5);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpLevel:
			CloseLevels();
			break;
		case ActionType.Upgrades:
			panel = UIController.GetPanel(PanelType.Type.Upgrade);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.CloseUpgrades:
			panel = UIController.GetPanel(PanelType.Type.Upgrade);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.Shop:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(false);
			panel = UIController.GetPanel(PanelType.Type.Shop);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.CloseShop:
			panel = UIController.GetPanel(PanelType.Type.Shop);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.PopUpBuy:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpBuy:
			panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.PopUpSAS:
			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpSAS:
			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.PopUpExit:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Exit);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpExit:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpUnlock:
			panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.ClosePopUpBuyPlane:
			panel = UIController.GetPanel(PanelType.Type.BuyPlane);
			panel.gameObject.SetActive(false);
			break;
		case ActionType.GamePlayLevel1:
			if(Application.loadedLevel != 3)  LevelsLoader.LoadLevel(3);
			break;
        case ActionType.GamePlayLevel2:
            if (Application.loadedLevel != 4) LevelsLoader.LoadLevel(4);
            break;
		case ActionType.GamePlayLevel3:
			if (Application.loadedLevel != 5) LevelsLoader.LoadLevel(5);
			break;
		case ActionType.GamePlayLevel4:
			if (Application.loadedLevel != 6) LevelsLoader.LoadLevel(6);
			break;
		case ActionType.GamePlayLevel5:
			if (Application.loadedLevel != 7) LevelsLoader.LoadLevel(7);
			break;
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
                }
                else if (error != null)
                {
                    Debug.Log("error purchasing product: " + error);
                }
            });
			#endif

			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(false);

			if(UIController.current.panelType == PanelType.Type.WinSAS)
			{
				UIController.current.gameObject.SetActive(false);
				UIController.previous = UIController.current;
				UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
				UIController.current.gameObject.SetActive(true);
			}
			break;
		case ActionType.Pause:
			Time.timeScale = 0;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.Resume:
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.Retry:
            LevelsLoader.LoadLevel(Application.loadedLevel);
			break;
		case ActionType.Exit:
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			LevelsLoader.LoadLevel(1);
			break;
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
		case ActionType.Eject: 
			//if(FlightView.eject)
			//{
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
				
				FlightView.eject = false;
				Screen.orientation = ScreenOrientation.AutoRotation;
			//}
			break;
		}
	}

	public GameObject parashute;
}
