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
	//private int indexSpawn = 0;
    private int numberWave = 1;
    private int numberEliteEnemy = 0;
	
	void Start ()
	{
		//indexSpawn = Random.Range (0, Objectman.Length);
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
            numberWave += 1;
            enemyCount += 2 * numberWave;
            numberEliteEnemy = 2 * numberWave;

            ProgressController.expAdd += 50 * numberWave;
			Enabled = true;
		}
		else if(gos.Length == 0 && !Enabled && TypeAction.type == 0)
		{
            // Free for all
			Screen.lockCursor = false;
			Time.timeScale = 0;
			ProgressController.goldAdd += 100 * SwipeAction.levelDifficult;
            Debug.Log(ProgressController.goldAdd.ToString());
            if (UIController.previous != null && UIController.previous != UIController.GetPanel(PanelType.Type.Win))
            {
                UIController.current.gameObject.SetActive(false);
                UIController.previous = UIController.current;
                UIController.current = UIController.GetPanel(PanelType.Type.Win);
                UIController.current.gameObject.SetActive(true);   
            }
		}

		if (!Enabled)
			return;

		if (gos.Length < enemyCount && Time.time > timetemp + timeSpawn) 
		{
			timetemp = Time.time;

            if (numberWave > 1 && numberEliteEnemy > 0)
            {
                numberEliteEnemy -= 1;

                GameObject obj = (GameObject)GameObject.Instantiate(Objectman[0], transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), Quaternion.identity);
                obj.GetComponent<DamageManager>().HP += 10 * numberWave;
                obj.GetComponent<WeaponController>().WeaponLists[0].GetComponent<WeaponLauncher>().Missile.GetComponent<Damage>().Damage += 2 * numberWave;
                obj.tag = Tag;
            }
            else 
            {
                GameObject obj = (GameObject)GameObject.Instantiate(Objectman[0], transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), Quaternion.identity);
                obj.tag = Tag;
            }
		}

		if(gos.Length == enemyCount)
		{
            Enabled = false;
		}
	}
}
