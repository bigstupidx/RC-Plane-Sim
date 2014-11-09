using UnityEngine;
using System.Collections;

public class ChangesAfterLevel : MonoBehaviour
{
    private UILabel goldAddLabel;
    private UILabel expAddLabel;

	void Awake () 
    {
        goldAddLabel = transform.FindChild("Gold Add").GetComponent<UILabel>();
        expAddLabel = transform.FindChild("Exp Add").GetComponent<UILabel>();
	}
	
	void OnEnable () 
    {
        goldAddLabel.text = "Gold: + " + ProgressController.goldAdd.ToString();
        ProgressController.gold += ProgressController.goldAdd;
        ProgressController.goldAdd = 0;

        expAddLabel.text = "Experience: + " + ProgressController.expAdd.ToString();
        ProgressController.exp += ProgressController.expAdd;
        ProgressController.expAdd = 0;

        ProgressController.SaveProgress();
	}
}
