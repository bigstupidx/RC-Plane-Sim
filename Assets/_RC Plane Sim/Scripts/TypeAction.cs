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
			type = type - 1;
			if(type < 0)
				type = types.Length - 1;
		}
		else
		{
			type = type + 1;
			if(type == types.Length)
				type = 0;
		}

		label.text = types[type];
	}
}
