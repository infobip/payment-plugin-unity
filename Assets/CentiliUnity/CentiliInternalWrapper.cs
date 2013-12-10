using UnityEngine;
using System;
using System.Collections;
using CentiliUnity;

public class CentiliInternalWrapper	: MonoBehaviour {
	
	private CentiliPaymentResponse lastCentiliPaymentResponse;
	private CentiliPaymentStatus lastCentiliPaymentStatus;
	
	public bool Logging { get; set; }
	public void SetLoggingEnabled(bool logging) 
	{
		Logging = logging;
	}
	
	public void MakePayment(CentiliPaymentRequest request, Action<CentiliPaymentStatus, CentiliPaymentResponse> callback)
	{
		StartCoroutine(MakePaymentCoroutine(request, callback));
	}
	
	public void SetPendingTransactionHandling(bool enabled) 
	{
		StartCoroutine(SetPendingTransactionHandlingCoroutine(enabled));
	}
	
	public void SetDebugMode(bool enabled) 
	{
		StartCoroutine(SetDebugModeCoroutine(enabled));
	}
	
	private IEnumerator SetDebugModeCoroutine(bool enabled) 
	{
		GetInstance();
		GetCurrentActivity().Call("setDebugModeEnabled", new object[] { enabled });
		yield return 0;
	}
	
	private IEnumerator SetPendingTransactionHandlingCoroutine(bool enabled)
	{
		GetInstance();
		GetCurrentActivity().Call("setPendingTransactionHandlingEnabled", new object[] { enabled });
		yield return 0;
	}
	
	/// <summary>
	/// MakePayment coroutine that can wait until the CentiliPaymentResponse is ready
	/// </summary>
	private IEnumerator MakePaymentCoroutine(CentiliPaymentRequest request, Action<CentiliPaymentStatus, CentiliPaymentResponse> callback)
	{
		// invoke singleton instance creation of this object, because we need it
		GetInstance();
		
		// Passing data over the bridge between C# here and Java running on Android has
		// a quirk, that it can't handle NULL values and throws exceptions. Here is a
		// workaround that uses an extra string to pass which variables are present and which
		// are absent.
		string presenceMask = new string(new char[]
		{
			request.ApiKey == null					? 'a' : 'p',
			//request.PackageIndex == null		? 'a' : 'p',
			'p',
			request.LanguageCode == null		? 'a' : 'p',
			request.Info == null						? 'a' : 'p',
			request.ClientId == null 				? 'a' : 'p',
			// request.TestModeEnabled == null	? 'a' : 'p',
			'p',
			// request.OfflineModeEnabled == null	? 'a' : 'p',
			'p',
			// request.PendingTransactionHandlingEnabled == null ? 'a' : 'p'
			'p'
		});

		// Java side will use this parameter to return data
		lastCentiliPaymentResponse = null;
		
		// A method with a lot of parameters is acceptable here, because other options add even
		// more bloat and won't help with readability (serializations)
		GetCurrentActivity().Call("startPayment", new object[] 
		{
			presenceMask,
			request.ApiKey == null					? "" : request.ApiKey,
			// request.PackageIndex == null		? (int)-1 : (int) request.PackageIndex,
			request.PackageIndex,
			request.LanguageCode == null		? "EN" : request.LanguageCode,
			request.Info == null						? "" : request.Info,
			request.ClientId == null 				? "" : request.ClientId,
			// request.TestModeEnabled == null	? true : request.TestModeEnabled,
			request.TestModeEnabled,
			// request.OfflineModeEnabled == null	? true : request.OfflineModeEnabled,
			request.OfflineModeEnabled,
			// request.PendingTransactionHandlingEnabled == null ? false : request.PendingTransactionHandlingEnabled
			request.Price
		});
		
		// if there is no callback, don't wait for response and return immediately
		if (callback == null)
		{
			yield break;
		}
		
		// wait until we get a result. This variable is set in NotifyResponseReady()
		// that is called from Android Java runtime
		while (lastCentiliPaymentResponse == null) 
		{
			yield return 1; // wait 1 frame
		}
		
		// call the callback and clear the buffer automatically
		callback(lastCentiliPaymentStatus, lastCentiliPaymentResponse);
		lastCentiliPaymentResponse = null;
	}
		
	// Just ignore PENDING notification
	public void SignalPaymentPending(string voidArg0)
	{
		CentiliPaymentResponse response = new CentiliPaymentResponse(GetCurrentActivity());
		
		lastCentiliPaymentStatus = CentiliPaymentStatus.PAYMENT_PENDING;
		lastCentiliPaymentResponse = response;
	}
	
	public void SignalPaymentFailed(string voidArg0)
	{
		CentiliPaymentResponse response = new CentiliPaymentResponse(GetCurrentActivity());
		
		lastCentiliPaymentStatus = CentiliPaymentStatus.PAYMENT_FAILED;
		lastCentiliPaymentResponse = response;
	}
	
	public void SignalPaymentCanceled(string voidArg0)
	{
		CentiliPaymentResponse response = new CentiliPaymentResponse(GetCurrentActivity());
		
		lastCentiliPaymentStatus = CentiliPaymentStatus.PAYMENT_CANCELED;
		lastCentiliPaymentResponse = response;
	}
	
	public void SignalPaymentSuccessful(string voidArg0)
	{
		CentiliPaymentResponse response = new CentiliPaymentResponse(GetCurrentActivity());
		
		lastCentiliPaymentStatus = CentiliPaymentStatus.PAYMENT_SUCCESSFUL;
		lastCentiliPaymentResponse = response;
	}

	private const string SINGLETON_GAME_OBJECT_NAME = "Centili Instance";
	
	public static CentiliInternalWrapper Instance 
	{
		get 
		{
			return GetInstance();
		}
	}
	
	private static CentiliInternalWrapper _instance = null;
	
	private static CentiliInternalWrapper GetInstance() 
	{
		if (_instance == null) 
		{
			_instance = FindObjectOfType(typeof(CentiliInternalWrapper)) as CentiliInternalWrapper;
			if (_instance == null)
			{
				var gameObject = new GameObject(SINGLETON_GAME_OBJECT_NAME);
				_instance = gameObject.AddComponent<CentiliInternalWrapper>();
			}
		}
		return _instance;
	}	

	private static AndroidJavaObject GetCurrentActivity()
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	}
}