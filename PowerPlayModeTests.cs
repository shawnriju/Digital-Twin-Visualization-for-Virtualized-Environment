using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class PowerPlayModeTests
{
    private const string BaseUri = "http://127.0.0.1:8697/api/vms/";
    private const string TestId = "O3K6N91OE39EVNLI21DFIKS10CEVTCAQ";

    [UnityTest]
    public IEnumerator TestPutRequestConfiguration()
    {
        // Create an instance of the VmPowerInfo component
        GameObject obj = new GameObject();
        VmPowerInfo vmPowerInfo = obj.AddComponent<VmPowerInfo>();
        vmPowerInfo.vmId = "O3K6N91OE39EVNLI21DFIKS10CEVTCAQ";
        vmPowerInfo.isPoweredOn = false; // Set initial power status

        vmPowerInfo.IsTesting = true;
        // Start the SendPutRequest coroutine
        yield return vmPowerInfo.StartCoroutine(vmPowerInfo.SendPutRequest());

        // Access the web request (you may need to expose it)
        UnityWebRequest webRequest = vmPowerInfo.WebRequest;
        string powerTestUrl = BaseUri + TestId + "/power";
        // Check that the web request was properly configured
        Assert.AreEqual(powerTestUrl, webRequest.url);
        Assert.AreEqual("PUT", webRequest.method);
        Assert.AreEqual("application/vnd.vmware.vmw.rest-v1+json", webRequest.GetRequestHeader("Content-Type"));
        // Add more assertions as needed
    }

    [UnityTest]
    public IEnumerator TestTogglingPowerStatus()
    {
        // Create an instance of the VmPowerInfo component
        GameObject obj = new GameObject();
        VmPowerInfo vmPowerInfo = obj.AddComponent<VmPowerInfo>();
        vmPowerInfo.vmId = "O3K6N91OE39EVNLI21DFIKS10CEVTCAQ";
        vmPowerInfo.isPoweredOn = false; // Set initial power status

        // Mock the renderer
        GameObject rendererObj = new GameObject();
        vmPowerInfo.vmRenderer = rendererObj.AddComponent<MeshRenderer>();

        // Start the SendPutRequest coroutine
        yield return vmPowerInfo.StartCoroutine(vmPowerInfo.SendPutRequest());

        // Check that the power status was toggled (you may need to mock the request response)
        Assert.AreEqual(true, vmPowerInfo.isPoweredOn);
        // Add more assertions as needed
    }

}
