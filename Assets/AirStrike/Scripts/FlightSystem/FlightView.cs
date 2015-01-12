﻿/// <summary>
/// Flight view. this script is the Camera Follower
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class FlightView : MonoBehaviour
{
    // Zone of battle
    [HideInInspector]
    public Vector2 xBattle;
    [HideInInspector]
    public Vector2 zBattle;

	public GameObject Target;// player ( Your Plane)
	public GameObject[] Cameras;// all cameras in the game ( in case of able to swith the views).
	public float FollowSpeedMult = 0.5f; // camera following speed 
	public float TurnSpeedMult = 5; // camera turning speed 
	private int indexCamera;// current camera index
	public Vector3 Offset = new Vector3 (10, 0.5f, 10);// position offset between plan and camera
	private float dieTime;
	private float gameTime;
	public static int wave;
	public static bool eject;

	private Vector3 positionCamera;
	private Quaternion quaternionCamera;
	
	// camera swith
	public void SwitchCameras ()
	{
		indexCamera += 1;
		if (indexCamera >= Cameras.Length) {
			indexCamera = 0;
		}
		for (int i =0; i<Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i].camera)
				Cameras [i].camera.enabled = false;
			if (Cameras [i] && Cameras [i].GetComponent<AudioListener> ())
				Cameras [i].GetComponent<AudioListener> ().enabled = false;
		}
		if (Cameras [indexCamera]) {
			if (Cameras [indexCamera] && Cameras [indexCamera].camera)
				Cameras [indexCamera].camera.enabled = true;
			if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<AudioListener> ())
				Cameras [indexCamera].GetComponent<AudioListener> ().enabled = true;
		}
	}

	public void AddCamera(GameObject cam)
	{
		GameObject[] temp = new GameObject[Cameras.Length+1];
		Cameras.CopyTo(temp, 0);
		Cameras = temp;
		Cameras[temp.Length-1] = cam;
	}
	
	void Start ()
	{
		// add this camera to primery
		positionCamera = transform.GetComponentInParent<Transform>().position;
		quaternionCamera = transform.GetComponentInParent<Transform>().rotation;
		
		Instantiate (PlaneAction.currentPlane.gameObject, positionCamera, quaternionCamera);

		AddCamera(this.gameObject);
		
		wave = 0;
		gameTime = 0;

		eject = false;
		
		Time.timeScale = 1;
		UIController.HidePanels();
		UIController.current.gameObject.SetActive(false);
		UIController.previous = UIController.current;
		UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
		UIController.current.gameObject.SetActive(true);
		
		xBattle = new Vector2 (-1200, 1200);
		zBattle = new Vector2 (-1200, 1200);
		AirplanePath[] planes;
		switch(TypeAction.type)
		{
		case TypeAction.FREE_FOR_ALL:
			var enemy = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject go in enemy)
			{
				go.GetComponent<DamageManager>().HP = 80 + 10 * SwipeAction.levelDifficult;
				go.GetComponent<FlightOnHit>().Damage = 5 + 1 * SwipeAction.levelDifficult;
			}

			gameTime = Time.timeSinceLevelLoad + 300;
			break;
		case TypeAction.SURVIVAL:
			planes = GameObject.FindObjectsOfType<AirplanePath>();
			for(int i = 0; i < planes.Length; i++)
			{
				if(WaveProps.waveProps[wave, i] != 0)
				{
					planes[i].speed = WaveProps.waveStats[WaveProps.waveProps[wave, i], 2] * 3;
				}
				else
				{
					planes[i].plane.gameObject.SetActive(false);
				}
			}
			var waves = GameObject.FindGameObjectsWithTag("Enemy");
			for(int i = 0; i < waves.Length; i++)
			{
				waves[i].GetComponent<DamageManager>().HP = WaveProps.waveStats[WaveProps.waveProps[wave, i], 1];
				waves[i].GetComponent<FlightOnHit>().Damage = WaveProps.waveStats[WaveProps.waveProps[wave, i], 0];
			}
			break;
		case TypeAction.FREE_FLIGHT:
			planes = GameObject.FindObjectsOfType<AirplanePath>();
			foreach(AirplanePath p in planes)
			{
				p.gameObject.SetActive(false);
			}
			break;
		}
		// if player is not included try to find a player
		if(!Target)
		{
			PlayerManager player = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));	
			Target = player.gameObject;
		}
	}

	Vector3 positionTargetUp; 
	void FixedUpdate ()
	{
		bool activecheck = false;
		for (int i =0; i<Cameras.Length; i++) 
		{
			if (Cameras [i] && Cameras [i].camera.enabled) 
			{
				activecheck = true;
				break;	
			}
		}
		if (!activecheck) 
		{
			this.camera.enabled = true;
			if (this.gameObject.GetComponent<AudioListener> ())
				this.gameObject.GetComponent<AudioListener> ().enabled = true;
		}

		var enemy = GameObject.FindGameObjectsWithTag("Enemy");

		if(enemy.Length == 0 && TypeAction.type == TypeAction.SURVIVAL && Time.timeScale == 1)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;
			ProgressController.killAdd++;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
			UIController.current.gameObject.SetActive(true);
		}

		if(gameTime != 0f && gameTime < Time.timeSinceLevelLoad && TypeAction.type == TypeAction.FREE_FOR_ALL)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;

			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
			UIController.current.gameObject.SetActive(true);
		}

		if(dieTime != 0f && dieTime < Time.timeSinceLevelLoad && TypeAction.type == TypeAction.SURVIVAL)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;
			
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
			UIController.current.gameObject.SetActive(true);
		}

		if(dieTime != 0f && dieTime < Time.timeSinceLevelLoad && TypeAction.type == TypeAction.FREE_FOR_ALL)
		{	
			Instantiate (PlaneAction.currentPlane.gameObject, positionCamera, quaternionCamera);

			PlayerManager player = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));	
			Target = player.gameObject;

			GameUI menu = (GameUI)GameObject.FindObjectOfType(typeof(GameUI));
			if(menu){
				menu.Mode = 0;	
			}

			dieTime = 0;
		}

		if (!Target)
		{
			if(dieTime == 0f)
				dieTime = Time.timeSinceLevelLoad + 3f;
			return;
		}

		this.transform.LookAt (Target.transform.position + Target.transform.forward * Offset.x);
		positionTargetUp = Vector3.Lerp(positionTargetUp,(-Target.transform.forward + (Target.transform.up * Offset.y)),Time.fixedDeltaTime * TurnSpeedMult);
		Vector3 positionTarget = Target.transform.position + (positionTargetUp * Offset.z);
		float distance = Vector3.Distance (positionTarget, this.transform.position);
		this.transform.position = Vector3.Lerp (this.transform.position, positionTarget, Time.fixedDeltaTime * (distance  * FollowSpeedMult));

		if (Target.CompareTag ("Parashute"))
		{
			if(dieTime == 0f)
				dieTime = Time.timeSinceLevelLoad + 3f;
			return;
		}

		if(transform.position.x < xBattle.x || transform.position.x > xBattle.y || transform.position.z < zBattle.x || transform.position.z > zBattle.y)
		{
			Target.GetComponent<FlightSystem>().enabled = false;

			eject = true;

			Screen.lockCursor = false;
		}
	}
}
