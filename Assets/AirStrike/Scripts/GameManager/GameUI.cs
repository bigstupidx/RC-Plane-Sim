using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;
	public Texture2D Logo;
	public int Mode;
	private GameManager game;
	private PlayerController play;
			
	void Start ()
	{
		game = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
		// define player
		
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
			if(GameObject.Find("LabelGame - Health"))
				GameObject.Find("LabelGame - Health").GetComponent<UILabel>().text = (int)((play.GetComponent<DamageManager> ().HP / play.GetComponent<DamageManager> ().HPmax) * 100) + "%";
			if(GetComponent<FlightView>().gameTime - Time.timeSinceLevelLoad > 0 && GameObject.Find("LabelGame - Time"))
			{
				GameObject.Find("LabelGame - Time").GetComponent<UILabel>().text = ToStringTime(GetComponent<FlightView>().gameTime - Time.timeSinceLevelLoad);
			}
			else if(GameObject.Find("LabelGame - Time"))
			{
				GameObject.Find("LabelGame - Time").GetComponent<UILabel>().text = "";
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
