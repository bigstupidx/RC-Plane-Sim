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
		GamePlay
	}

	public ActionType action;
	private PanelType panel;

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
			if(Application.loadedLevel != 3)  Application.LoadLevel(3);
			break;
		}
	}
}
