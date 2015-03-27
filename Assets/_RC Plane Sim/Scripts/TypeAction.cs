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
		if(left)
		{
			return;
		}

		ProgressController.level = level;
		label.text = types[type];

		if(type == FREE_FOR_ALL)
		{
			score.text = "Swipe to select difficulty level";
			score.GetComponent<TweenAlpha>().from = 1f;
			score.GetComponent<TweenAlpha>().to = 0;
			score.GetComponent<TweenAlpha>().ResetToBeginning();
			score.GetComponent<TweenAlpha> ().Play ();
		}
		else if(type == SURVIVAL)
		{
			score.text = "[99ff00]Highscore:[-]\n" + ProgressController.scoreWave[level] + " Waves";
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
	}

	public void OnAlphaComplete()
	{
		//change helping text to highscore after animation complete
		if(score.color.a != 0)
			return;

		score.GetComponent<TweenAlpha>().from = 0;
		score.GetComponent<TweenAlpha>().to = 1f;
		score.GetComponent<TweenAlpha>().ResetToBeginning();
		score.GetComponent<TweenAlpha> ().Play ();

		if(type == FREE_FOR_ALL)
		{
			score.text = "[99ff00]Highscore:[-]\n" + ProgressController.scoreFree[level, SwipeAction.levelDifficult - 1] + " Kills  " + ProgressController.scoreDeath[ProgressController.level, SwipeAction.levelDifficult - 1] + " Deaths";
		}
		else
		{
			score.text = "[99ff00]Highscore:[-]\n" +  ProgressController.scoreWave[level] + " Waves";
		}
	}
	
	void OnClick()
	{
		//change game type on arrow press
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

		//change highscore text according to game type
		if(type == FREE_FOR_ALL)
		{
			score.text = "Swipe to select difficulty level";
			score.GetComponent<TweenAlpha>().from = 1;
			score.GetComponent<TweenAlpha>().to = 0;
			score.GetComponent<TweenAlpha>().ResetToBeginning();
			score.GetComponent<TweenAlpha> ().Play ();
		}
		else if(type == SURVIVAL)
		{
			score.text = "[99ff00]Highscore:[-]\n" + ProgressController.scoreWave[level] + " Waves";
			score.GetComponent<TweenAlpha>().ResetToBeginning();
			score.GetComponent<TweenAlpha> ().enabled = false;
			score.alpha = 1f;
		}
		//show difficulty selection according to game type
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
