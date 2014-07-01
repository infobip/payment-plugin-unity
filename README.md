# Centili in-app Payment Plugin for Unity
*only for Android platform*

This project is a Unity3d ([Unity3d](http://unity3d.com/unity "Unity3d")) package which can be merged with your Unity3d project and enables you to use the Centili Mobile Payments system.

## Step By Step integration
1. Extract our [.unitypackage](http://www.centili.com/manual/unity3d/CentiliUnityPrime31.unitypackage "Download CentiliUnity.unitypackage") to your project.
2. Create a new _CentiliPaymentRequest_ object in your code (with _ApiKey_ as the only mandatory field).
 
	```
	CentiliPaymentRequest request = new CentiliPaymentRequest("your-api-key-abc123abc123")
	{
		PackageIndex = 1, /* Preselecting package with index 1 */
		ClientId = this.MyUserId
	};
	```

3. Call static method _CentiliPaymentManager.MakePayment(centiliPaymentRequest, call-back)_ where _call-back_ is your _void_ method that receives _CentiliPaymentStatus_ and _CentiliPaymentResponse_ as params.

        CentiliPaymentManager.MakePayment(request, OnPaymentFinished);

4. Your call-back method will be invoked upon completing the payment request. All you have to do is handle the payment result in your application (_CentiliPaymentStatus_ can be "cancelled", "successful" or "failed"; additional info is provided by _CentiliPaymentResponse_).

	```
	void OnPaymentFinished(CentiliPaymentStatus status, CentiliPaymentResponse response)
	{
		if (CentiliPaymentStatus.PAYMENT_SUCCESSFUL.Equals(status))
	    {
	    	this.Users.FindById(response.ClientId).AddCredit(response.ItemAmount);
		}
	}
	```

## Additional methods

- You can get and set *CentiliPaymentManager*.*DebugMode* to 'true' or 'false' to get our logger output debug data. Defaults to 'false'.
- You can also set or un-set *CentiliPaymentManager*.*PendingTransactionHandling*, which will influence whether the pending payment continues when a new payment request is sent, or we will start a new payment request. Default is 'true', which means will try to resume the unresolved transaction by default.

Owners
------

Framework Integration Team @ Infobip Belgrade, Serbia

*Android is a trademark of Google Inc.*

© 2013-2014, Infobip Ltd.
