using UnityEngine;
using System.Collections;

public class TextureAction : MonoBehaviour 
{
	public bool left;
	public UILabel label;

	void Start()
	{
		label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
	}

	void OnClick()
	{
		if(left)
		{
			PlaneAction.currentMaterial = Mathf.Max(0, PlaneAction.currentMaterial - 1);
			label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
		}
		else
		{
			PlaneAction.currentMaterial = Mathf.Min(PlaneAction.currentPlane.materials.Length - 1, PlaneAction.currentMaterial + 1);
			label.text = PlaneAction.currentPlane.materials [PlaneAction.currentMaterial].name;
		}
		PlaneAction.currentPlane.Adjust();
	}
}
