using UnityEngine;
using System.Collections;

public class TypeAction : MonoBehaviour 
{
	public bool left;
	public UILabel label;

	private string[] types = new string[3]{"FREE FOR ALL", "SURVIVAL", "FREE FLIGHT"};
	public static int type = 0;
	
	void Start()
	{
		label.text = types[type];
	}
	
	void OnClick()
	{
		if(left)
		{
			type = Mathf.Max(0, type - 1);
		}
		else
		{
			type = Mathf.Min(types.Length - 1, type + 1);
		}

		label.text = types[type];
	}
}
