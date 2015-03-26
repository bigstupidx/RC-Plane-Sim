using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
    private static PanelType[] panels;
	public static PanelType current;
	public static PanelType previous;

	//Save Data
	public static bool acceleration = false;

	//float deltaTime = 0.0f;
	//float fps = 0.0f;

	// Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        panels = gameObject.GetComponentsInChildren<PanelType>();

        for(int i = 0; i < panels.Length; i++)
        {
            if(panels[i].panelType != PanelType.Type.MainMenu)
            {
                panels[i].gameObject.SetActive(false);
            }
			else
			{
				current = panels[i];
			}
        }

		LevelsLoader.LoadLevel (1);
    }

    void Update()
    {
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
        }
    }

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

	public static void HidePanels()
	{
		for(int i = 0; i < panels.Length; i++)
		{
			panels[i].gameObject.SetActive(false);
		}
	}
}
