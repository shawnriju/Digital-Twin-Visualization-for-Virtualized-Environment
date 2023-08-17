using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class GetWebRequestTest
{
    private const string BaseUri = "http://127.0.0.1:8697/api/vms";
    private const string TestId = "O3K6N91OE39EVNLI21DFIKS10CEVTCAQ";
    // A Test behaves as an ordinary method
    [Test]
    public void TestMakeWebRequest()
    {
        // Create an instance of the APIHelper component
        GameObject obj = new GameObject();
        APIHelper apiHelper = obj.AddComponent<APIHelper>();

        // Call the method to create the web request
        UnityWebRequest webRequest = apiHelper.MakeWebRequest(BaseUri);

        // Check that the web request is properly configured
        Assert.AreEqual(BaseUri, webRequest.url); 
        Assert.AreEqual("application/vnd.vmware.vmw.rest-v1+json", webRequest.GetRequestHeader("Accept")); 
    
    }
}
