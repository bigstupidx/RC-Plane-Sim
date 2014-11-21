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
		PopUpSASYes
	}

	public ActionType action;
	private PanelType panel;
	private float fireTime = 0;
	private float heatTime = 0;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Screen.lockCursor = false;
			Time.timeScale = 0;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.PauseMenu);
			UIController.current.gameObject.SetActive(true);
		}
	}

	public void Press()
	{
		FlightView camera;
		PlaneStatAdjustment stat;
		
		switch(action)
		{
		case ActionType.Fire1:
			camera = FindObjectOfType<FlightView>();
			stat = FindObjectOfType<PlaneStatAdjustment>();

			if(fireTime == 0)
			{
				fireTime = Time.timeSinceLevelLoad + stat.minigun.ReloadTime;
			}

			if(fireTime < Time.timeSinceLevelLoad)
			{
				heatTime = Time.timeSinceLevelLoad + stat.minigun.AmmoMax;
			}
			Debug.Log(fireTime + " " + heatTime);
			if(heatTime > Time.timeSinceLevelLoad)
				return;

			camera.Target.GetComponent<FlightSystem>().WeaponControl.LaunchWeapon(0);
			break;
		case ActionType.Fire2:
			camera = FindObjectOfType<FlightView>();
			camera.Target.GetComponent<FlightSystem>().WeaponControl.LaunchWeapon(1);
			break;
        case ActionType.Boost:
            camera = FindObjectOfType<FlightView>();
			camera.Target.GetComponent<FlightSystem>().Boost();
            break;
		}
	}

	public void Reveal()
	{
		fireTime = 0;
	}

	void OnClick()
	{
		FlightView camera;

		switch(action)
		{
		case ActionType.SinglePlayer:
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
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel1);
			panel.gameObject.SetActive(true);
			break;
        case ActionType.PopUpLevel2:
            panel = UIController.GetPanel(PanelType.Type.PopUpLevel2);
            panel.gameObject.SetActive(true);
            break;
		case ActionType.PopUpLevel3:
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel3);
			panel.gameObject.SetActive(true);
			break;
		case ActionType.ClosePopUpLevel:
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel1);
            panel.gameObject.SetActive(false);
            panel = UIController.GetPanel(PanelType.Type.PopUpLevel2);
			panel.gameObject.SetActive(false);
			panel = UIController.GetPanel(PanelType.Type.PopUpLevel3);
			panel.gameObject.SetActive(false);
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
		case ActionType.PopUpSASYes:
			ProgressController.isSas = true;
			var go = GameObject.Find("Dragonfly").GetComponent<PlaneAction>();
			if(go != null)
			{
				go.UpdatePlane(2);
			}
			ProgressController.SaveProgress();
			panel = UIController.GetPanel(PanelType.Type.PopUpSAS);
			panel.gameObject.SetActive(false);
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
		case ActionType.Eject: 
			var parashute = GameObject.FindGameObjectWithTag("Parashute");
			camera = FindObjectOfType<FlightView>();

			parashute.transform.GetChild(0).gameObject.SetActive(true);
			parashute.transform.GetChild(1).gameObject.SetActive(true);
			parashute.transform.position = camera.transform.position;

			camera.Target = parashute.transform.GetChild(0).gameObject;

			gameObject.SetActive(false);
			break;
		}
	}
}
