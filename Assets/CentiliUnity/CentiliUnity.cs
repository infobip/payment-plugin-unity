using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
namespace CentiliUnity 
{
	
	/// <summary>
	/// 
	/// </summary>
	public class CentiliPaymentRequest 
	{
		public string ApiKey;
		public int PackageIndex { get; set; }
		public string LanguageCode { get; set; }
		public string Info { get; set; }
		public string ClientId { get; set; }
		public bool TestModeEnabled { get; set; }
		public bool OfflineModeEnabled { get; set; }
		public double Price { get; set; }
		
		public CentiliPaymentRequest(string ApiKey) 
		{
			this.ApiKey = ApiKey;
			this.PackageIndex = -1;
			this.Price = -1.0d;
		}

		public override string ToString() 
		{
			var request = this;
			return string.Format("CentiliPaymentRequest: {\n\t{0},\n\t{1},\n\t{2},\n\t{3},\n\t{4},\n\t{5},\n\t{6}\n\t{7}}",
				"ApiKey: " + request.ApiKey == null										? "" : request.ApiKey,
				"PackageIndex: " + request.PackageIndex == null					? (int)(-1) : (int) request.PackageIndex,
				"LanguageCode: " + request.LanguageCode == null					? "EN" : request.LanguageCode,
				"Info: " + request.Info == null												? "" : request.Info,
				"ClientId: " + request.ClientId == null 									? "" : request.ClientId,
				"TestModeEnabled: " + request.TestModeEnabled == null			? false : request.TestModeEnabled,
				"OfflineModeEnabled: " + request.OfflineModeEnabled == null	? false : request.OfflineModeEnabled,
				"Price: " + request.Price == null ? (double)(-1.0d) : request.Price
			);
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class CentiliPaymentResponse 
	{
		
		public CentiliPaymentResponse(AndroidJavaObject activity)
		{
			ApiKey = activity.Get<string>("responseApiKey");
			ClientId = activity.Get<string>("responseClientId");
			Price = activity.Get<double>("responsePrice");
			ItemAmount = activity.Get<int>("responseItemAmount");
			ItemName = activity.Get<string>("responseItemName");
			Currency = activity.Get<string>("responseCurrency");
			TransactionId = activity.Get<string>("responseTransactionId");
			ErrorMessage = activity.Get<string>("responseErrorMessage");
		}
		
		// Amount of credits paid for by the user (if the user buys "3 swords" then this value is "3").
		public int ItemAmount;
		
		// Name of the credits paid for by the user (in case of "3 swords" this would be "swords").
		public string ItemName;

		// Currency in which transaction was processed
		public string Currency;
		
		// Id of the client
		public string ClientId;
		
		// Amount paid by the user in a fractional format (X.YZ)
		public double Price;
		
		// API KEY
		public string ApiKey;
		
		// Transaction id
		public string TransactionId;
		
		// Error message (null if no error)
		public string ErrorMessage;
		
		// Interval is omitted because subscription payment method is not used
		// public int Interval;
		
		public override string ToString() {
			return string.Format("CentiliPaymentResponse: {\n\t" +
				"ApiKey:{0},\n\tTransactionId:{1},\n\tItemAmount:{2},\n\t" +
				"ItemName:{3},\n\tPrice:{4},\n\tCurrency:{5},\n\t" +
				"ClientId:{6},\n\tErrorMessage:{7}\n}",
				ApiKey, TransactionId, ItemAmount, 
				ItemName, Price, Currency,
				ClientId, ErrorMessage);
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public enum CentiliPaymentStatus 
	{
		// Shouldn't ever get PENDING status (in end-user code)
		PAYMENT_PENDING = 0,
		PAYMENT_FAILED = 1,
		PAYMENT_CANCELED = 2,
		PAYMENT_SUCCESSFUL = 3
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class CentiliPaymentManager {
				
		/// <summary>
		/// 
		/// </summary>
		/*public static void SetLoggingEnabled(bool loggingEnabled)
		{
			if(Application.platform == RuntimePlatform.Android) {
				CentiliInternalWrapper.SetLoggingEnabled(loggingEnabled);
			}
		}
		*/
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name='request'>
		/// the CentiliPaymentRequest object for the payment
		/// </param>
		/// <param name='callback'>
		/// the callback method on submitting request for payment
		/// </param>
		public static void MakePayment(CentiliPaymentRequest request, Action<CentiliPaymentStatus, CentiliPaymentResponse> callback)
		{
			if(Application.platform == RuntimePlatform.Android) {
				CentiliInternalWrapper.Instance.MakePayment(request, callback);
			} else {
				// your mockup code here
			}
		}
		
		private static bool _PendingTransactionHandling = true;
		public static bool PendingTransactionHandling { 
			get 
			{
				return _PendingTransactionHandling;
			}
			set
			{
				if (Application.platform == RuntimePlatform.Android) {
					CentiliInternalWrapper.Instance.SetPendingTransactionHandling(value);
				}
				_PendingTransactionHandling = value;
			}
		}
		
		private static bool _DebugMode = false;
		public static bool DebugMode {
			get 
			{
				return _DebugMode;
			}
			set
			{
				if (Application.platform == RuntimePlatform.Android) {
					CentiliInternalWrapper.Instance.SetDebugMode(value);
				}
				_DebugMode = value;
			}
		}

	}
}