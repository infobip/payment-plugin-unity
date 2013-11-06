# Centili Mobile Payments Unity 3D Plugin
This project is Unity3d ([Unity3d](http://unity3d.com/unity "Unity3d")) package which can be merged with your Unity3d project and enable you to use Centili Mobile Payments system.

## Step By Step integration
1. Extract our [.unitypackage](http://www.centili.com/manual/unity3d/CentiliUnity.unitypackage "Download CentiliUnity.unitypackage") to your project.
2. In your code make new _CentiliPaymentRequest_ object (with _ApiKey_ as only mandatory field).
 
	```
	CentiliPaymentRequest request = new CentiliPaymentRequest("your-api-key-abc123abc123")
	{
		PackageIndex = 1, /* Preselecting package with index 1 */
		ClientId = this.MyUserId
	};
	```

3. Call static method _CentiliPaymentManager.MakePayment(centiliPaymentRequest, callback)_ where _callback_ is your _void_ method that receives _CentiliPaymentStatus_ and _CentiliPaymentResponse_ as params.

	```
	CentiliPaymentManager.MakePayment(request, OnPaymentFinished);
	```

4. Your callback method will be invoked upon completing payment request. All you have to do is handle payment result in your application (_CentiliPaymentStatus_ can be "canceled", "successful" or "failed"; additional info is provided by _CentiliPaymentResponse_).

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

- You can get and set *CentiliPaymentManager*.*DebugMode* to true or false to get our logger output debug data. Defaults to false.
- You can also set or unset *CentiliPaymentManager*.*PendingTransactionHandling*, which will influence whether will we continue pending payment when new payment request is sent, or will we start a new payment request. Default is true, which means that we will try to resume unresolved transaction.

## Owners
Framework Integration Team
