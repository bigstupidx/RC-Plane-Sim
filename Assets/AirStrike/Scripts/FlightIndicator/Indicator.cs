/// <summary>
/// Nav mode. this script will display an enemys, cockpit HUD , crosshair and define a plane cameras.
/// </summary>
using UnityEngine;
using System.Collections;

public enum NavMode
{
	Third,
	Cockpit,
	None
}

public class Indicator : MonoBehaviour
{
	public string[] TargetTag;// all target tag
	public Texture2D[] NavTexture;
	public Texture2D Crosshair;
	public Texture2D Crosshair_in;
	public Vector2 CrosshairOffset;
	public Vector2 CrosshairOffset_in;
	public float DistanceSee = 800; // limited distance
	public float Alpha = 0.7f;// GUI opacity.
	
	public Camera[] CockpitCamera;// cameras list
	public int PrimaryCameraIndex;// which camera is the cockpit camera
	
	public bool Show = true;
	
	[HideInInspector]
	public NavMode Mode = NavMode.Third;// camera mode
	[HideInInspector]
	public FlightSystem flight;// core plane system

	private float scale;

	void Awake ()
	{
		scale = 500f / Screen.width;
		flight = this.GetComponent<FlightSystem> ();
	}
	
	void Start ()
	{
		Mode = NavMode.Third;
	}

	public void DrawNavEnemy ()
	{
		// find all target in TargetTag[]
		for (int t = 0; t < TargetTag.Length; t++) 
		{
			if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) 
			{
				GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
				for (int i = 0; i < objs.Length; i++) 
				{
					if (objs [i]) 
					{
						Vector3 dir = (objs [i].transform.position - transform.position).normalized;
						float direction = Vector3.Dot (dir, transform.forward);

						if (direction >= 0.7f) 
						{
							float dis = Vector3.Distance (objs [i].transform.position, transform.position);

							if (DistanceSee > dis) 
							{
								DrawTargetLockon (objs [i].transform, t);
							}
						}
						else
						{
							Vector3 screenPos = Camera.main.WorldToScreenPoint (objs [i].transform.position);
							int type = 0;
							if(screenPos.x - (Screen.width * 0.05f) / 2 <= 0)
							{
								GUI.color = Color.red;
								type = 1;
							}
							else if(screenPos.x - (Screen.width * 0.05f) / 2 >= Screen.width)
							{
								GUI.color = Color.red;
								type = 2;
							}

							if(type == 0)
								return;

							Rect position = new Rect (Mathf.Clamp(screenPos.x - (Screen.width * 0.05f) / 2, 0, Screen.width - (Screen.width * 0.05f)), 
							                          Mathf.Clamp(Screen.height - screenPos.y - (Screen.width * 0.05f) / 2, 0, Screen.height), 
							                          Screen.width * 0.05f, 
							                          Screen.width * 0.05f);
							GUI.DrawTexture (position, NavTexture [type]);
						}
					}
				}
			}
		}
	}

	void OnGUI ()
	{
		if(Time.timeScale == 0)
			return;

		if (Show) 
		{
			GUI.color = new Color (1, 1, 1, Alpha);
			switch (Mode) 
			{
			case NavMode.Third:
				if (Crosshair)
					GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair.width / 2) + CrosshairOffset.x, (Screen.height / 2 - Crosshair.height / 2) + CrosshairOffset.y, Crosshair.width * scale, Crosshair.height * scale), Crosshair);	
				DrawNavEnemy ();
				break;
			case NavMode.Cockpit:
				if (Crosshair_in)
					GUI.DrawTexture (new Rect ((Screen.width / 2 - Crosshair_in.width / 2) + CrosshairOffset_in.x, (Screen.height / 2 - Crosshair_in.height / 2) + CrosshairOffset_in.y, Crosshair_in.width, Crosshair_in.height), Crosshair_in);	
				DrawNavEnemy ();
				break;
			case NavMode.None:
				break;
			}
		}
	}
	
	public void DrawTargetLockon (Transform aimtarget, int type)
	{
		if (Camera.main && Camera.main.GetComponent<Camera>() != null) 
		{
			Vector3 dir = (aimtarget.position - Camera.main.transform.position).normalized;
			float direction = Vector3.Dot (dir, Camera.main.transform.forward);
			
			if (direction > 0.5f) 
			{
				Vector3 screenPos = Camera.main.WorldToScreenPoint (aimtarget.transform.position);
				type = 0;
				GUI.color = Color.white;

				Rect position = new Rect (screenPos.x - (Screen.width * 0.1f) / 2, 
				                          Screen.height - screenPos.y - (Screen.width * 0.1f) / 2, 
				                          Screen.width * 0.1f, 
				                          Screen.width * 0.1f);
				GUI.DrawTexture (position, NavTexture [type]);
			}
		}
	}
}
