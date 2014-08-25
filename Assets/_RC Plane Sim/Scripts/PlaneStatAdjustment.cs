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
		GetComponent<DamageManager> ().HP = armour.levels [armour.currentLevel].value;

		var mgun = PlaneAction.FindStatType (PlaneAction.Stat.Type.Machinegun);
		minigun.Missile.GetComponent<Damage> ().Damage = mgun.levels [mgun.currentLevel].value;

		var rgun = PlaneAction.FindStatType (PlaneAction.Stat.Type.Rockets);
		minigun.Missile.GetComponent<Damage> ().Damage = rgun.levels [rgun.currentLevel].value;

		var range = PlaneAction.FindStatType (PlaneAction.Stat.Type.Range);
		minigun.Missile.GetComponent<MoverBullet> ().Lifetime = range.levels [range.currentLevel].value / 200;
		rocket.Missile.GetComponent<MoverMissile> ().LifeTime = range.levels [range.currentLevel].value / 200;

		render.sharedMaterial = materials [PlaneAction.currentMaterial];
		if(PlaneAction.planeModel != null)
			PlaneAction.planeModel.GetComponent<PlaneStatAdjustment>().render.sharedMaterial = materials [PlaneAction.currentMaterial];
	}
}
