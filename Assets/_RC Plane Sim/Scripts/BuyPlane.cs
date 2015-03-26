using UnityEngine;
using System.Collections;

public class BuyPlane : MonoBehaviour 
{
	public static PlaneAction plane;

	void OnEnable()
	{
		if(plane != null)
			GameObject.Find ("BuyPlaneLabel").GetComponent<UILabel> ().text = "Are you sure you want to buy [99ff00]" + plane.name + "[-] for [99ff00]" + plane.unlockPrice + "[-] coins?";
	}

	void OnClick()
	{
		UIController.GetPanel(PanelType.Type.BuyPlane).gameObject.SetActive(false);

		if(ProgressController.gold < plane.unlockPrice)
		{
			UIController.GetPanel(PanelType.Type.PopUpBuy).gameObject.SetActive(true);
			return;
		}
		ProgressController.gold -= plane.unlockPrice;
		plane.isOwned = true;
		plane.transform.FindChild("Label").GetComponent<UILabel>().enabled = false;
		ProgressController.SaveProgress();
		plane.Click();
	}
}
