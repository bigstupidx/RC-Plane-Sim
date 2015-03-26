using UnityEngine;
using System.Collections;
using Prime31;
using Prime31.WinPhoneStore;


public class WinPhoneStoreDemoUI : MonoBehaviourGUI 
{
#if UNITY_WP8

	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Load Demo Licensing Info" ) )
		{
			// xml with two mock products for testing split up on many lines for easy reading/modification
			var xml = "<?xml version=\"1.0\"?><ProductListings>"
				+ "<ProductListing Key=\"test1key\" Purchased=\"false\" Fulfilled=\"false\">"
					+ "<Name>Test Durable Product</Name>"
					+ "<Description>A sample product listing</Description>"
					+ "<ProductId>test.durable.1</ProductId>"
					+ "<ProductType>Durable</ProductType>"
					+ "<FormattedPrice>$1.00</FormattedPrice>"
					+ "<ImageUri>http://google.com/someImage.png</ImageUri>"
					+ "<Keywords>test;product</Keywords>"
					+ "<Tag>Additional text</Tag>"
				+ "</ProductListing>"
				+ "<ProductListing Key=\"test2key\" Purchased=\"false\" Fulfilled=\"false\">"
					+ "<Name>Test Consumable Product</Name>"
					+ "<Description>A sample product listing</Description>"
					+ "<ProductId>test.consumable.1</ProductId>"
					+ "<ProductType>Consumable</ProductType>"
					+ "<FormattedPrice>$0.99</FormattedPrice>"
					+ "<ImageUri></ImageUri>"
					+ "<Keywords>test;product</Keywords>"
					+ "<Tag>Additional text</Tag>"
				+ "</ProductListing>"
				+ "</ProductListings>";
			
			// by calling loadTestingLicenseXml you are putting the app into test mode
			Store.loadTestingLicenseXml( xml );
		}
		
		
		if( GUILayout.Button( "Clear Test Cache" ) )
		{
			Store.clearCache();
		}
		
		
		if( GUILayout.Button( "Load Listing Info" ) )
		{
			Store.loadListingInformation( listingInfo =>
			{
				Debug.Log( "listing information loaded: " + listingInfo );

				// print out all of our product listings as well
				foreach( var prod in listingInfo.productListings )
					Debug.Log( prod );
			});
		}
		
		
		if( GUILayout.Button( "Show Marketplace Detail Page" ) )
		{
			Store.showMarketplaceDetailPage();
		}
		
		
		endColumn( true );
		
		
		if( GUILayout.Button( "Get App License Info" ) )
		{
			Debug.Log( "App License Info: " + Store.getLicenseInformation() );
		}
		

		if( GUILayout.Button( "Purchase Durable" ) )
		{
			Store.requestProductPurchase( "test.durable.1", ( receipt, error ) =>
			{
				// we will either have a receipt or an error
				if( receipt != null )
					Debug.Log( "purchase completed with receipt: " + receipt );
				else if( error != null )
					Debug.Log( "error purchasing product: " + error );
			});
		}
		
		
		if( GUILayout.Button( "Purchase Consumable" ) )
		{
			Store.requestProductPurchase( "test.consumable.1", ( receipt, error ) =>
			{
				// we will either have a receipt or an error
				if( receipt != null )
					Debug.Log( "purchase completed with receipt: " + receipt );
				else if( error != null )
					Debug.Log( "error purchasing product: " + error );
			});
		}
		
		
		if( GUILayout.Button( "Report Product Fulfillment" ) )
		{
			Store.reportProductFulfillment( "test.consumable.1" );
		}
		
		
		if( GUILayout.Button( "Get Product License" ) )
		{
			Debug.Log( "Product License: " + Store.getProductLicense( "test.consumable.1" ) );
		}
		
		endColumn();
	}

#endif
}
