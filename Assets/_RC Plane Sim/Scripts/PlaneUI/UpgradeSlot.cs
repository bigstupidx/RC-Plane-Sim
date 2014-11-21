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
		stat.currentLevel = Mathf.Min(stat.currentLevel + 1, stat.levels.Count - 1);
		ProgressController.SaveProgress ();
		UpdateSlot ();
	}
}
