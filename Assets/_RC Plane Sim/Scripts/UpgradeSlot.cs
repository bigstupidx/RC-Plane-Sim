using UnityEngine;
using System.Collections;

public class UpgradeSlot : MonoBehaviour
{
	public int id;

	void Start()
	{
		transform.FindChild("Stat Name").GetComponent<UILabel>().text = PlaneAction.currentStats.stats[id].type.ToString() + ": " + PlaneAction.currentStats.stats[id].levels[PlaneAction.currentStats.stats[id].currentLevel].value.ToString();
		var stats = transform.FindChild ("Plane - StatDelim");
		int j = 0;
		for(; j <= PlaneAction.currentStats.stats [id].currentLevel; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(true);
		}
		for(; j <= 7; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(false);
		}
	}

	public void Click()
	{
		PlaneAction.currentStats.stats [id].currentLevel = Mathf.Min(PlaneAction.currentStats.stats [id].currentLevel + 1, PlaneAction.currentStats.stats [id].levels.Length - 1);

		transform.FindChild("Stat Name").GetComponent<UILabel>().text = PlaneAction.currentStats.stats[id].type.ToString() + ": " + PlaneAction.currentStats.stats[id].levels[PlaneAction.currentStats.stats[id].currentLevel].value.ToString();
		var stats = transform.FindChild ("Plane - StatDelim");
		int j = 0;
		for(; j <= PlaneAction.currentStats.stats [id].currentLevel; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(true);
		}
		for(; j <= 7; j++)
		{
			stats.transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(false);
		}
	}
}
