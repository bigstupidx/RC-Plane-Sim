using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;
	public Texture2D Logo;
	public int Mode;
	private GameManager game;
	private PlayerController play;

	UILabel healthLabel;
	UILabel timeLabel;
			
	void OnEnable()
	{
		game = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
	}

	public void OnGUI ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			play.Active = false;
		}
		
		if (skin)
			GUI.skin = skin;

		if (play && Time.timeScale != 0) 
		{
			play.Active = true;
				
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if(healthLabel)
			{
				healthLabel.text = (int)((play.GetComponent<DamageManager> ().HP / play.GetComponent<DamageManager> ().HPmax) * 100) + "%";
			}
			else
			{
				if (GameObject.Find ("LabelGame - Health"))
				{
					healthLabel = GameObject.Find ("LabelGame - Health").GetComponent<UILabel> ();
				}
			}
			if(timeLabel)
			{
				if(GetComponent<FlightView>().gameTime - Time.timeSinceLevelLoad > 0)
				{
					timeLabel.text = ToStringTime(GetComponent<FlightView>().gameTime - Time.timeSinceLevelLoad);
				}
				else if(timeLabel)
				{
					timeLabel.text = "";
				}
			}
			else
			{
				if(GameObject.Find("LabelGame - Time"))
				{
					timeLabel = GameObject.Find("LabelGame - Time").GetComponent<UILabel>();
				}
			}

			GUI.skin.label.fontSize = 26;
		}
		else if(Mode != 1)
		{
			play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
		}
	}

	private string ToStringTime(float time)
	{
		var minutes = ((int)(time / 60)).ToString();
		var seconds = ((int)(time % 60)).ToString();
		if (minutes.Length == 1) minutes = "0" + minutes;
		if (seconds.Length == 1) seconds = "0" + seconds;
		return minutes + ":" + seconds;
	}
}
