using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour 
{
	public float music;
	public float sound;
	public bool first;

	public AudioSource _click;
	public static AudioSource click;

	public static VolumeController instance;

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

		if(instance == null)
		{
			instance = this;
		}

		first = PlayerPrefs.GetInt ("First") == 0;

		if(first)
		{
			PlayerPrefs.SetFloat ("Sound", 1f);
			PlayerPrefs.SetFloat ("Music", 1f);
			PlayerPrefs.SetInt("First", 1);
		}

		Adjust ();

		if(name == "Slider - Sound")
		{
			GetComponent<UISlider>().value = instance.sound;
		}
		else if(name == "Slider - Music")
		{
			GetComponent<UISlider>().value = instance.music;
		}
	}

	public void Adjust ()
	{
		instance.sound = PlayerPrefs.GetFloat ("Sound");
		instance.music = PlayerPrefs.GetFloat ("Music");
		
		var audio = FindObjectsOfType<AudioSource> ();
		
		foreach(AudioSource a in audio)
		{
			if(a.tag == "MainCamera")
			{
				a.volume = instance.music;
			}
			else
			{
				a.volume = instance.sound;
			}
		}
	}
	
	public void ChangeSound()
	{
		instance.sound = UISlider.current.value;
		
		PlayerPrefs.SetFloat ("Sound", instance.sound);

		Adjust ();
	}

	public void ChangeSound(float value)
	{
		instance.sound = value;
		
		PlayerPrefs.SetFloat ("Sound", instance.sound);
		
		Adjust ();
	}

	public void ChangeMusic()
	{
		instance.music = UISlider.current.value;

		PlayerPrefs.SetFloat ("Music", instance.music);

		Adjust ();
	}

	public void ChangeMusic(float value)
	{
		instance.music = value;
		
		PlayerPrefs.SetFloat ("Music", instance.music);
		
		Adjust ();
	}
}
