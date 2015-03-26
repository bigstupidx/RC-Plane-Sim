using UnityEngine;
using System.Collections;

public class UnlockController : MonoBehaviour 
{
	bool level2;
	bool level3;
	bool level4;
	bool level5;
	bool level6;
	bool level7;
	bool level9;

	void OnLevelWasLoaded(int level) 
	{
		if(level != 2)
		{
			return;
		}

		level2 = PlayerPrefs.GetInt ("Level2") != 0;
		level3 = PlayerPrefs.GetInt ("Level3") != 0;
		level4 = PlayerPrefs.GetInt ("Level4") != 0;
		level5 = PlayerPrefs.GetInt ("Level5") != 0;
		level6 = PlayerPrefs.GetInt ("Level6") != 0;
		level7 = PlayerPrefs.GetInt ("Level7") != 0;
		level9 = PlayerPrefs.GetInt ("Level9") != 0;

		if(!level2 && ProgressController.GetPlayerLevel() == 2)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'GAS STATION'\n\nlevel";
			PlayerPrefs.SetInt("Level2", 1);
		}
		else if(!level3 && ProgressController.GetPlayerLevel() == 3)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'War Angel'\n\nplane";
			PlayerPrefs.SetInt("Level3", 1);
		}
		else if(!level4 && ProgressController.GetPlayerLevel() == 4)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'GYM'\n\nlevel";
			PlayerPrefs.SetInt("Level4", 1);
		}
		else if(!level5 && ProgressController.GetPlayerLevel() == 5)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'Warthog AX'\n\nplane";
			PlayerPrefs.SetInt("Level5", 1);
		}
		else if(!level6 && ProgressController.GetPlayerLevel() == 6)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'METRO'\n\nlevel";
			PlayerPrefs.SetInt("Level6", 1);
		}
		else if(!level7 && ProgressController.GetPlayerLevel() == 7)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'Golden Eagle'\n\nplane";
			PlayerPrefs.SetInt("Level7", 1);
		}
		else if(!level9 && ProgressController.GetPlayerLevel() == 9)
		{
			var panel = UIController.GetPanel(PanelType.Type.Unlock);
			panel.gameObject.SetActive(true);
			GameObject.Find("LabelUnlock").GetComponent<UILabel>().text = "Congratulation!\nYou've unlocked\n\n'Predator'\n\nplane";
			PlayerPrefs.SetInt("Level9", 1);
		}
	}
}
