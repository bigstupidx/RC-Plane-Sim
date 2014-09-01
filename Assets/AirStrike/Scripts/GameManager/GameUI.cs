using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{

	public GUISkin skin;
	public Texture2D Logo;
	public int Mode;
	private GameManager game;
	private PlayerController play;
	private WeaponController weapon;
			
	void Start ()
	{
		game = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
		weapon = play.GetComponent<WeaponController> ();
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
				
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label (new Rect (Screen.width - 220, 20, 200, 50), "ARMOR " + play.GetComponent<DamageManager> ().HP);

			if(transform.position.x < 1200 || transform.position.x > 2800 || transform.position.z < 1200 || transform.position.z > 2800)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label (new Rect (Screen.width / 2 - 300, 200, 600, 100), "Come back to battle zone or your plane will be crashed");
			}

			GUI.skin.label.fontSize = 16;
				
			// Draw Weapon system
			if (weapon.WeaponLists [weapon.CurrentWeapon].Icon)
			{
				GUI.DrawTexture (new Rect (Screen.width - 100, Screen.height - 100, 80, 80), weapon.WeaponLists [weapon.CurrentWeapon].Icon);
			}
				
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			if (weapon.WeaponLists [weapon.CurrentWeapon].Ammo <= 0 && weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess > 0) 
			{
				if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
				{
					GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), "Reloading " + Mathf.Floor ((1 - weapon.WeaponLists [weapon.CurrentWeapon].ReloadingProcess) * 100) + "%");
				}
			} 
			else 
			{
				if (!weapon.WeaponLists [weapon.CurrentWeapon].InfinityAmmo)
				{
					GUI.Label (new Rect (Screen.width - 230, Screen.height - 120, 200, 30), weapon.WeaponLists [weapon.CurrentWeapon].Ammo.ToString ());
				}
			}
		}
		else if(Mode != 1)
		{
			play = (PlayerController)GameObject.FindObjectOfType (typeof(PlayerController));
			weapon = play.GetComponent<WeaponController> ();
		}
	}
}
