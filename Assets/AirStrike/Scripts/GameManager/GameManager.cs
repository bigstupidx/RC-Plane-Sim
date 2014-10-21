using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// basic game score
	public int Score = 0;
	public int Killed = 0;

	void Start () 
	{
		Score = 0;
		Killed = 0;

        Vector3 positionCamera = transform.GetComponentInParent<Transform>().position;
        Quaternion quaternionCamera = transform.GetComponentInParent<Transform>().rotation;

		Instantiate (PlaneAction.currentPlane.gameObject, positionCamera, quaternionCamera);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// add score function
	public void AddExp(int exp)
	{
		ProgressController.expAdd += exp;
        Score += exp;
		Killed += 1;
	}
	
	void OnGUI(){
		//GUI.Label(new Rect(20,20,300,30),"Kills "+Score);
	}
	// game over fimction
	public void GameOver(){
		GameUI menu = (GameUI)GameObject.FindObjectOfType(typeof(GameUI));
		if(menu){
			menu.Mode = 1;	
		}
	}
}
