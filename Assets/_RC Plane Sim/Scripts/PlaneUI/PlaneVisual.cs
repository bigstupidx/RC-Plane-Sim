using UnityEngine;
using System.Collections;

public class PlaneVisual : MonoBehaviour 
{
	private PlaneVisualStat[] stats;
	private UILabel planeName;

	void Start()
	{
		stats = gameObject.GetComponentsInChildren<PlaneVisualStat> ();
		planeName = transform.FindChild ("Plane - Name").GetComponent<UILabel> ();

		UpdateStats ();
	}

	public void UpdateStats()
	{
		//update plane stats at garage ui after upgrading it or reselected plane
		if (PlaneAction.currentPlane == null)
						return;

		GameObject.Find ("Plane - Description").GetComponent<UILabel> ().text = PlaneAction.currentPlane.plane.desc;

		PlaneAction.currentPlane.plane.Adjust ();

		planeName.text = PlaneAction.currentPlane.gameObject.name;

		for(int i = 0; i < stats.Length; i++)
		{
			stats[i].UpdateSlot();
		}
	}
}
