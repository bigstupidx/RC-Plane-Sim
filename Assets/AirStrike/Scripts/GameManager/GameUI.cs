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
			GUI.Label (new Rect ((Screen.width - Screen.width * 0.1f) / 2, Screen.height * 0.03f, Screen.width * 0.15f, Screen.width * 0.05f), (int)((play.GetComponent<DamageManager> ().HP / play.GetComponent<DamageManager> ().HPmax) * 100) + "%");

			if(transform.position.x < -1000 || transform.position.x > 1000 || transform.position.z < -1000 || transform.position.z > 1000)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label (new Rect (Screen.width / 2 - 300, 200, 600, 100), "Come back to battle zone or your plane will be crashed");
			}

			GUI.skin.label.fontSize = 26;
		}
		else if(Mode != 1)
		{
			play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
		}
	}
}
