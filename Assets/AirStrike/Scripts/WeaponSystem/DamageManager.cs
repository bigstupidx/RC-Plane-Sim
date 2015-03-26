/// <summary>
/// Damage manager. 
/// </summary>
using UnityEngine;
using System.Collections;

public class DamageManager : MonoBehaviour
{
    public AudioClip[] HitSound;
    public GameObject Effect;
    public float HP = 100;
	public int level;
	public float HPmax;
	public ParticleSystem OnFireParticle;
	
    private void Start()
    {
		HPmax = HP;

		if(OnFireParticle){
			OnFireParticle.Stop();
		}
    }
	// Damage function
    public void ApplyDamage(int damage, GameObject killer)
    {
		if(HP < 0)
			return;
	
        if (HitSound.Length > 0)
        {
            AudioSource.PlayClipAtPoint(HitSound[Random.Range(0, HitSound.Length)], transform.position);
        }
        HP -= damage;
		if(OnFireParticle)
		{
			if(HP < (int)(HPmax / 2.0f))
			{
				OnFireParticle.Play();
			}
		}
        if (HP <= 0)
        {
			if(this.gameObject.GetComponent<FlightOnDead>())
			{
				this.gameObject.GetComponent<FlightOnDead>().OnDead(killer);
			}

            Dead(killer);
        }
    }

	public void Boom(int damage, GameObject killer)
	{
		if (Effect)
		{
			GameObject obj = (GameObject)GameObject.Instantiate(Effect, transform.position, transform.rotation);
			if(this.GetComponent<Rigidbody>()){
				if(obj.GetComponent<Rigidbody>()){
					obj.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
					obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
				}
			}
		}

		Destroy(this.gameObject);
	}

	private void Dead(GameObject killer)
    {
		if(gameObject.CompareTag("Enemy"))
		{
			if (Effect)
			{
				GameObject obj = (GameObject)GameObject.Instantiate(Effect, transform.position, transform.rotation);
				if(this.GetComponent<Rigidbody>()){
					if(obj.GetComponent<Rigidbody>()){
						obj.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
						obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
					}
				}
			}

			var aishoot = transform.GetComponentInParent<AirplanePath>();

			transform.parent.gameObject.SetActive(false);

			if(!killer.CompareTag("Player"))
			{
				if(aishoot && TypeAction.type == TypeAction.FREE_FOR_ALL)
				{
					aishoot.StartRespawn();
				}
				return;
			}

			if(aishoot && TypeAction.type == TypeAction.FREE_FOR_ALL)
			{
				ProgressController.goldAdd += 10 * SwipeAction.levelDifficult;
				ProgressController.killAdd++;
				aishoot.StartRespawn();
			}
			else
			{
				ProgressController.goldAdd += 10 * level;
			}
		}
		else if(gameObject.CompareTag("Player"))
		{
			ProgressController.scoreDeath[ProgressController.level, SwipeAction.levelDifficult - 1]++;
			FlightView.Target.GetComponent<FlightSystem>().enabled = false;
            GameObject.Find("Button - Eject").GetComponentInChildren<UISprite>().enabled = true;
            GameObject.Find("Button - Eject").GetComponentInChildren<UILabel>().enabled = true;
            GameObject.Find("Button - Eject").GetComponentInChildren<TweenRotation>().enabled = true;
			FlightView.eject = true;
			FlightView.scrOrient = Screen.orientation;
		}
    }

}
