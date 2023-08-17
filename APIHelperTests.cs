using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

public class APIHelperTests
{
  
    [UnityTest]
    public IEnumerator TestWebRequestResponse()
    {
        // Create an instance of the APIHelper component
        GameObject obj = new GameObject();
        APIHelper apiHelper = obj.AddComponent<APIHelper>();

        // Create a mock prefab or assign a test prefab
        apiHelper.vmPrefab = new GameObject("TestPrefab");
        apiHelper.IsTesting = true;

        // Start the coroutine
        yield return apiHelper.StartCoroutine(apiHelper.GetAndProcessVirtualMachines());

        // Access the web request from the APIHelper component
        UnityWebRequest webRequest = apiHelper.WebRequest;

        // Check that the web request was successful
        Assert.AreEqual(UnityWebRequest.Result.Success, webRequest.result);
    }
}
