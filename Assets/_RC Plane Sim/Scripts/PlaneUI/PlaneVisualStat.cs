using UnityEngine;
using System.Collections;

public class PlaneVisualStat : MonoBehaviour 
{
	public enum Type
	{
		Range,
		Firepower,
		Accuracy,
		Armor,
		Speed
	}

	public Type type;

	public void UpdateSlot()
	{
		PlaneAction.Stat param1;
		PlaneAction.Stat param2;

		float value1;
		float value2;

		float value;

		int roundValue;

		int j = 1;

		switch(type)
		{
		case Type.Range:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Machinegun);
			param2 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets);

			value1 = param1.levels[param1.currentLevel].value;
			value2 = param2.levels[param1.currentLevel].value;

			value = (value1 / 2.83f) + (value2 / 35) + 1;
			roundValue = (int)value;	

			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= Mathf.Min(roundValue, 20) / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.one;
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.zero;
			}
			break;
		case Type.Firepower:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Machinegun);
			param2 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets);

			value1 = param1.levels[param1.currentLevel].value;
			value2 = param2.levels[param1.currentLevel].value;
			
			value = (value1 / 2.83f) + (value2 / 35) + 1;
			roundValue = (int)value;
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= Mathf.Min(roundValue, 20) / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.one;
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.zero;
			}
			break;
		case Type.Accuracy:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Machinegun);
			param2 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets);
			
			value1 = param1.levels[param1.currentLevel].value;
			value2 = param2.levels[param1.currentLevel].value;
			
			value = (value1 / 2.83f) + (value2 / 35) + 1;
			roundValue = (int)value;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= Mathf.Min(roundValue, 20) / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.one;
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.zero;
			}
			break;
		case Type.Armor:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Armour);
			
			value1 = param1.levels[param1.currentLevel].value;
			
			value = (value1 / 1382f) * 20f;
			roundValue = (int)value;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= Mathf.Min(roundValue, 20) / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.one;
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.zero;
			}
			break;
		case Type.Speed:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Speed);
			
			value1 = param1.levels[param1.currentLevel].value;

			value = (value1 / 89f) * 20f;
			roundValue = (int)value;	

			if(roundValue <= 10)
			{
				transform.FindChild("Stat Param").GetComponent<UILabel>().text = "Slow";
			}
			else if(roundValue <= 14)
			{
				transform.FindChild("Stat Param").GetComponent<UILabel>().text = "Medium";
			}
			else if(roundValue <= 20)
			{
				transform.FindChild("Stat Param").GetComponent<UILabel>().text = "Fast";
			}

			for(; j <= Mathf.Min(roundValue, 20) / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.one;
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).transform.localScale = Vector3.zero;
			}
			break;
		}
	}
}
