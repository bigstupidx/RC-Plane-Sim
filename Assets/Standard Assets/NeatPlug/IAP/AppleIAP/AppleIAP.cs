/**
 * AppleIAP.cs
 * 
 * A Singleton class encapsulating public access methods for Apple IAP processes. 
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-apple-iap-plugin to find information how 
 * to use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All rights reserved.
 * 
 * @version 1.3.0
 * @iap_api since iOS SDK 4.0
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

public class AppleIAP  {	
	
	#region Fields
	
	private static AppleIAP _instance = null;	
		
	#endregion
	
	#region Functions	
	
	/**
	 * Constructor.
	 */
	private AppleIAP()
	{	
#if UNITY_IPHONE		
		AppleIAPIOS.Instance();
#endif
	}
	
	/**
	 * Instance method.
	 */
	public static AppleIAP Instance()
	{		
		if (_instance == null) 
		{
			_instance = new AppleIAP();
		}
		
		return _instance;
	}	
	
	/**
	 * Initialization.
	 * 
	 * @param productIds
	 *              string[] - A set of product IDs you want the plugin to automatically 
	 *                         retrieve information for you.
	 * 
	 * @param showSpinner
	 *              bool - True for showing a spinner while a transaction is in process.
	 * 
	 * @param verifyReceiptFromDevice
	 *              bool - True for verifying receipt from the user device once the 
	 *                     purchase is "completed". If the verification succeeds, 
	 *                     AppleIAPListener::OnPurchaseCompleted() will be triggered,
	 *                     but if the receipt is invalid, AppleIAPListener::OnPurchaseFailed()
	 *                     will be triggered.	               
	 * 
	 * @param serverReceiveReceiptURL
	 *              string - Setting a valid URL on your own server enables posting the
	 *                       StoreKit Receipt to your server, for further server-to-server 
	 *                       verification and other server-side processes you need.
	 *                       e.g. creating subscirption entries, preparing downloadable 
	 *                       content, etc...
	 */
	public void Initialize(string[] productIds, bool showSpinner, bool verifyReceiptFromDevice, string serverReceiveReceiptURL)
	{
#if UNITY_IPHONE		
		AppleIAPIOS.Instance().Initialize(productIds, showSpinner, verifyReceiptFromDevice, serverReceiveReceiptURL);
#endif		
	}	
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param productId
	 *           string - IAP item identifier, the Product ID you defined at itunesConnect.
	 * 	
	 * 	
	 */
	public void Purchase(string productId)
	{
#if UNITY_IPHONE		
		AppleIAPIOS.Instance().Purchase(productId, 1, null);
#endif			
	}		
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param productId
	 *           string - IAP item identifier, the Product ID you defined at itunesConnect.
	 * 
	 * @param quantity
	 *           int - The quantity of prodcuts to buy.
	 * 	
	 */
	public void Purchase(string productId, int quantity)
	{
#if UNITY_IPHONE		
		AppleIAPIOS.Instance().Purchase(productId, quantity, null);
#endif			
	}
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param productId
	 *           string - IAP item identifier, the Product ID you defined at itunesConnect.
	 * 
	 * @param quantity
	 *           int - The quantity of prodcuts to buy.
	 * 
	 * @param extras
	 *           Dictionary - The Extra parameters you want to post the receipt along with.
	 *                        (This is only needed if you set "ServerReceiveReceiptURL"
	 *                          property of AppleIAPAgent gameObject, that says you want
	 *                          the plugin to automatically post the receipt data to your 
	 *                          server for further verification)
	 * 	
	 */
	public void Purchase(string productId, int quantity, Dictionary<string, string> extras)
	{
#if UNITY_IPHONE		
		AppleIAPIOS.Instance().Purchase(productId, quantity, extras);
#endif			
	}	
		
	/**
	 * Get the specified item information.
	 * 
	 * The item information is retrieved at app launch and it is cached in plugin for better performance.
	 * You should always call this function in AppleIAPListener::OnItemDataReady() to make sure the data
	 * has been ready for you to query.
	 * 
	 * @param productId
	 * 			string - IAP item identifier, the productId you defined at itunesConnect.
	 * 
	 * @return 
	 * 			AppleIAPItem - A Apple IAP Item which contains { title, description, price, currencySymbol }
	 */
	public AppleIAPItem GetItemInfo(string productId)
	{
		AppleIAPItem item = null;		
		
#if UNITY_IPHONE		
		item = AppleIAPIOS.Instance().GetItemInfo(productId);
#endif
		
		return item;
	}	
	
	/**
	 * Restore completed transactions.
	 * 
	 * This is useful when your locally saved purchase records are somehow wiped out on the device,
	 * in this case you may want to redeliver the purchased Non-consumable products if any.
	 * 
	 * NOTE: Since Apple requires the user to authenticate before restoring completed transactions,
	 * this task cannot be done automatically when App starts. You would need to add a button named
	 * "Restore Completed Transactions", to provide the user with a way to restore his / her original 
	 * purchases.
	 * 
	 */
	public void RestoreCompletedTransactions()
	{
#if UNITY_IPHONE		
		AppleIAPIOS.Instance().RestoreCompletedTransactions();
#endif
	}
	
	/**
	 * Get the price of specified item.
	 * 
	 * The item information is retrieved at app launch and it is cached in plugin for better performance.
	 * You should always call this function in AppleIAPListener::OnItemDataReady() to make sure the data
	 * has been ready for you to query.
	 * 
	 * @param productId
	 * 			string - IAP item identifier, the productId you defined at itunesConnect.
	 * 
	 * @return 
	 * 			float - The price of the item
	 */	
	public float GetItemPrice(string productId)
	{		
		float price = 0.0f;
		
#if UNITY_IPHONE		
		price = AppleIAPIOS.Instance().GetItemPrice(productId);
#endif
		
		return price;		
	}	

	#endregion
}
