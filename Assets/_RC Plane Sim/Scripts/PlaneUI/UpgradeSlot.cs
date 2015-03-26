using UnityEngine;
using System.Collections;

public class UpgradeSlot : MonoBehaviour
{
	public PlaneAction.Stat.Type type;

	void OnEnable()
	{
		UpdateSlot ();
	}

	private void UpdateSlot()
	{
		PlaneAction.Stat stat = PlaneAction.FindStatType (type);

		if (stat == null)
			return;

		Transform stats;
		int j;

		if(stat.playerLevel == 1)
		{
			transform.FindChild("Stat Name").GetComponent<UILabel>().text = "Unlock";
			if(type == PlaneAction.Stat.Type.Rockets) transform.FindChild("Icon - Front").GetComponent<UISprite>().spriteName = "rocket";
			stats = transform.FindChild ("Plane - StatDelim");
			j = 1;
			for(; j <= 4; j++)
			{
				stats.transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			transform.FindChild ("Button - Buy").transform.localScale = Vector3.one;
			transform.FindChild ("Button - Buy").GetComponentInChildren<UILabel> ().text = "200\ncoins";
			return;
		}

		if(PlaneAction.currentPlane.name.Contains("Dragon") && type == PlaneAction.Stat.Type.Rockets)
		{
			transform.FindChild("Stat Name").GetComponent<UILabel>().text = "laser" + ": " + (int)(stat.levels[stat.currentLevel].value);
			transform.FindChild("Icon - Front").GetComponent<UISprite>().spriteName = "Icon_energy";
		}
		else
		{
			transform.FindChild("Stat Name").GetComponent<UILabel>().text = stat.type.ToString() + ": " + (int)(stat.levels[stat.currentLevel].value);
			if(type == PlaneAction.Stat.Type.Rockets) transform.FindChild("Icon - Front").GetComponent<UISprite>().spriteName = "rocket";
		}

		if(stat.levels [stat.currentLevel].cost == 0)
		{
			transform.FindChild ("Button - Buy").transform.localScale = Vector3.zero;
		}
		else
		{
			transform.FindChild ("Button - Buy").transform.localScale = Vector3.one;
			transform.FindChild ("Button - Buy").GetComponentInChildren<UILabel> ().text = stat.levels [stat.currentLevel].cost + "\ncoins";
		}

		stats = transform.FindChild ("Plane - StatDelim");
		j = 1;
		for(; j <= stat.currentLevel + 1; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + j ).gameObject.SetActive(true);
		}
		for(; j <= 4; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
		}
	}

	public void Click()
	{
		PlaneAction.Stat stat = PlaneAction.FindStatType (type);

		if(stat.playerLevel == 1)
		{
			if(ProgressController.gold < stat.levels [stat.currentLevel].cost)
			{
				PanelType panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
				panel.gameObject.SetActive(true);
				return;
			}

			ProgressController.gold -= 200;

			stat.playerLevel = 0;
			ProgressController.SaveProgress ();
			UpdateSlot ();
			return;
		}

		if(ProgressController.gold < stat.levels [stat.currentLevel].cost)
		{
			PanelType panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(true);
			return;
		}

		ProgressController.gold -= stat.levels [stat.currentLevel].cost;

		stat.currentLevel = Mathf.Min(stat.currentLevel + 1, stat.levels.Length - 1);
		ProgressController.SaveProgress ();
		UpdateSlot ();
	}
}
