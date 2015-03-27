using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
	//list of all ui screens
    private static PanelType[] panels;
	public static PanelType current;
	public static PanelType previous;
	
	public static bool acceleration = false;

	// Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
		//get all ui screens
        panels = gameObject.GetComponentsInChildren<PanelType>();

        for(int i = 0; i < panels.Length; i++)
        {
			//set main menu ui to visible
            if(panels[i].panelType != PanelType.Type.MainMenu)
            {
                panels[i].gameObject.SetActive(false);
            }
			else
			{
				current = panels[i];
			}
        }
		//load main menu level
		LevelsLoader.LoadLevel (1);
    }

    void Update()
    {
		//return to previous screen or quit or pause game on device back button
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(current.panelType == PanelType.Type.Garage)
            {
                UIController.HidePanels();
                UIController.current.gameObject.SetActive(false);
                UIController.previous = UIController.current;
                UIController.current = UIController.GetPanel(PanelType.Type.MainMenu);
                UIController.current.gameObject.SetActive(true);
                if (Application.loadedLevel != 1) LevelsLoader.LoadLevel(1);
            }
            else if(current.panelType == PanelType.Type.Map)
            {
                UIController.HidePanels();
                Time.timeScale = 1;
                if (Application.loadedLevel != 2) LevelsLoader.LoadLevel(2);
                UIController.current.gameObject.SetActive(false);
                UIController.previous = UIController.current;
                UIController.current = UIController.GetPanel(PanelType.Type.Garage);
                UIController.current.gameObject.SetActive(true);
            }
			else if(current.panelType == PanelType.Type.MainMenu)
			{
				Application.Quit();
			}
			else if (Application.loadedLevel > 2) 
			{
				Screen.lockCursor = false;
				Time.timeScale = 0;
				current.gameObject.SetActive(false);
				previous = UIController.current;
				current = UIController.GetPanel(PanelType.Type.PauseMenu);
				current.gameObject.SetActive(true);
			}
        }
    }

	//get ui screen by type
	public static PanelType GetPanel(PanelType.Type type)
	{
		for(int i = 0; i < panels.Length; i++)
		{
			if(panels[i].panelType == type)
			{
				return panels[i];
			}
		}

		return null;
	}

	//hide all ui screens
	public static void HidePanels()
	{
		for(int i = 0; i < panels.Length; i++)
		{
			panels[i].gameObject.SetActive(false);
		}
	}
}
