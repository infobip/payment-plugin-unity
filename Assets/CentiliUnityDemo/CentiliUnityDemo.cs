using UnityEngine;
using System.Collections;
using CentiliUnity;

public class CentiliUnityDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI()
	{
	    if (GUILayout.Button("Test Centili Unity Payment"))
	    {
            print("Request for payment sent.");
			var paymentRequest = new CentiliPaymentRequest("28550ec26491d4ed1b1de6fd3fe2b92a");
			paymentRequest.ClientId = "test";
			CentiliPaymentManager.DebugMode = true;
			CentiliPaymentManager.MakePayment(paymentRequest, CentiliCallback);
	    }
	}

	private void CentiliCallback(CentiliPaymentStatus status, CentiliPaymentResponse response)
    {
        print(status);
        print(response.ItemAmount);
    }
}
