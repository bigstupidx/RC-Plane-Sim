/// <summary>
/// Player controller.
/// </summary>
using UnityEngine;
using System.Collections;
[RequireComponent(typeof(FlightSystem))]

public class PlayerController : MonoBehaviour {
	
	FlightSystem flight;// Core plane system
	FlightView View;
	
	public bool Active = true;
	public bool SimpleControl;// make it easy to control Plane will turning easier.
	public bool Acceleration;// Mobile*** enabled gyroscope controller
	public float AccelerationSensitivity = 5;// Mobile*** gyroscope sensitivity
	private TouchScreenVal controllerTouch;// Mobile*** move
	private TouchScreenVal fireTouch;// Mobile*** fire
	private TouchScreenVal switchTouch;// Mobile*** swich
	private TouchScreenVal sliceTouch;// Mobile*** slice
	private bool directVelBack;
	public GUISkin skin;
	public bool ShowHowto;
	void Start () 
	{
		flight = this.GetComponent<FlightSystem>();
		View = (FlightView)GameObject.FindObjectOfType(typeof(FlightView));
		// setting all Touch screen controller in the position
		controllerTouch = new TouchScreenVal (new Rect (0, 0, Screen.width / 4, Screen.height / 3));
		fireTouch = new TouchScreenVal (new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height));
		switchTouch = new TouchScreenVal (new Rect (0, Screen.height - 100, Screen.width / 2, 100));
		
		sliceTouch = new TouchScreenVal (new Rect (0, 0, Screen.width / 2, 50));
		
		if(flight)
		directVelBack = flight.DirectVelocity;
	}
	
	void Update () 
	{
		if(!flight || !Active)
			return;
		SimpleControl = true;
		Acceleration = UIController.acceleration;
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		// On Desktop
		DesktopController();
		#else
		// On Mobile device
		MobileController();
		#endif
		
	}
	
	
	void DesktopController()
	{
		// Desktop controller
		flight.SimpleControl = SimpleControl;
		
		// lock mouse position to the center.
		//Screen.lockCursor = true;
		flight.AxisControl(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") ));
		if(SimpleControl)
		{
			flight.DirectVelocity = false;
			flight.TurnControl(Input.GetAxis("Mouse X"));
		}
		else
		{
			flight.DirectVelocity = directVelBack;	
		}

		flight.TurnControl(Input.GetAxis ("Horizontal"));
		flight.SpeedUp(Input.GetAxis ("Vertical"));
		
		
		if(Input.GetButton("Fire1"))
		{
            flight.WeaponControl.LaunchWeapon();
        }
		
		if(Input.GetButtonDown("Fire2"))
		{
            flight.WeaponControl.SwitchWeapon();
        }
		
       	if (Input.GetKeyDown (KeyCode.C)) 
		{
			if(View)
				View.SwitchCameras ();	
		}	
	}
	
	
	void MobileController()
	{
		// Mobile controller
		
		flight.SimpleControl = SimpleControl;
		
		if (Acceleration) 
		{
			// get axis control from device acceleration
			Vector3 acceleration = Input.acceleration;
			Vector2 accValActive = new Vector2 (acceleration.x, acceleration.normalized.y);
			flight.FixedX = false;
			flight.FixedY = false;
			flight.FixedZ = true;
			flight.AxisControlTilt (accValActive);
			flight.TurnControlTilt (accValActive.x * 2f);
		} 
		else 
		{
			flight.FixedX = true;
			flight.FixedY = false;
			flight.FixedZ = true;
			// get axis control from touch screen
			Vector2 dir = controllerTouch.OnDragDirection (true);
			dir = Vector2.ClampMagnitude(dir, 1.0f);
			flight.AxisControl (new Vector2 (dir.x, -dir.y) * 10f);
			flight.TurnControl (dir.x * 3f);
		}
		sliceTouch.OnDragDirection(true);
		// slice speed
		flight.SpeedUp(sliceTouch.slideVal.x);
	}
}
