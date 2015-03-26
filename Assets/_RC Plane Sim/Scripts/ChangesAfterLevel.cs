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
        expAddLabel = GameObject.Find("Exp Add").GetComponent<UILabel>();
		ejectAddLabel = transform.FindChild("Eject Add").GetComponent<UILabel>();

		goldSAS = GameObject.Find("Gold Add SAS");
		expSAS = GameObject.Find("Exp Add SAS");
	}
	
	void OnEnable () 
    {
		int i = 0;
		int index = 0;
		for(; i < UILevelController.exps.Length; i++)
		{
			if(ProgressController.exp > UILevelController.exps[i])
			{
				index = i;
			}
		}

		if(index + 1 == UILevelController.exps.Length)
		{
			ProgressController.expAdd = 0;
		}

		if(ProgressController.isSas)
		{
			goldAddLabel.text = "Coins: + " + (ProgressController.goldAdd * 2).ToString();
			expAddLabel.text = "Experience: + " + (ProgressController.expAdd * 2).ToString();
		}
		else
		{
			goldAddLabel.text = "Coins: + " + ProgressController.goldAdd.ToString();
			expAddLabel.text = "Experience: + " + ProgressController.expAdd.ToString();
		}

		ejectAddLabel.text = "Eject: + " + ProgressController.ejectAdd.ToString () + " Coins";

		if(TypeAction.type == TypeAction.FREE_FOR_ALL)
		{
			ProgressController.scoreFree[ProgressController.level, SwipeAction.levelDifficult - 1] = Mathf.Max(ProgressController.scoreFree[ProgressController.level, SwipeAction.levelDifficult - 1], ProgressController.killAdd);
		}
		else if(TypeAction.type == TypeAction.SURVIVAL)
		{
			ProgressController.scoreWave[ProgressController.level] = Mathf.Max(ProgressController.scoreWave[ProgressController.level], ProgressController.killAdd);
		}

		if(goldSAS != null)  goldSAS.GetComponent<UILabel>().text = "Coins: + " + ProgressController.goldAdd.ToString();
		if(expSAS != null)  expSAS.GetComponent<UILabel>().text = "Experience: + " + ProgressController.expAdd.ToString();

		ProgressController.goldAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;
		ProgressController.expAdd *= ProgressController.isSas ? ProgressController.sasBonus : 1;

        ProgressController.gold += ProgressController.goldAdd + ProgressController.ejectAdd;
        
        ProgressController.exp += ProgressController.expAdd;
		ProgressController.expAdd = 0;

		if(Application.loadedLevel > 2)
        	ProgressController.SaveProgress();
	}
}
