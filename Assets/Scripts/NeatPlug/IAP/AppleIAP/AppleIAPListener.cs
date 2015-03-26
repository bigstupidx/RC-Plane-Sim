/**
 * AppleIAPListener.cs
 * 
 * AppleIAPListener listens to the Apple In-App IAP events.
 * File location: Assets/Scripts/NeatPlug/IAP/AppleIAP/AppleIAPListener.cs
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-apple-iap-plugin to find information 
 * about how to integrate and use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All Rights Reserved.
 * 
 * @version 1.3.0
 * @iap_api since iOS SDK 4.0 
 *
 */

using UnityEngine;
using System.Collections;

public class AppleIAPListener : MonoBehaviour {
	
	// Debug information printing switch, turn it off in production environment.
	private bool _debug = true;

	void Awake()
	{
		// The gameObject will retain...
		DontDestroyOnLoad(this);
		AppleIAP.Instance();		
	}
	
	void OnEnable()
	{
		// Register the IAP events.
		// Do not modify the codes below.		
		AppleIAPAgent.OnIAPSupported += OnIAPSupported;			
		AppleIAPAgent.OnItemDataReady += OnItemDataReady;	
		AppleIAPAgent.OnItemDataFailed += OnItemDataFailed;
		AppleIAPAgent.OnOwnedItemReported += OnOwnedItemReported;		
		AppleIAPAgent.OnPurchaseCompleted += OnPurchaseCompleted;
		AppleIAPAgent.OnPurchaseFailed += OnPurchaseFailed;
		AppleIAPAgent.OnPurchaseCancelled += OnPurchaseCancelled;		
		AppleIAPAgent.OnItemAlreadyOwned += OnItemAlreadyOwned;		
		AppleIAPAgent.OnReceiptPosted += OnReceiptPosted;
		AppleIAPAgent.OnReceiptFailedToPost += OnReceiptFailedToPost;
		AppleIAPAgent.OnCompletedTransactionsRestored += OnCompletedTransactionsRestored;
		AppleIAPAgent.OnCompletedTransactionsFailedToRestore += OnCompletedTransactionsFailedToRestore;
	}

	void OnDisable()
	{
		// Unregister the IAP events.
		// Do not modify the codes below.		
		AppleIAPAgent.OnIAPSupported -= OnIAPSupported;				
		AppleIAPAgent.OnItemDataReady -= OnItemDataReady;	
		AppleIAPAgent.OnItemDataFailed -= OnItemDataFailed;
		AppleIAPAgent.OnOwnedItemReported -= OnOwnedItemReported;		
		AppleIAPAgent.OnPurchaseCompleted -= OnPurchaseCompleted;
		AppleIAPAgent.OnPurchaseFailed -= OnPurchaseFailed;
		AppleIAPAgent.OnPurchaseCancelled -= OnPurchaseCancelled;		
		AppleIAPAgent.OnItemAlreadyOwned -= OnItemAlreadyOwned;		
		AppleIAPAgent.OnReceiptPosted -= OnReceiptPosted;
		AppleIAPAgent.OnReceiptFailedToPost -= OnReceiptFailedToPost;
		AppleIAPAgent.OnCompletedTransactionsRestored -= OnCompletedTransactionsRestored;
		AppleIAPAgent.OnCompletedTransactionsFailedToRestore -= OnCompletedTransactionsFailedToRestore;
	}
	
	/**
	 * Fired when the check for the In-App Purchase support is done.	
	 */
	void OnIAPSupported(bool supported)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnIAPSupported: supported -> " + supported.ToString());
		
