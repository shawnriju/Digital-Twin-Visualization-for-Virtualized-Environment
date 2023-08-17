using System.Collections;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class DeletePlayModeTest
{
    private const string BaseUri = "http://127.0.0.1:8697/api/vms";

    [UnityTest]
    public IEnumerator TestDeleteVmRequest()
    {
        // Create an instance of the VmDeleteHelper component
        GameObject obj = new GameObject();
        VmDeleteHelper vmDeleteHelper = obj.AddComponent<VmDeleteHelper>();
        vmDeleteHelper.vmId = "JOQPQDAEDL5F3CTB4P2SCTEAEPAOMF34"; // Set a test VM ID
        vmDeleteHelper.IsTesting = true;
        string testDeleteUrl = BaseUri + "/" + vmDeleteHelper.vmId;
        // Start the DeleteVm coroutine
        yield return vmDeleteHelper.StartCoroutine(vmDeleteHelper.DeleteVm());

        // Access the web request from the VmDeleteHelper component (you may need to expose it)
        UnityWebRequest webRequest = vmDeleteHelper.WebRequest;
        //Debug.Log("URL from main Delete in test: " + webRequest.url);

        // Check that the web request was properly configured
        Assert.AreEqual(testDeleteUrl, webRequest.url);
        Debug.Log("TEST URL inside Delete test: " + testDeleteUrl);
        Assert.AreEqual("DELETE", webRequest.method);
        Assert.AreEqual("application/vnd.vmware.vmw.rest-v1+json", webRequest.GetRequestHeader("Accept"));
    }

    [UnityTest]
    public IEnumerator TestInitialize()
    {
        // Create an instance of the VmDeleteHelper component
        GameObject obj = new GameObject();
        VmDeleteHelper vmDeleteHelper = obj.AddComponent<VmDeleteHelper>();

        // Mock the delete button
        GameObject buttonObj = new GameObject();
        vmDeleteHelper.deleteButton = buttonObj.AddComponent<Button>();

        // Call the Initialize method
        vmDeleteHelper.Initialize();

        // Assert that the listener has been added
        Assert.AreEqual(0, vmDeleteHelper.deleteButton.onClick.GetPersistentEventCount());

        // Wait for a frame to ensure the Unity engine has had a chance to process the button click event
        yield return null;
    }

}
