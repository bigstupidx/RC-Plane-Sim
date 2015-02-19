using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour 
{
	public float music;
	public float sound;

	public AudioSource _click;
	public static AudioSource click;

	// Use this for initialization
	void OnLevelWasLoaded (int level) 
	{
		Adjust ();
	}

	void OnEnable()
	{
		if(_click != null)
		{
			click = _click;
		}

		sound = PlayerPrefs.GetFloat ("Sound");
		music = PlayerPrefs.GetFloat ("Music");

		if(name == "Slider - Sound")
		{
			GetComponent<UISlider>().value = sound;
		}
		else if(name == "Slider - Music")
		{
			GetComponent<UISlider>().value = music;
		}
	}

	public void Adjust ()
	{
		sound = PlayerPrefs.GetFloat ("Sound");
		music = PlayerPrefs.GetFloat ("Music");
		
		var audio = FindObjectsOfType<AudioSource> ();
		
		foreach(AudioSource a in audio)
		{
			if(a.tag == "MainCamera")
			{
				a.volume = music;
			}
			else
			{
				a.volume = sound;
			}
		}
	}
	
	public void ChangeSound()
	{
		sound = UISlider.current.value;
		
		PlayerPrefs.SetFloat ("Sound", sound);

		Adjust ();
	}

	public void ChangeSound(float value)
	{
		sound = value;
		
		PlayerPrefs.SetFloat ("Sound", sound);
		
		Adjust ();
	}

	public void ChangeMusic()
	{
		music = UISlider.current.value;

		PlayerPrefs.SetFloat ("Music", music);

		Adjust ();
	}

	public void ChangeMusic(float value)
	{
		music = value;
		
		PlayerPrefs.SetFloat ("Music", music);
		
		Adjust ();
	}
}
