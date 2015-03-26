using UnityEngine;
using System.Collections;

public class TextureAction : MonoBehaviour 
{
	public bool left;
	public UILabel label;

	void OnEnable()
	{
		if(PlaneAction.currentPlane != null)
		{
			label.text = PlaneAction.currentPlane.plane.materials [PlaneAction.currentPlane.material].name;
		}
	}

	void OnClick()
	{
		if(left)
		{
			PlaneAction.currentPlane.material = PlaneAction.currentPlane.material - 1;
			if(PlaneAction.currentPlane.material < 0)
				PlaneAction.currentPlane.material = PlaneAction.currentPlane.plane.materials.Length - 1;
			label.text = PlaneAction.currentPlane.plane.materials [PlaneAction.currentPlane.material].name;
		}
		else
		{
			PlaneAction.currentPlane.material = PlaneAction.currentPlane.material + 1;
			if(PlaneAction.currentPlane.material == PlaneAction.currentPlane.plane.materials.Length)
				PlaneAction.currentPlane.material = 0;
			label.text = PlaneAction.currentPlane.plane.materials [PlaneAction.currentPlane.material].name;
		}
		PlaneAction.currentPlane.plane.Adjust();
		ProgressController.SaveProgress ();
	}
}
