﻿using UnityEngine;
using System.Collections;

public class DamageBase : MonoBehaviour {

	public GameObject Effect;
	public float LifeTimeEffect = 3;
	[HideInInspector]
    public GameObject Owner;
    public int Damage = 20;
	public string[] TargetTag;
}

public class WeaponBase : MonoBehaviour {
	[HideInInspector]
    public GameObject Owner;
	[HideInInspector]
	public GameObject Target;
    public string[] TargetTag;
	public bool RigidbodyProjectile;
	public Vector3 TorqueSpeedAxis;
	public GameObject TorqueObject;
}

