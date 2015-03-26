using UnityEngine;
using System.Collections;

public class PlaneStatAdjustment : MonoBehaviour 
{
	public WeaponLauncher minigun;
	public WeaponLauncher rocket;

	public Material[] materials;

	public MeshRenderer render;
	public string desc;

	private float currentPosition;
	private float previousPosition;
	private float deltaPosition;
	private RaycastHit hit;

	public void Adjust()
	{
		var armour = PlaneAction.FindStatType (PlaneAction.Stat.Type.Armour);
		GetComponent<DamageManager> ().HP = (int)armour.levels [armour.currentLevel].value;

		var spd = PlaneAction.FindStatType (PlaneAction.Stat.Type.Speed);
		GetComponent<FlightSystem>().Speed = spd.levels [spd.currentLevel].value;
		GetComponent<FlightSystem>().SpeedMax = spd.levels [spd.currentLevel].value;

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

		MoverMissile mm = rocket.Missile.GetComponent<MoverMissile> ();
		if(mm == null)
		{
			rocket.Missile.GetComponent<MoverBullet> ().Lifetime = range.levels [range.currentLevel].value / 200f;
		}
		else
		{
			mm.LifeTime = Mathf.CeilToInt(range.levels [range.currentLevel].value / 200f);
		}

		render.sharedMaterial = materials [PlaneAction.currentPlane.material];
		if(PlaneAction.planeModel != null)
			PlaneAction.planeModel.GetComponent<PlaneStatAdjustment>().render.sharedMaterial = materials [PlaneAction.currentPlane.material];
	}

	void Update()
	{
		if (Application.loadedLevel != 2)
			return;

		if(Input.GetMouseButtonDown(0))
		{
			currentPosition = Input.mousePosition.x;
			previousPosition = Input.mousePosition.x;
		}
		if(Input.GetMouseButton(0) && Input.mousePosition.x < Screen.width * 0.7f && Input.mousePosition.y > Screen.height * 0.2f && Input.mousePosition.y < Screen.height * 0.8f && UIController.current.panelType != PanelType.Type.Map)
		{
			currentPosition = Input.mousePosition.x;
			
			deltaPosition = (currentPosition - previousPosition) * 0.4f;
			
			PlaneAction.planeModel.transform.localEulerAngles = new Vector3(0, PlaneAction.planeModel.transform.localEulerAngles.y - deltaPosition, 0);
			
			previousPosition = Input.mousePosition.x;
		}
		else
		{
			PlaneAction.planeModel.transform.localEulerAngles = new Vector3 (0, PlaneAction.planeModel.transform.localEulerAngles.y + 0.3f, 0);
		}
	}
}
