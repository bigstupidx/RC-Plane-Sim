using UnityEngine;
using System.Collections;

public class ProgressController : MonoBehaviour 
{
    public static int gold;
    public static int exp;
    public static int goldAdd;
    public static int expAdd;

	void Awake () 
    {
        LoadProgress();
	}

    public static void SaveProgress() 
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Exp", exp);
    }

    void LoadProgress()
    {
        gold = PlayerPrefs.GetInt("Gold");
        exp = PlayerPrefs.GetInt("Exp");
    }
}
