using UnityEngine;
using System.Collections;

public class TiltSensativity : MonoBehaviour 
{
	public static float sensativity;

	public void SetTilt()
	{
		sensativity = 2f + UISlider.current.value * 6f;
	}
}
