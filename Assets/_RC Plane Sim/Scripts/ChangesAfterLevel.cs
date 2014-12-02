using UnityEngine;
using System.Collections;

public class ChangesAfterLevel : MonoBehaviour
{
    private UILabel goldAddLabel;
    private UILabel expAddLabel;

	private GameObject goldSAS;
	private GameObject expSAS;

	void Awake () 
    {
        goldAddLabel = transform.FindChild("Gold Add").GetComponent<UILabel>();
        expAddLabel = transform.FindChild("Exp Add").GetComponent<UILabel>();

		goldSAS = GameObject.Find("Gold Add SAS");
		expSAS = GameObject.Find("Exp Add SAS");
	}
	
	void OnEnable () 
    {
		ProgressController.goldAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;
		ProgressController.expAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;

        goldAddLabel.text = "Gold: + " + ProgressController.goldAdd.ToString();
		if(goldSAS != null)  goldSAS.GetComponent<UILabel>().text = "Gold: + " + (ProgressController.goldAdd * ProgressController.sasBonus).ToString();
        ProgressController.gold += ProgressController.goldAdd;
        ProgressController.goldAdd = 0;

        expAddLabel.text = "Experience: + " + ProgressController.expAdd.ToString();
		if(expSAS != null)  expSAS.GetComponent<UILabel>().text = "Experience: + " + (ProgressController.expAdd * ProgressController.sasBonus).ToString();
        ProgressController.exp += ProgressController.expAdd;
        ProgressController.expAdd = 0;

		if(Application.loadedLevel > 2)
        	ProgressController.SaveProgress();
	}
}
