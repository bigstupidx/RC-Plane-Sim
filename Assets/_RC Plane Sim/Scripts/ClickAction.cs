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
		GamePlay,
		Pause,
		Resume,
		Retry,
		Exit,
		Eject
	}

	public ActionType action;
	private PanelType panel;

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

	void OnClick()
	{
		switch(action)
		{
		case ActionType.SinglePlayer:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Garage);
			UIController.current.gameObject.SetActive(true);
			if(Application.loadedLevel != 2)  Application.LoadLevel(2);
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
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			if(Application.loadedLevel != 1) Application.LoadLevel(1);
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
		case ActionType.GamePlay:
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			if(Application.loadedLevel != 3)  Application.LoadLevel(3);
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
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
			UIController.current.gameObject.SetActive(true);
			Application.LoadLevel(3);
			break;
		case ActionType.Exit:
			Time.timeScale = 1;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
			UIController.current.gameObject.SetActive(true);
			Application.LoadLevel(1);
			break;
		case ActionType.Eject: 
			var parashute = GameObject.FindGameObjectWithTag("Parashute");
			var camera = FindObjectOfType<FlightView>();

			parashute.transform.GetChild(0).gameObject.SetActive(true);
			parashute.transform.GetChild(1).gameObject.SetActive(true);
			parashute.transform.position = camera.transform.position;

			camera.Target = parashute.transform.GetChild(0).gameObject;
			break;
		}
	}
}