		/// Your code here...
	}
	
	/**
	 * Fired when the item data is ready to query.
	 * Do your item query then if you need.
	 */
	void OnItemDataReady()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemDataReady() Fired.");
		
		/// Your code here... (Sample codes provided below)
		
		/*************************************************************
		  BEGIN CODE SAMPLE : Sample of getting item information.
		 *************************************************************
		 
		// Get item info:
		AppleIAPItem iapItem = AppleIAP.Instance().GetItemInfo("your_item_productId");		
		Debug.Log ("iapItem.title: " + iapItem.title);
		Debug.Log ("iapItem.description: " + iapItem.description);
		Debug.Log ("iapItem.price: " + iapItem.price);		
		Debug.Log ("iapItem.currency: " + iapItem.currency);		
		
		// Get item price:
		float itemPrice = AppleIAP.Instance().GetItemPrice("your_item_productId");
		Debug.Log ("itemPrice: " + itemPrice);		
		
		*************************************************************
		 END CODE SAMPLE : Sample of getting item information.
		*************************************************************/
	}	
	
	/**
	 * Fired when failed to query item data.
	 * 
	 * @param err
	 *         string - The error code returned.
	 */
	void OnItemDataFailed(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemDataFailed Fired. Err -> " + err);	
		
		/// Your code here...
	}
	
	/**
	 * Fired when receiving an owned item report event.
	 * 
	 * This indicates that the item type is "NonConsumable" and the user has already 
	 * owned the item. By default the plugin gets notified with the event every time your 
	 * app launches, it is suggested that you should redeliver the item to the user here 
	 * if the locally saved data record cannot be found. (Probably the user cleared the 
	 * PlayerPrefs data or a new device is being used) 
	 * 
	 * @param receipt
	 *           AppleIAPReceipt - An object which contains the purchase information. 
	 *                              { productId, transactionDate, transactionId, 
	 *                                transactionState, quantity, receiptData }
	 */	
	void OnOwnedItemReported(AppleIAPReceipt receipt)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnOwnedItemReported Fired. { \n" 
				+ "productId -> " + receipt.productId + ", \n" 
				+ "transactionDate -> " + receipt.transactionDate.ToString() + ", \n"		
				+ "transactionId -> " + receipt.transactionId + ", \n"			  
			    + "quantity -> " + receipt.quantity + ", \n"		           
			    + "receipt -> " + receipt.data + " }"
			);
		
		/// This is the best place to re-deliver the NonConsumable item if you cannot 
		/// find the locally saved data records saying the item has been delivered.	
		
		switch(receipt.productId)
		{
		case "com.lunagames.RCW1":
			ProgressController.isSas = true;
			var go = GameObject.Find("Dragonfly");
			if(go != null)
			{
				go.GetComponent<PlaneAction>().UpdatePlane(2);
				GameObject.Find("SAS Button").SetActive(false);
			}
			
			ProgressController.SaveProgress();
			break;
		case "com.lunagames.RCW2":
			ProgressController.adsNeeded = true;
			break;
		}
		
		ProgressController.SaveProgress ();
	}		
	
	/**
	 * Fired when the purchase successfully completed.	
	 * 
	 * This is where you should deliver the item to the user.
	 * 
	 * @param receipt
	 *           AppleIAPReceipt - An object which contains the purchase information. 
	 *                              { productId, transactionDate, transactionId, 
	 *                                transactionState, quantity, receiptData }
	 */
	void OnPurchaseCompleted(AppleIAPReceipt receipt)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseCompleted Fired. { \n" 
				+ "productId -> " + receipt.productId + ", \n" 
				+ "transactionDate -> " + receipt.transactionDate.ToString() + ", \n"		
				+ "transactionId -> " + receipt.transactionId + ", \n"			  
			    + "quantity -> " + receipt.quantity + ", \n"		           
			    + "receipt -> " + receipt.data + " }"			  
			);
		
		switch(receipt.productId)
		{
		case "com.lunagames.RCW1":
			ProgressController.isSas = true;
			var go = GameObject.Find("Dragonfly");
			if(go != null)
			{
				go.GetComponent<PlaneAction>().UpdatePlane(2);
				GameObject.Find("SAS Button").SetActive(false);
			}
			
			ProgressController.SaveProgress();
			break;
		case "com.lunagames.RCW2":
			ProgressController.adsNeeded = true;
			break;
		case "com.lunagames.RCW3":
			ProgressController.gold += 4000;
			break;
		case "com.lunagames.RCW4":
			ProgressController.gold += 10000;
			break;
		case "com.lunagames.RCW5":
			ProgressController.gold += 30000;
			break;
		}

		ProgressController.SaveProgress ();
	}	
	
	/**
	 * Fired when the purchase failed.
	 * 		 
	 * @param productId
	 *           string - IAP item identifier, the Product ID you defined at iTunesConnect.	
	 * 
	 * @param err
	 *           string - The reason for failure.
	 */	
	void OnPurchaseFailed(string productId, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseFailed Fired. productId -> " + productId + ", err -> " + err);	
		
		/// Your code here...		
	}	
	
	/**
	 * Fired when the purchase cancelled by the user.
	 * 		 
	 * @param productId
	 *           string - IAP item identifier, the Product ID you defined at iTunesConnect.
	 */	
	void OnPurchaseCancelled(string productId)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseCancelled Fired. productId -> " + productId);	
		
		/// Your code here...			
	}
	
	/**
	 * Fired if the user has already owned this NonConsumable item when a corresponding
	 * purchase is attempted.
	 *
	 * This indicates that the item type is "NonConsumable" and the user has already owned
	 * the item. This event is only triggered in case you ignored the default automatic
	 * owned item reporting happened in OnOwnedItemReported() at app launches, but 
	 * you are not suggested to do so since requiring the user who has already purchased 
	 * the NonConsumable item to perform the purchase again, and tell the user "You have already
	 * owned the item", is obviously causing confusion.
	 *
	 * In most cases you should only play with the "OnOwnedItemReported()" event.
	 * But you can use this where you really need it to be that way.
	 *
	 * @param productId
	 *           string - IAP item identifier, the productId you defined at iTunesConnect.	
	 */	
	void OnItemAlreadyOwned(string productId)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemAlreadyOwned(" + productId + ") Fired.");		
		
		/// Your code here...
	}
	
	/**
	 * Fired when the receipt data is successfully posted to your server.
	 * 
	 * Developers who need additional server-side process for the purchase would need this
	 * feature, if you don't have any server-side processes, no need to read this.
	 * 
	 * Posting a returned receipt to your own server is done automatically by the plugin,
	 * all you have to do is to simply fill in the "ServerReceiveReceiptURL" property of
	 * the AppleIAPAgent gameObject in your first scene. The receipt posting action will
	 * happen every time a purchase is successfully completed.
	 * 
	 * The posting receipt mechanism is for you to easily upload the returned receipt data
	 * to your own server for further verification, it is for security purpose.
	 * 
	 * @param productId
	 *           string - IAP item identifier, the productId you defined at iTunesConnect.	
	 * 
	 * @param response
	 *           string - The response from your server.
	 * 	
	 */	
	void OnReceiptPosted(string productId, string response)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnReceiptPosted(" + productId + ") Fired. Server Response -> " + response);		
		
		/// Your code here...
	}
	
	/**
	 * Fired when the receipt data failed to be posted.
	 * 
	 * Developers who need additional server-side process for the purchase would need this
	 * feature, if you don't have any server-side processes, no need to read this.
	 * 
	 * Posting a returned receipt to your own server is done automatically by the plugin,
	 * all you have to do is to simply fill in the "ServerReceiveReceiptURL" property of
	 * the AppleIAPAgent gameObject in your first scene. The receipt posting action will
	 * happen every time a purchase is successfully completed.
	 * 
	 * The posting receipt mechanism is for you to easily upload the returned receipt data
	 * to your own server for further verification, it is for security purpose.
	 * 
	 * @param productId
	 *           string - IAP item identifier, the productId you defined at iTunesConnect.	
	 * 
	 * @param response
	 *           string - The response from your server.
	 * 	
	 */	
	void OnReceiptFailedToPost(string productId, string response)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnReceiptFailedToPost(" + productId + ") Fired. Server Response -> " + response);			
		
		/// Your code here...
	}
	
	/**
	 * Fired when the completed transactions are successfully restored.
	 * 
	 * NOTE: This event gets triggered only if you call 
	 * AppleIAP.Instance().RestoreCompletedTransactions().
	 * This event is for informational purpose only, you may want to
	 * inform the user of the restore completion here.
	 * 
	 * If there are any purchased Non-Consumable products, 
	 * then AppleIAPListener::OnOwnedItemReported() will be triggered, 
	 * you can redeliver the products to the user in that event handler.	
	 */
	void OnCompletedTransactionsRestored()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnCompletedTransactionsRestored() Fired.");
	}
	
	/**
	 * Fired when the completed transactions failed to restore.	
	 * 
	 * @param err
	 *           string - The error message returned from Apple.
	 */	
	void OnCompletedTransactionsFailedToRestore(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnCompletedTransactionsFailedToRestore Fired. err -> " + err);	
		
		/// Your code here...		
	}		
	
	
}
