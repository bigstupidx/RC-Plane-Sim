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
		transform.FindChild("Stat Name").GetComponent<UILabel>().text = stat.type.ToString() + ": " + stat.levels[stat.currentLevel].value.ToString();
		if(stat.levels [stat.currentLevel].cost == 0)
		{
			transform.FindChild ("Button - Buy").transform.localScale = Vector3.zero;
		}
		else
		{
			transform.FindChild ("Button - Buy").transform.localScale = Vector3.one;
			transform.FindChild ("Button - Buy").GetComponentInChildren<UILabel> ().text = stat.levels [stat.currentLevel].cost.ToString ();
		}

		var stats = transform.FindChild ("Plane - StatDelim");
		int j = 1;
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

		if(ProgressController.gold < stat.levels [stat.currentLevel].cost)
		{
			PanelType panel = UIController.GetPanel(PanelType.Type.PopUpBuy);
			panel.gameObject.SetActive(true);
			return;
		}

		ProgressController.gold -= stat.levels [stat.currentLevel].cost;

		stat.currentLevel = Mathf.Min(stat.currentLevel + 1, stat.levels.Count - 1);
		ProgressController.SaveProgress ();
		UpdateSlot ();
	}
}
