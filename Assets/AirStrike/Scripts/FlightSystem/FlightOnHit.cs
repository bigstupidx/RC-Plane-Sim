/// <summary>
/// Flight on hit. this script will take damage to the plane. If got hit by an object that are Taged in the list.
/// </summary>
using UnityEngine;
using System.Collections;

public class FlightOnHit : MonoBehaviour 
{
	public string[] Tag = new string[2]{"Scene", "SceneLimit"};// All scene object tag.
	public string AirportTag = "Airport";// air port tag.
	public int Damage = 100;
	public AudioClip[] SoundOnHit;

	void Awake()
	{
		if(Camera.main.GetComponent<CC_AnalogTV>() != null)
			Camera.main.GetComponent<CC_AnalogTV>().enabled = false;
	}

    private void OnCollisionEnter(Collision collision)
    {
		bool hit = false;
		bool off = false;

		for(int i = 0; i < Tag.Length; i++)
		{
			if(collision.gameObject.tag == Tag[i])
			{
				if(collision.gameObject.tag == "SceneLimit")
				{
					off = true;
				}

				hit = true;
			}
		}
		
        if (hit)
        {
			if(SoundOnHit.Length > 0)
				AudioSource.PlayClipAtPoint(SoundOnHit[Random.Range(0,SoundOnHit.Length)],this.transform.position);

			if(this.GetComponent<DamageManager>() && !off)
			{
				this.GetComponent<DamageManager>().Boom(Damage, collision.gameObject);
			}
			else
			{
				FlightView.Target.GetComponent<FlightSystem>().enabled = false;
				Camera.main.GetComponent<CC_AnalogTV>().enabled = true;
				FlightView.eject = true;
				Screen.lockCursor = false;
			}
		}	
    }
}
