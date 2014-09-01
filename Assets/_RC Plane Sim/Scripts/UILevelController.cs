using UnityEngine;
using System.Collections;

public class UILevelController : MonoBehaviour 
{
	private GameObject[] levels;
	private UISprite foreground;

	private int[] exps = new int[9] {
		0,
		500,
		1250,
		2250,
		3500,
		5000,
		6750,
		8750,
		11000
	};

	void OnEnable()
	{
		if(levels == null)
		{
			levels = GameObject.FindGameObjectsWithTag ("Level");
			foreground = transform.FindChild("Level Foreground").GetComponent<UISprite>();
		}

		int i = 0;
		int index = 0;

		for(; i < exps.Length; i++)
		{
			if(UIController.exp > exps[i])
			{
				index = i;
			}
		}

		int current = UIController.exp - exps [index];
		float percent = current / (float)(exps [index + 1] - exps [index]);

		foreground.width = (int)(383 * percent);

		i = 0;

		for(; i < levels.Length; i++)
		{
			if(levels[i].name != index.ToString())
			{
				levels[i].SetActive(false);
			}
		}
	}
}
