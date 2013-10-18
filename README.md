# Centili Mobile Payments Unity 3D Plugin
This project is Unity3d ([Unity3d](http://unity3d.com/unity "Unity3d")) package which can be merged with your Unity3d project and enable you to use Centili Mobile Payments system.

## Step By Step integration
1. Extract our [.unitypackage](http://download.com "Download CentiliUnity.unitypackage") to your project.
2. In your code make new _CentiliPaymentRequest_ object (with _ApiKey_ as only mandatory field).
3. Call static method _CentiliPaymentManager.MakePayment(centiliPaymentRequest, callback)_ where _callback_ is your _void_ method that receives _CentiliPaymentStatus_ and _CentiliPaymentResponse_ as params.
4. Your callback method will be invoked upon completing payment request. All you have to do is handle payment result in your application (_CentiliPaymentStatus_ can be "canceled", "successful" or "failed"; additional info is provided by _CentiliPaymentResponse_).