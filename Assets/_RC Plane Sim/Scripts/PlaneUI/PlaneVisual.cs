using UnityEngine;
using System.Collections;

public class PlaneVisual : MonoBehaviour 
{
	public GameObject[] stats;

	void Start()
	{
		UpdateStats ();
	}

	public void UpdateStats()
	{
		for(int i = 0; i < stats.Length; i++)
		{
			stats[i].transform.FindChild("Stat Param").GetComponent<UILabel>().text = PlaneAction.currentStats.stats[i].levels[PlaneAction.currentStats.stats[i].currentLevel].value.ToString();
			int j = 0;
			for(; j <= PlaneAction.currentStats.stats[i].currentLevel; j++)
			{
				stats[i].transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(true);
			}
			for(; j <= 7; j++)
			{
				stats[i].transform.FindChild("Plane - StatDelim" + (j + 1)).gameObject.SetActive(false);
			}

		}
	}
}
