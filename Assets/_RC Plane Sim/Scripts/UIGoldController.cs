using UnityEngine;
using System.Collections;

public class UIGoldController : MonoBehaviour 
{
    private UILabel goldLabel;

	void Awake () 
    {
        goldLabel = transform.FindChild("Gold Label").GetComponent<UILabel>();   
	}

    void OnEnable()
    {
        goldLabel.text = ProgressController.gold.ToString();
    }
}
