using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]

public class WeaponLauncher : WeaponBase
{
	public bool OnActive;
	public Texture2D Icon;
	public Transform[] MissileOuter;
	public GameObject Missile;
	public float FireRate = 0.1f;
	public float Spread = 1;
	public float ForceShoot = 8000;
	public int NumBullet = 1;
	public int Ammo = 10;
	public float AmmoMax = 10;
	public bool InfinityAmmo = false;
	public float ReloadTime = 1;
	public bool ShowHUD = true;
	public int MaxAimRange = 10000;
	public bool ShowCrosshair;
	public Texture2D CrosshairTexture;
	public Texture2D TargetLockOnTexture;
	public Texture2D TargetLockedTexture;
	public float DistanceLock = 200;
	public float TimeToLock = 2;
	public float AimDirection = 0.8f;
	public bool Seeker;
	public GameObject Shell;
	public float ShellLifeTime = 4;
	public Transform[] ShellOuter;
	public int ShellOutForce = 300;
	public GameObject Muzzle;
	public float MuzzleLifeTime = 2;
	public AudioClip[] SoundGun;
	public AudioClip SoundReloading;
	public AudioClip SoundReloaded;
	private float timetolockcount = 0;
	private float nextFireTime = 0;
	private GameObject target;
	private Vector3 torqueTemp;
	private float reloadTimeTemp;
	private AudioSource audio;
	[HideInInspector]
	public bool Reloading;
	[HideInInspector]
	public float ReloadingProcess;
	
	private void Start ()
	{
		if (!Owner) 
			Owner = this.transform.root.gameObject;
		
		if (!audio) {
			audio = this.GetComponent<AudioSource> ();
			if (!audio) {
				this.gameObject.AddComponent<AudioSource> ();	
			}
		}

		CurrentCamera = Camera.main;
	}

	[HideInInspector]
	public Vector3 AimPoint;
	[HideInInspector]
	public GameObject AimObject;

