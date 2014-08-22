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

			value = (value1 + 5f + (value2 / 10f) + (2f * 4.3f) + (value2 / 15f) + (5f * 2.7f)) / 6f + 1f;
			roundValue = (int)value;	

			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= roundValue / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(true);
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			break;
		case Type.Firepower:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Machinegun);
			param2 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets);

			value1 = param1.levels[param1.currentLevel].value;
			value2 = param2.levels[param1.currentLevel].value;
			
			value  = (value1 + 5f + (value2 / 10f) + (2f * 4.3f) + (value2 / 15f) + (5f * 2.7f)) / 6f + 1f;
			roundValue = (int)value;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= roundValue / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(true);
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			break;
		case Type.Accuracy:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Machinegun);
			param2 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Rockets);
			
			value1 = param1.levels[param1.currentLevel].value;
			value2 = param2.levels[param1.currentLevel].value;
			
			value  = (value1 + 5f + (value2 / 10f) + (2f * 4.3f) + (value2 / 15f) + (5f * 2.7f)) / 6f + 1f;
			roundValue = (int)value;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= roundValue / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(true);
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			break;
		case Type.Armor:
			param1 = PlaneAction.FindStatType(PlaneAction.Stat.Type.Armour);
			
			value1 = param1.levels[param1.currentLevel].value;
			
			value  = value1 / 100f + 1f;
			roundValue = (int)value;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= roundValue / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(true);
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			break;
		case Type.Speed:
			roundValue = 10;	
			
			transform.FindChild("Stat Param").GetComponent<UILabel>().text = roundValue.ToString();

			for(; j <= roundValue / 2; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(true);
			}
			for(; j <= 10; j++)
			{
				transform.FindChild("Plane - StatDelim" + j).gameObject.SetActive(false);
			}
			break;
		}
	}
}
