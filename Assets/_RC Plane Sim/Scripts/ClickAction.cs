using UnityEngine;
using System.Collections;

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
		NextWave
	}

	public ActionType action;
	private bool isGun;
	private PanelType panel;
	private float fireTime = 0;
	private float heatTime = 0;

	private int[] unlockMap = new int[3]{2, 4, 6};

	void Update()
	{
		if(FlightView.eject && Input.acceleration.magnitude > 2f)
		{
			GameObject _parashute = GameObject.Instantiate(parashute) as GameObject;

			_parashute.transform.GetChild(0).gameObject.SetActive(true);
			_parashute.transform.GetChild(1).gameObject.SetActive(true);
			_parashute.transform.position = camera.transform.position;

			FlightView.Target.GetComponent<PlayerManager>().plane.transform.SetParent(_parashute.transform.FindChild("Crate"));
			FlightView.Target.GetComponent<PlayerManager>().plane.transform.localPosition = Vector3.zero;
			_parashute.transform.FindChild("Crate").GetComponent<MeshRenderer>().enabled = false;
			
			Destroy(FlightView.Target);

			FlightView.Target = _parashute.transform.GetChild(0).gameObject;

			ProgressController.ejectAdd += 10;

			FlightView.eject = false;
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
			break;
		}
	}

	void OnEnable()
	{
		switch(action)
		{
		case ActionType.Fire1:
			isGun = false;
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Fire2:
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.Boost:
			heatTime = 0;
			fireTime = 0;
			break;
		case ActionType.PopUpLevel2:
			if(ProgressController.GetPlayerLevel() >= unlockMap[0])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				collider.enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[0];
				collider.enabled = false;
			}
			break;
		case ActionType.PopUpLevel3:
			if(ProgressController.GetPlayerLevel() >= unlockMap[1])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				collider.enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[1];
				collider.enabled = false;
			}
			break;
		case ActionType.PopUpLevel4:
			if(ProgressController.GetPlayerLevel() >= unlockMap[2])
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				collider.enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "unlock on lvl " + unlockMap[2];
				collider.enabled = false;
			}
			break;
		case ActionType.PopUpLevel5:
			if(ProgressController.isSas)
			{
				transform.GetComponentInChildren<UILabel>().enabled = false;
				collider.enabled = true;
			}
			else
			{
				transform.GetComponentInChildren<UILabel>().text = "join the sas to unlock";
				collider.enabled = false;
			}
			break;
		case ActionType.PopUpSAS:
			if(ProgressController.isSas)
			{
				GameObject.Find("SAS Button").SetActive(false);
			}
			break;
		case ActionType.NextWave:
			if(TypeAction.type == TypeAction.FREE_FOR_ALL)
			{
				GameObject.Find("KillsLabelText").GetComponent<UILabel>().text = "PLAYER KILLS:";

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
            camera = FindObjectOfType<FlightView>();
			FlightView.Target.GetComponent<FlightSystem>().Boost();
			heatTime = Time.timeSinceLevelLoad + 6f;
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

		switch(action)
		{
		case ActionType.Gold1:
			ProgressController.gold += 4000;
			ProgressController.SaveProgress();
			break;
		case ActionType.Gold2:
			ProgressController.gold += 10000;
			ProgressController.SaveProgress();
			break;
		case ActionType.Gold3:
			ProgressController.gold += 30000;
			ProgressController.SaveProgress();
			break;
		case ActionType.Gold4:
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
			ProgressController.isSas = true;
			var go = GameObject.Find("Dragonfly");
			if(go != null)
			{
				go.GetComponent<PlaneAction>().UpdatePlane(2);
				GameObject.Find("SAS Button").SetActive(false);
			}

			ProgressController.SaveProgress();
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
			Time.timeScale = 1;
			ProgressController.expAdd = 0;
			FlightView.wave = Mathf.Min(73, FlightView.wave + 1);
			var planes = GameObject.FindObjectsOfType<AirplanePath>();
			for(int i = 0; i < planes.Length; i++)
			{
				if(WaveProps.waveProps[FlightView.wave, i] != 0)
				{
					planes[i].plane.gameObject.SetActive(true);
					planes[i].speed = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 2] * 3;
				}
			}
			var waves = GameObject.FindGameObjectsWithTag("Enemy");
			for(int i = 0; i < waves.Length; i++)
			{
				waves[i].GetComponent<DamageManager>().HP = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 1];
				waves[i].GetComponent<FlightOnHit>().Damage = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 0];
			}

			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			break;
		case ActionType.Eject: 
			GameObject _parashute = GameObject.Instantiate(parashute) as GameObject;

			camera = FindObjectOfType<FlightView>();

			_parashute.transform.GetChild(0).gameObject.SetActive(true);
			_parashute.transform.GetChild(1).gameObject.SetActive(true);
			_parashute.transform.position = camera.transform.position;

			FlightView.Target.GetComponent<PlayerManager>().plane.transform.SetParent(_parashute.transform.FindChild("Crate"));
			FlightView.Target.GetComponent<PlayerManager>().plane.transform.localPosition = Vector3.zero;
			_parashute.transform.FindChild("Crate").GetComponent<MeshRenderer>().enabled = false;
			
			Destroy(FlightView.Target);

			FlightView.Target = _parashute.transform.GetChild(0).gameObject;

			gameObject.SetActive(false);
			break;
		}
	}

	public GameObject parashute;
}
