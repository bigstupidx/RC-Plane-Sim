using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour 
{
	private float music;
	private float sound;

	// Use this for initialization
	void OnLevelWasLoaded (int level) 
	{
		Adjust ();
	}

	void OnEnable()
	{
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

	private void Adjust ()
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

	public void ChangeMusic()
	{
		music = UISlider.current.value;

		PlayerPrefs.SetFloat ("Music", music);

		Adjust ();
	}
}