	private void rayAiming ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, this.transform.forward, out hit, MaxAimRange)) 
		{
			if (Missile != null && hit.collider.tag != Missile.tag) 
			{
				AimPoint = hit.point;
				AimObject = hit.collider.gameObject;
			}
		} 
		else 
		{
			AimPoint = this.transform.position + (this.transform.forward * MaxAimRange);
			AimObject = null;
		}
		
	}
	
	void FixedUpdate ()
	{
		if (OnActive) {
			rayAiming ();
		}
	}
	
	private void Update ()
	{
		if (OnActive) 
		{
			if (TorqueObject) 
			{
				TorqueObject.transform.Rotate (torqueTemp * Time.deltaTime);
				torqueTemp = Vector3.Lerp (torqueTemp, Vector3.zero, Time.deltaTime);
			}
			if (Seeker) 
			{
				for (int t=0; t<TargetTag.Length; t++) 
				{
					if (GameObject.FindGameObjectsWithTag (TargetTag [t]).Length > 0) 
					{
						GameObject[] objs = GameObject.FindGameObjectsWithTag (TargetTag [t]);
						float distance = int.MaxValue;
						
						if (AimObject != null && AimObject.tag == TargetTag [t]) 
						{
							float dis = Vector3.Distance (AimObject.transform.position, transform.position);
							if (DistanceLock > dis) 
							{
								if (distance > dis) 
								{
									if (timetolockcount + TimeToLock < Time.time) 
									{	
										distance = dis;
										target = AimObject;
									}
								}
							}	
						} 
						else 
						{
							for (int i = 0; i < objs.Length; i++) 
							{
								if (objs [i]) 
								{
									Vector3 dir = (objs [i].transform.position - transform.position).normalized;
									float direction = Vector3.Dot (dir, transform.forward);
									float dis = Vector3.Distance (objs [i].transform.position, transform.position);
									if (direction >= AimDirection) 
									{
										if (DistanceLock > dis) 
										{
											if (distance > dis) 
											{
												if (timetolockcount + TimeToLock < Time.time) 
												{	
													distance = dis;
													target = objs [i];
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (target) {
				float targetdistance = Vector3.Distance (transform.position, target.transform.position);
				Vector3 dir = (target.transform.position - transform.position).normalized;
				float direction = Vector3.Dot (dir, transform.forward);

				if (targetdistance > DistanceLock || direction <= AimDirection) {
					Unlock ();
				}
			}
		
			if (Reloading) {
				ReloadingProcess = ((1 / ReloadTime) * (reloadTimeTemp + ReloadTime - Time.time));
				if (Time.time >= reloadTimeTemp + ReloadTime) {
					Reloading = false;
					if (SoundReloaded) {
						if (audio) {
							audio.PlayOneShot (SoundReloaded);
						}
					}
					Ammo = (int)AmmoMax;
				}
			} else {
				if (Ammo <= 0) {
					Unlock ();
					Reloading = true;
					reloadTimeTemp = Time.time;
				
					if (SoundReloading) {
						if (audio) {
							audio.PlayOneShot (SoundReloading);
						}
					}
				}
			}
		}
	}
	public Camera CurrentCamera;
	private void DrawTargetLockon (Transform aimtarget, bool locked)
	{
		if (!ShowHUD)
			return;
		
		if (CurrentCamera) {
			Vector3 dir = (aimtarget.position - CurrentCamera.transform.position).normalized;
			float direction = Vector3.Dot (dir, CurrentCamera.transform.forward);
			if (direction > 0.5f) {
				Vector3 screenPos = CurrentCamera.WorldToScreenPoint (aimtarget.transform.position);
				float distance = Vector3.Distance (transform.position, aimtarget.transform.position);
				if (locked) {
					if (TargetLockedTexture)
						GUI.DrawTexture (new Rect (screenPos.x - (TargetLockedTexture.width / 2), Screen.height - screenPos.y - (TargetLockedTexture.height / 2), TargetLockedTexture.width, TargetLockedTexture.height), TargetLockedTexture);
					GUI.Label (new Rect (screenPos.x + 40, Screen.height - screenPos.y, 200, 30), aimtarget.name + " " + Mathf.Floor (distance) + "m.");
				} else {
					if (TargetLockOnTexture)
						GUI.DrawTexture (new Rect (screenPos.x - TargetLockOnTexture.width / 2, Screen.height - screenPos.y - TargetLockOnTexture.height / 2, TargetLockOnTexture.width, TargetLockOnTexture.height), TargetLockOnTexture);
				}
				
            	
			}
		} else {
			//Debug.Log("Can't Find camera");
		}
	}

	private Vector3 crosshairPos;

	private void DrawCrosshair ()
	{
		if(!ShowCrosshair)
			return;
		
		if (CurrentCamera) 
		{
			Vector3 screenPosAim = CurrentCamera.WorldToScreenPoint (AimPoint);
			crosshairPos += ((screenPosAim - crosshairPos) / 5);
			if (CrosshairTexture) 
			{
				Rect position = new Rect (crosshairPos.x - (Screen.width * 0.03f / 2) / 2, Screen.height - crosshairPos.y - (Screen.width * 0.03f / 2), Screen.width * 0.03f, Screen.width * 0.03f);
				GUI.DrawTexture (position, CrosshairTexture);
			}
		}
	}

	private void OnGUI ()
	{
		if (OnActive) 
		{
			if (Seeker) 
			{
           
				if (target) 
				{
					DrawTargetLockon (target.transform, true);
				}
            
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
								if (direction >= AimDirection) 
								{
									float dis = Vector3.Distance (objs [i].transform.position, transform.position);
									if (DistanceLock > dis) 
									{
										DrawTargetLockon (objs [i].transform, false);
									}
								}
							}
						}
					}
				}
			}
			DrawCrosshair ();
		}
		
	}

	private void Unlock ()
	{
		timetolockcount = Time.time;
		target = null;
	}
	
	private int currentOuter = 0;

	public void Shoot (int damage = 5)
	{
		if (InfinityAmmo) 
		{
			Ammo = 1;	
		}
		if (Ammo > 0) 
		{
			if (Time.time > nextFireTime + FireRate) 
			{
				nextFireTime = Time.time;
				torqueTemp = TorqueSpeedAxis;
				Ammo -= 1;
				Vector3 missileposition = this.transform.position;
				Quaternion missilerotate = this.transform.rotation;
				if (MissileOuter.Length > 0) 
				{
					missilerotate = MissileOuter [currentOuter].transform.rotation;	
					missileposition = MissileOuter [currentOuter].transform.position;	
				}

				if (MissileOuter.Length > 0) 
				{
					currentOuter += 1;
					if (currentOuter >= MissileOuter.Length)
						currentOuter = 0;
				}
			
				if (Muzzle) 
				{
					GameObject muzzle = (GameObject)GameObject.Instantiate (Muzzle, missileposition, missilerotate);
					muzzle.transform.parent = this.transform;
					GameObject.Destroy (muzzle, MuzzleLifeTime);
					if (MissileOuter.Length > 0) 
					{
						muzzle.transform.parent = MissileOuter [currentOuter].transform;
					}
				}
			
				for (int i = 0; i < NumBullet; i++) 
				{
					if (Missile) 
					{
						Vector3 spread = new Vector3 (Random.Range (-Spread, Spread), Random.Range (-Spread, Spread), Random.Range (-Spread, Spread)) / 100;
						Vector3 direction = this.transform.forward + spread;
					
					
					
						GameObject bullet = (GameObject)Instantiate (Missile, missileposition, missilerotate);
						DamageBase damangeBase = bullet.GetComponent<DamageBase> ();
						if (damangeBase) 
						{
							if(Owner.CompareTag("Enemy"))
							{
								damangeBase.Damage = damage * SwipeAction.levelDifficult;
							}

							damangeBase.Owner = Owner;
							damangeBase.TargetTag = TargetTag;
						}
						WeaponBase weaponBase = bullet.GetComponent<WeaponBase> ();
						if (weaponBase) 
						{
							weaponBase.Owner = Owner;
							weaponBase.Target = target;
							weaponBase.TargetTag = TargetTag;
						}
						bullet.transform.forward = direction;
						if (RigidbodyProjectile) 
						{
							if (bullet.rigidbody) 
							{
								if (Owner != null && Owner.rigidbody) 
								{
									bullet.rigidbody.velocity = Owner.rigidbody.velocity;
								}
								bullet.rigidbody.AddForce (direction * ForceShoot);	
							}
						}
					
					}
				}
				if (Shell) 
				{
					Transform shelloutpos = this.transform;
					if (ShellOuter.Length > 0) 
					{
						shelloutpos = ShellOuter [currentOuter];
					}
				
					GameObject shell = (GameObject)Instantiate (Shell, shelloutpos.position, Random.rotation);
					GameObject.Destroy (shell.gameObject, ShellLifeTime);
					if (shell.rigidbody) 
					{
						shell.rigidbody.AddForce (shelloutpos.forward * ShellOutForce);
					}
				}
				
				if (SoundGun.Length > 0) 
				{
					if (audio) 
					{
						audio.PlayOneShot (SoundGun [Random.Range (0, SoundGun.Length)]);
					}
				}
			
				nextFireTime += FireRate;
			}
		} 
	}
}


