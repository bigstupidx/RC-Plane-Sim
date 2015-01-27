using UnityEngine;
using System.Collections;

public class ChangesAfterLevel : MonoBehaviour
{
    private UILabel goldAddLabel;
    private UILabel expAddLabel;
	private UILabel killAddLabel;
	private UILabel ejectAddLabel;

	private GameObject goldSAS;
	private GameObject expSAS;

	void Awake () 
    {
        goldAddLabel = transform.FindChild("Gold Add").GetComponent<UILabel>();
        expAddLabel = transform.FindChild("Exp Add").GetComponent<UILabel>();
		ejectAddLabel = transform.FindChild("Eject Add").GetComponent<UILabel>();
		killAddLabel = transform.FindChild ("killAddLabel").GetComponent<UILabel> ();

		goldSAS = GameObject.Find("Gold Add SAS");
		expSAS = GameObject.Find("Exp Add SAS");
	}
	
	void OnEnable () 
    {
		goldAddLabel.text = "Gold: + " + ProgressController.goldAdd.ToString();
		expAddLabel.text = "Experience: + " + ProgressController.expAdd.ToString();
		ejectAddLabel.text = "Eject: + " + ProgressController.ejectAdd.ToString ();
		killAddLabel.text = ProgressController.killAdd.ToString ();

		if(goldSAS != null)  goldSAS.GetComponent<UILabel>().text = "Gold: + " + ProgressController.goldAdd.ToString();
		if(expSAS != null)  expSAS.GetComponent<UILabel>().text = "Experience: + " + ProgressController.expAdd.ToString();

		ProgressController.goldAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;
		ProgressController.expAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;

        ProgressController.gold += ProgressController.goldAdd + ProgressController.ejectAdd;
		ProgressController.goldAdd = 0;
		ProgressController.ejectAdd = 0;
        
        ProgressController.exp += ProgressController.expAdd;
        ProgressController.expAdd = 0;

		ProgressController.killAdd = 0;

		if(Application.loadedLevel > 2)
        	ProgressController.SaveProgress();
	}
}
