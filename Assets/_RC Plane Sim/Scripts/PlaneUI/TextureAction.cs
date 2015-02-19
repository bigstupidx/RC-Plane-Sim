using UnityEngine;
using System.Collections;

public class TextureAction : MonoBehaviour 
{
	public bool left;
	public UILabel label;

	void OnEnable()
	{
		label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
	}

	void OnClick()
	{
		if(left)
		{
			PlaneAction.currentMaterial = PlaneAction.currentMaterial - 1;
			if(PlaneAction.currentMaterial < 0)
				PlaneAction.currentMaterial = PlaneAction.currentPlane.materials.Length - 1;
			label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
		}
		else
		{
			PlaneAction.currentMaterial = PlaneAction.currentMaterial + 1;
			if(PlaneAction.currentMaterial == PlaneAction.currentPlane.materials.Length)
				PlaneAction.currentMaterial = 0;
			label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
		}
		PlaneAction.currentPlane.Adjust();
	}
}
