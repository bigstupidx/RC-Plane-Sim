using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class LevelsLoader : MonoBehaviour 
{
	//loading background
	public GUIStyle front;
	//animated loading indicator
	public GUIStyle loader;
	//helper for loading animation	
	private float angle;
	//is background needed
	public static bool isBlack;

	//auto define game quality
    void Awake()
    {
        if(Screen.dpi > 300)
        {
            QualitySettings.SetQualityLevel(3);
        }
        else
        {
            QualitySettings.SetQualityLevel(2);
        }
    }

	void OnLevelWasLoaded(int level) 
	{
		//hide back on loading complete
		isBlack = false;

		//show ads in garage level
        if (ProgressController.adsNeeded || level != 2)
            return;

		#if UNITY_IPHONE || UNITY_ANDROID
		Chartboost.showInterstitial (CBLocation.Default);
		#else
        AdmobAd.Instance().LoadInterstitialAd();
		#endif
    }

	//show loading back and load level in background
	public static void LoadLevel(int level)
	{
		isBlack = true;
		Application.LoadLevelAsync(level);
	}

	void OnEnable() 
	{
		// Listen to all impression-related events for Chartboost
		Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
		Chartboost.didDismissInterstitial += didDismissInterstitial;
		Chartboost.didCloseInterstitial += didCloseInterstitial;
		Chartboost.didClickInterstitial += didClickInterstitial;
		Chartboost.didCacheInterstitial += didCacheInterstitial;
		Chartboost.shouldDisplayInterstitial += shouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial += didDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps += didFailToLoadMoreApps;
		Chartboost.didDismissMoreApps += didDismissMoreApps;
		Chartboost.didCloseMoreApps += didCloseMoreApps;
		Chartboost.didClickMoreApps += didClickMoreApps;
		Chartboost.didCacheMoreApps += didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps += shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps += didDisplayMoreApps;
		Chartboost.didFailToRecordClick += didFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
		Chartboost.didClickRewardedVideo += didClickRewardedVideo;
		Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;
		Chartboost.didCacheInPlay += didCacheInPlay;
		Chartboost.didFailToLoadInPlay += didFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation += didPauseClickForConfirmation;
		Chartboost.willDisplayVideo += willDisplayVideo;
		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
		#endif
	}

	void OnDisable() 
	{
		// Remove event handlers for Chartboost
		Chartboost.didFailToLoadInterstitial -= didFailToLoadInterstitial;
		Chartboost.didDismissInterstitial -= didDismissInterstitial;
		Chartboost.didCloseInterstitial -= didCloseInterstitial;
		Chartboost.didClickInterstitial -= didClickInterstitial;
		Chartboost.didCacheInterstitial -= didCacheInterstitial;
		Chartboost.shouldDisplayInterstitial -= shouldDisplayInterstitial;
		Chartboost.didDisplayInterstitial -= didDisplayInterstitial;
		Chartboost.didFailToLoadMoreApps -= didFailToLoadMoreApps;
		Chartboost.didDismissMoreApps -= didDismissMoreApps;
		Chartboost.didCloseMoreApps -= didCloseMoreApps;
		Chartboost.didClickMoreApps -= didClickMoreApps;
		Chartboost.didCacheMoreApps -= didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps -= shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps -= didDisplayMoreApps;
		Chartboost.didFailToRecordClick -= didFailToRecordClick;
		Chartboost.didFailToLoadRewardedVideo -= didFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo -= didDismissRewardedVideo;
		Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
		Chartboost.didClickRewardedVideo -= didClickRewardedVideo;
		Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
		Chartboost.shouldDisplayRewardedVideo -= shouldDisplayRewardedVideo;
		Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
		Chartboost.didDisplayRewardedVideo -= didDisplayRewardedVideo;
		Chartboost.didCacheInPlay -= didCacheInPlay;
		Chartboost.didFailToLoadInPlay -= didFailToLoadInPlay;
		Chartboost.didPauseClickForConfirmation -= didPauseClickForConfirmation;
		Chartboost.willDisplayVideo -= willDisplayVideo;
		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow -= didCompleteAppStoreSheetFlow;
		#endif
	}

	void didFailToLoadInterstitial(CBLocation location, CBImpressionError error) 
	{
		Debug.Log(string.Format("didFailToLoadInterstitial: {0} at location {1}", error, location));
	}
	
	void didDismissInterstitial(CBLocation location) 
	{
		Debug.Log("didDismissInterstitial: " + location);
	}
	
	void didCloseInterstitial(CBLocation location) 
	{
		Debug.Log("didCloseInterstitial: " + location);
	}
	
	void didClickInterstitial(CBLocation location) 
	{
		Debug.Log("didClickInterstitial: " + location);
	}
	
	void didCacheInterstitial(CBLocation location) 
	{
		Debug.Log("didCacheInterstitial: " + location);
	}
	
	bool shouldDisplayInterstitial(CBLocation location) 
	{
		Debug.Log("shouldDisplayInterstitial: " + location);
		return true;
	}
	
	void didDisplayInterstitial(CBLocation location)
	{
		Debug.Log("didDisplayInterstitial: " + location);
	}
	
	void didFailToLoadMoreApps(CBLocation location, CBImpressionError error) 
	{
		Debug.Log(string.Format("didFailToLoadMoreApps: {0} at location: {1}", error, location));
	}
	
	void didDismissMoreApps(CBLocation location) 
	{
		Debug.Log(string.Format("didDismissMoreApps at location: {0}", location));
	}
	
	void didCloseMoreApps(CBLocation location) 
	{
		Debug.Log(string.Format("didCloseMoreApps at location: {0}", location));
	}
	
	void didClickMoreApps(CBLocation location) 
	{
		Debug.Log(string.Format("didClickMoreApps at location: {0}", location));
	}
	
	void didCacheMoreApps(CBLocation location) 
	{
		Debug.Log(string.Format("didCacheMoreApps at location: {0}", location));
	}
	
	bool shouldDisplayMoreApps(CBLocation location) 
	{
		Debug.Log(string.Format("shouldDisplayMoreApps at location: {0}", location));
		return true;
	}
	
	void didDisplayMoreApps(CBLocation location){
		Debug.Log("didDisplayMoreApps: " + location);
	}
	
	void didFailToRecordClick(CBLocation location, CBImpressionError error) 
	{
		Debug.Log(string.Format("didFailToRecordClick: {0} at location: {1}", error, location));
	}
	
	void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error) 
	{
		Debug.Log(string.Format("didFailToLoadRewardedVideo: {0} at location {1}", error, location));
		//hide free coins button
		GameObject.Find ("Gold Background 1").transform.localScale = Vector3.zero;
	}
	
	void didDismissRewardedVideo(CBLocation location) 
	{
		Debug.Log("didDismissRewardedVideo: " + location);
	}
	
	void didCloseRewardedVideo(CBLocation location) 
	{
		Debug.Log("didCloseRewardedVideo: " + location);
	}
	
	void didClickRewardedVideo(CBLocation location) 
	{
		Debug.Log("didClickRewardedVideo: " + location);
	}
	
	void didCacheRewardedVideo(CBLocation location) 
	{
		Debug.Log("didCacheRewardedVideo: " + location);
		//show free coins button
		GameObject.Find ("Gold Background 1").transform.localScale = Vector3.one;
	}
	
	bool shouldDisplayRewardedVideo(CBLocation location) 
	{
		Debug.Log("shouldDisplayRewardedVideo: " + location);
		return true;
	}
	
	void didCompleteRewardedVideo(CBLocation location, int reward) 
	{
		Debug.Log(string.Format("didCompleteRewardedVideo: reward {0} at location {1}", reward, location));
		//reward player with coins
		ProgressController.gold += 200;
		ProgressController.SaveProgress ();
	}
	
	void didDisplayRewardedVideo(CBLocation location)
	{
		Debug.Log("didDisplayRewardedVideo: " + location);
	}
	
	void didCacheInPlay(CBLocation location) {
		Debug.Log("didCacheInPlay called: "+location);
	}
	
	void didFailToLoadInPlay(CBLocation location, CBImpressionError error) 
	{
		Debug.Log(string.Format("didFailToLoadInPlay: {0} at location: {1}", error, location));
	}
	
	void didPauseClickForConfirmation() 
	{
		Debug.Log("didPauseClickForConfirmation called");
	}
	
	void willDisplayVideo(CBLocation location) 
	{
		Debug.Log("willDisplayVideo: " + location);
	}
	
	#if UNITY_IPHONE
	void didCompleteAppStoreSheetFlow() 
	{
		Debug.Log("didCompleteAppStoreSheetFlow");
	}
	#endif

	void OnGUI()
	{
		//if level is loading
		if(isBlack) 
		{
			angle++;
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "", front);
			//rotate loader  indicator
			Rect guiRect = new Rect((Screen.width - Screen.width * 0.11f) / 2, Screen.height - Screen.width * 0.115f, Screen.width * 0.11f, Screen.width * 0.1f);
			float xValue = ((guiRect.x + guiRect.width / 2.0f));
			float yValue = ((guiRect.y + guiRect.height * 0.65f));
			Vector2 Pivot = new Vector2(xValue, yValue);
			GUIUtility.RotateAroundPivot(angle, Pivot);
			GUI.Box(guiRect, "", loader);
			GUI.matrix = Matrix4x4.identity;
		}
	}
}
