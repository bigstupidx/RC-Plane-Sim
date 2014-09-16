/// <summary>
/// Enemy spawner. auto Re-Spawning an Enemy by Random index of Objectman[]
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	
	public bool Enabled = true;
	public GameObject[] Objectman; // object to spawn
	public float timeSpawn = 3;
	public int enemyCount = 10;
	public int radius = 10;
	public string Tag = "Enemy";
	public string Type = "Enemy";
	private float timetemp = 0;
	private int indexSpawn;
	
	void Start ()
	{
		indexSpawn = Random.Range (0, Objectman.Length);
		timetemp = Time.time;

		if(TypeAction.type == 2)
		{
			Enabled = false;
		}
	}

	void Update ()
	{
		var gos = GameObject.FindGameObjectsWithTag (Tag);

		if(gos.Length == 0 && !Enabled && TypeAction.type == 1)
		{
			Enabled = true;
		}
		else if(gos.Length == 0 && !Enabled && TypeAction.type == 0)
		{
			Screen.lockCursor = false;
			Time.timeScale = 0;
			UIController.exp += 300;
			UIController.current.gameObject.SetActive(false);
			UIController.previous = UIController.current;
			UIController.current = UIController.GetPanel(PanelType.Type.Win);
			UIController.current.gameObject.SetActive(true);
		}

		if (!Enabled)
			return;

		if (gos.Length < enemyCount && Time.time > timetemp + timeSpawn) 
		{
			// spawing an enemys by random index of Objectman[]
			timetemp = Time.time;
			GameObject obj = (GameObject)GameObject.Instantiate (Objectman [indexSpawn], transform.position + new Vector3 (Random.Range (-radius, radius), 0, Random.Range (-radius, radius)), Quaternion.identity);
			obj.tag = Tag;
			indexSpawn = Random.Range (0, Objectman.Length);
		}

		if(gos.Length == enemyCount)
		{
			Enabled = false;
		}
	}
}
