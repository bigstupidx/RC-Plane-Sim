using UnityEngine;
using System.Collections;

public class PanelType : MonoBehaviour 
{
    public enum Type
    {
        MainMenu,
        Garage,
        Settings,
        Shop,
		PopUpSAS,
		Upgrade,
		PopUpBuy,
		GameMenu,
		PauseMenu,
		Map,
		PopUpLevel,
		Win
    }

    public Type panelType;
}
