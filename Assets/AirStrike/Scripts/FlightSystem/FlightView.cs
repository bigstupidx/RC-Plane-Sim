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

	void Awake ()
	{
		// add this camera to primery
		AddCamera(this.gameObject);
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
		if(dieTime != 0f && dieTime < Time.timeSinceLevelLoad)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;

            var enemy = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemy.Length == 0)
                ProgressController.goldAdd += 100 * SwipeAction.levelDifficult;
            else if (enemy.Length == 1)
                ProgressController.goldAdd += 50 * SwipeAction.levelDifficult;
            else if (enemy.Length == 2)
                ProgressController.goldAdd += 25 * SwipeAction.levelDifficult;
            else if (enemy.Length >= 3)
                ProgressController.goldAdd += 10 * SwipeAction.levelDifficult;

            if (UIController.previous != null && UIController.previous != UIController.GetPanel(PanelType.Type.Win))
            {
                UIController.current.gameObject.SetActive(false);
                UIController.previous = UIController.current;
                UIController.current = UIController.GetPanel(PanelType.Type.Win);
                UIController.current.gameObject.SetActive(true);
            }
		}

		if (!Target)
		{
			if(dieTime == 0f)
				dieTime = Time.timeSinceLevelLoad + 3f;
			return;
		}

		// rotation , moving along the player	
		//Quaternion lookAtRotation = Quaternion.LookRotation (Target.transform.position);
		this.transform.LookAt (Target.transform.position + Target.transform.forward * Offset.x);
		positionTargetUp = Vector3.Lerp(positionTargetUp,(-Target.transform.forward + (Target.transform.up * Offset.y)),Time.fixedDeltaTime * TurnSpeedMult);
		Vector3 positionTarget = Target.transform.position + (positionTargetUp * Offset.z);
		float distance = Vector3.Distance (positionTarget, this.transform.position);
		this.transform.position = Vector3.Lerp (this.transform.position, positionTarget, Time.fixedDeltaTime * (distance  * FollowSpeedMult));

		if (Target.CompareTag ("Parashute"))
		{
			return;
		}

		if(transform.position.x < xBattle.y || transform.position.x > xBattle.x || transform.position.z < zBattle.y || transform.position.z > zBattle.x)
		{
			Target.GetComponent<DamageManager>().ApplyDamage(2000, null);
		}
	}

	void Update ()
	{
		
		bool activecheck = false;
		for (int i =0; i<Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i].camera.enabled) {
				activecheck = true;
				break;	
			}
		}
		if (!activecheck) {
			this.camera.enabled = true;
			if (this.gameObject.GetComponent<AudioListener> ())
				this.gameObject.GetComponent<AudioListener> ().enabled = true;
		}
	}
}
