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
                GameObject.Find("Button - Eject").GetComponentInChildren<UISprite>().enabled = true;
                GameObject.Find("Button - Eject").GetComponentInChildren<UILabel>().enabled = true;
                GameObject.Find("Button - Eject").GetComponentInChildren<TweenRotation>().enabled = true;
				FlightView.eject = true;
				Screen.lockCursor = false;
				FlightView.scrOrient = Screen.orientation;
			}
		}	
    }
}
