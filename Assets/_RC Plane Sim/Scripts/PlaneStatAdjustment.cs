using UnityEngine;
using System.Collections;

public class PlaneStatAdjustment : MonoBehaviour 
{
	public WeaponLauncher minigun;
	public WeaponLauncher rocket;

	public Material[] materials;

	public MeshRenderer render;

	public void Adjust()
	{
		var armour = PlaneAction.FindStatType (PlaneAction.Stat.Type.Armour);
		GetComponent<DamageManager> ().HP = (int)armour.levels [armour.currentLevel].value;

		var mgun = PlaneAction.FindStatType (PlaneAction.Stat.Type.Machinegun);
		var mtime = PlaneAction.FindStatType (PlaneAction.Stat.Type.GunFireTime);
		var mheat = PlaneAction.FindStatType (PlaneAction.Stat.Type.GunHeatTime);
		minigun.Missile.GetComponent<Damage> ().Damage = (int)mgun.levels [mgun.currentLevel].value;
		minigun.ReloadTime = mtime.levels [mgun.currentLevel].value;
		minigun.AmmoMax = mheat.levels [mgun.currentLevel].value;

		var rgun = PlaneAction.FindStatType (PlaneAction.Stat.Type.Rockets);
		var rreload = PlaneAction.FindStatType (PlaneAction.Stat.Type.RocketsReload);
		rocket.Missile.GetComponent<Damage> ().Damage = (int)rgun.levels [rgun.currentLevel].value;
		rocket.ReloadTime = rreload.levels [rgun.currentLevel].value;

		var range = PlaneAction.FindStatType (PlaneAction.Stat.Type.Range);
		minigun.Missile.GetComponent<MoverBullet> ().Lifetime = range.levels [range.currentLevel].value / 200f;
		rocket.Missile.GetComponent<MoverMissile> ().LifeTime = Mathf.CeilToInt(range.levels [range.currentLevel].value / 200f);

		render.sharedMaterial = materials [PlaneAction.currentMaterial];
		if(PlaneAction.planeModel != null)
			PlaneAction.planeModel.GetComponent<PlaneStatAdjustment>().render.sharedMaterial = materials [PlaneAction.currentMaterial];
	}

	void Update()
	{
		if (Application.loadedLevel != 2)
						return;

		PlaneAction.planeModel.transform.localEulerAngles = new Vector3 (0, PlaneAction.planeModel.transform.localEulerAngles.y + 0.3f, 0);
	}
}
