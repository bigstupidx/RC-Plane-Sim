using UnityEngine;
using System.Collections;

public class PlaneVisual : MonoBehaviour 
{
	private PlaneVisualStat[] stats;

	void Start()
	{
		stats = gameObject.GetComponentsInChildren<PlaneVisualStat> ();

		UpdateStats ();
	}

	public void UpdateStats()
	{
		for(int i = 0; i < stats.Length; i++)
		{
			stats[i].UpdateSlot();
		}
	}
}
