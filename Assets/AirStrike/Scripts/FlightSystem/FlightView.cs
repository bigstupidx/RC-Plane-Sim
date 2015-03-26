/// <summary>
/// Flight view. this script is the Camera Follower
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class FlightView : MonoBehaviour
{
	public static GameObject Target;// player ( Your Plane)
	public GameObject[] Cameras;// all cameras in the game ( in case of able to swith the views).
	public float FollowSpeedMult = 0.5f; // camera following speed 
	public float TurnSpeedMult = 5; // camera turning speed 
	private int indexCamera;// current camera index
	public Vector3 Offset = new Vector3 (10, 0.5f, 10);// position offset between plan and camera
	private float dieTime;
	public float gameTime;
	public static int wave;
	public static bool eject;
	public static ScreenOrientation scrOrient;

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
			if (Cameras [i] && Cameras [i].GetComponent<Camera>())
				Cameras [i].GetComponent<Camera>().enabled = false;
			if (Cameras [i] && Cameras [i].GetComponent<AudioListener> ())
				Cameras [i].GetComponent<AudioListener> ().enabled = false;
		}
		if (Cameras [indexCamera]) {
			if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<Camera>())
				Cameras [indexCamera].GetComponent<Camera>().enabled = true;
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
		positionCamera = transform.position;
		quaternionCamera = transform.rotation;

		Instantiate (PlaneAction.currentPlane.plane.gameObject, positionCamera, quaternionCamera);

		AddCamera(this.gameObject);
		
		wave = 0;
		gameTime = 0;

		eject = false;
		Screen.orientation = ScreenOrientation.AutoRotation;
		
		Time.timeScale = 1;
		UIController.HidePanels();
		UIController.current.gameObject.SetActive(false);
		UIController.previous = UIController.current;
		UIController.current = UIController.GetPanel(PanelType.Type.GameMenu);
		UIController.current.gameObject.SetActive(true);

		AirplanePath[] planes;
		switch(TypeAction.type)
		{
		case TypeAction.FREE_FOR_ALL:
			var enemy = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject go in enemy)
			{
				go.GetComponent<DamageManager>().HP = 60 + 10 * SwipeAction.levelDifficult;
				go.GetComponent<FlightOnHit>().Damage = 5 + 2 * SwipeAction.levelDifficult;
			}

			gameTime = Time.timeSinceLevelLoad + 180;
			break;
		case TypeAction.SURVIVAL:
			planes = GameObject.FindObjectsOfType<AirplanePath>();
			for(int i = 0; i < planes.Length; i++)
			{
				if(planes[i].CompareTag("Train"))
				{
					continue;
				}

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
				waves[i].GetComponent<DamageManager>().level = WaveProps.waveProps[wave, i] % 4;
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
			if(Camera.main.GetComponent<AudioSource>().volume == 0)
				Target.GetComponent<AudioSource>().enabled = true;
		}

		ProgressController.goldAdd = 0;
		ProgressController.ejectAdd = 0;
		ProgressController.expAdd = 0;
		ProgressController.killAdd = 0;
	}

	Vector3 positionTargetUp; 
	void FixedUpdate ()
	{
		if(eject)
		{
			Screen.orientation = scrOrient;
		}

		//NEW
		var go = GameObject.Find ("Colliders");
		if(go != null && Target != null)
		{
			for(int i = 0; i < go.transform.childCount; i++)
			{
				var child = go.transform.GetChild(i);
				float dis;

				if(child.localScale.x == 0)
				{
					dis = Vector2.Distance(new Vector2(0, child.position.z), new Vector2(0, Target.transform.position.z));
				}
				else
				{
					dis = Vector2.Distance(new Vector2(0, child.position.x), new Vector2(0, Target.transform.position.x));
				}


				if(dis < 100 && !eject)
				{
					Debug.Log(child.name);
					GameObject.Find("CrashedLabel").GetComponent<UILabel>().enabled = true;
					GameObject.Find("CrashedLabel").GetComponent<UILabel>().text = "Come back to battle zone or your plane will be crashed " + Mathf.RoundToInt(Mathf.Max(0, dis)) + " meters";
					break;
				}
				else
				{
					GameObject.Find("CrashedLabel").GetComponent<UILabel>().enabled = false;
				}
			}
		}
		//NEW

		bool activecheck = false;
		for (int i = 0; i < Cameras.Length; i++) 
		{
			if (Cameras [i] && Cameras [i].GetComponent<Camera>().enabled) 
			{
				activecheck = true;
				break;	
			}
		}
		if (!activecheck) 
		{
			this.GetComponent<Camera>().enabled = true;
			if (this.gameObject.GetComponent<AudioListener> ())
				this.gameObject.GetComponent<AudioListener> ().enabled = true;
		}

		var enemy = GameObject.FindGameObjectsWithTag("Enemy");

		if(enemy.Length == 0 && TypeAction.type == TypeAction.SURVIVAL && Time.timeScale == 1f && UIController.current.panelType != PanelType.Type.WinSAS && !isWait)
		{
			isWait = true;
			StartCoroutine(StopCurrentWave());
		}

		if(gameTime != 0f && gameTime < Time.timeSinceLevelLoad && TypeAction.type == TypeAction.FREE_FOR_ALL && UIController.current.panelType != PanelType.Type.WinSAS)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;

			ProgressController.expAdd += 100 * SwipeAction.levelDifficult;

			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
			UIController.current.gameObject.SetActive(true);
		}

		if(dieTime != 0f && dieTime < Time.timeSinceLevelLoad && TypeAction.type == TypeAction.SURVIVAL && UIController.current.panelType != PanelType.Type.WinSAS)
		{
			Time.timeScale = 0;
			Screen.lockCursor = false;
			
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
			UIController.current.gameObject.SetActive(true);
		}

		if(dieTime != 0f && dieTime < Time.timeSinceLevelLoad && (TypeAction.type == TypeAction.FREE_FOR_ALL || TypeAction.type == TypeAction.FREE_FLIGHT))
		{	
			Instantiate (PlaneAction.currentPlane.plane.gameObject, positionCamera, quaternionCamera);
            GameObject.Find("Button - Eject").GetComponentInChildren<UISprite>().enabled = false;
            GameObject.Find("Button - Eject").GetComponentInChildren<UILabel>().enabled = false;
            GameObject.Find("Button - Eject").GetComponentInChildren<TweenRotation>().enabled = false;
			eject = false;
			Screen.orientation = ScreenOrientation.AutoRotation;
			PlayerManager player = (PlayerManager)GameObject.FindObjectOfType(typeof(PlayerManager));	
			Target = player.gameObject;
			if(Camera.main.GetComponent<AudioSource>().volume == 0)
				Target.GetComponent<AudioSource>().enabled = true;
			GameUI menu = (GameUI)GameObject.FindObjectOfType(typeof(GameUI));
			if(menu){
				menu.Mode = 0;	
			}

			dieTime = 0;
		}

		if (!Target)
		{
			if(dieTime == 0f)
			{
				dieTime = Time.timeSinceLevelLoad + 3f;
			}
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
	}

	public IEnumerator StartNextWave()
	{
		yield return StartCoroutine (WaitForRealTimePlay(3f));
	}

	private IEnumerator StopCurrentWave()
	{
		yield return StartCoroutine (WaitForRealTimeStop(2f));

		isWait = false;

		Time.timeScale = 0;
		Screen.lockCursor = false;
		ProgressController.killAdd++;
		ProgressController.expAdd += 50 * ProgressController.killAdd;
		UIController.current.gameObject.SetActive(false);
		UIController.previous = UIController.current;
		UIController.current = UIController.GetPanel(PanelType.Type.WinSAS);
		UIController.current.gameObject.SetActive(true);
	}

	public static bool isWait;
	public static IEnumerator WaitForRealTimePlay(float delay)
	{
		while(true)
		{
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			float startEndTime = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < pauseEndTime)
			{
				Time.timeScale = (Time.realtimeSinceStartup - startEndTime) / delay;
				yield return 0;
			}
			break;
		}

		isWait = false;

		Time.timeScale = 1f;
		
		wave = Mathf.Min(73, wave + 1);
		var planes = GameObject.FindObjectsOfType<AirplanePath>();
		for(int i = 0; i < planes.Length; i++)
		{
			if(planes[i].plane.name.Contains("Subway"))
			{
				continue;
			}
			
			if(WaveProps.waveProps[FlightView.wave, i] != 0)
			{
				planes[i].plane.gameObject.SetActive(true);
				planes[i].speed = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 2] * 3;
			}
		}
		var waves = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < waves.Length; i++)
		{
			waves[i].GetComponent<DamageManager>().HP = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 1];
			waves[i].GetComponent<DamageManager>().level = WaveProps.waveProps[FlightView.wave, i] % 4;
			waves[i].GetComponent<FlightOnHit>().Damage = WaveProps.waveStats[WaveProps.waveProps[FlightView.wave, i], 0];
		}
	}

	public static IEnumerator WaitForRealTimeStop(float delay)
	{
		while(true)
		{
			float pauseEndTime = Time.realtimeSinceStartup + delay;
			float startEndTime = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < pauseEndTime)
			{
				Time.timeScale = pauseEndTime - Time.realtimeSinceStartup;
				yield return 0;
			}
			break;
		}
	}
}
