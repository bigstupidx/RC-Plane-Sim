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
		PopUpBuy
    }

    public Type panelType;
}
