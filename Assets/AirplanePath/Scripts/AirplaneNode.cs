using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AirplaneNode : MonoBehaviour 
{
	public Vector3 Position {get{return transform.position;}}
	public float tangentStrength = 1;
	public float tangentAngleOffset = 0;
	[HideInInspector]
	public Vector3[] controlPoints;
	[HideInInspector]
	public AirplanePath path;
	
	void OnDestroy()
	{
		if(path != null)
		{
			path.OnDestroyedNode(this);
		}
	}
}

