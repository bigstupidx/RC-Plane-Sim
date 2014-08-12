﻿using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
    private static PanelType[] panels;
	public static PanelType current;
	public static PanelType previous;

	// Use this for initialization
    void Awake()
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
}
