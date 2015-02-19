using UnityEngine;
using System.Collections;

public class TypeAction : MonoBehaviour 
{
	public bool left;
	public UILabel label;
	public UILabel score;
	public int level;

	public const int FREE_FOR_ALL = 0;
	public const int SURVIVAL = 1;
	public const int FREE_FLIGHT = 2;

	private string[] types = new string[3]{"FREE FOR ALL", "SURVIVAL", "FREE FLIGHT"};
	public static int type = 0;
	
	void OnEnable()
	{
		ProgressController.level = level;
		label.text = types[type];
		score.text = "Swipe to select difficulty level";
		if((ProgressController.scoreFree[level] != 0 && type == FREE_FOR_ALL) || (ProgressController.scoreWave[level] != 0 && type == SURVIVAL))
		{
			score.GetComponent<TweenAlpha> ().PlayForward ();
		}
	}

	public void OnAlphaComplete()
	{
		if(score.color.a == 1f)
			return;

		if(type == FREE_FOR_ALL)
		{
			score.text = ProgressController.scoreFree[level] + " Kills";
		}
		else
		{
			score.text = ProgressController.scoreWave[level] + " Waves";
		}

		score.GetComponent<TweenAlpha> ().PlayReverse ();
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
		score.text = "Swipe to select difficulty level";
		if((ProgressController.scoreFree[level] != 0 && type == FREE_FOR_ALL) || (ProgressController.scoreWave[level] != 0 && type == SURVIVAL))
		{
			score.GetComponent<TweenAlpha> ().PlayForward ();
		}

		if(type == FREE_FOR_ALL)
		{
			score.enabled = true;
			GameObject.FindObjectOfType<SwipeAction>().enabled = true;
			GameObject.FindObjectOfType<SwipeAction>().transform.localScale = Vector3.one;
		}
		else if(type == SURVIVAL)
		{
			score.enabled = true;
			GameObject.FindObjectOfType<SwipeAction>().enabled = false;
			GameObject.FindObjectOfType<SwipeAction>().transform.localScale = Vector3.zero;
		}
		else
		{
			score.enabled = false;
			GameObject.FindObjectOfType<SwipeAction>().enabled = false;
			GameObject.FindObjectOfType<SwipeAction>().transform.localScale = Vector3.zero;
		}

		label.text = types[type];
	}
}
