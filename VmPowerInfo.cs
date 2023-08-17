using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VmPowerInfo : MonoBehaviour
{
    public string vmId;
    public bool isPoweredOn;
    public Button powerButton;
    public Renderer vmRenderer;

    public UnityWebRequest WebRequest { get; private set; }
    public bool IsTesting { get; set; } = false;

    // Start is called before the first frame update
    public void Initialize()
    {
        powerButton.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        StartCoroutine(SendPutRequest());
    }

    //public IEnumerator SendPutRequest()
    //{
    //    // Prepare the PUT request
    //    string powerStatus = isPoweredOn ? "off" : "on";
    //    string url = "http://127.0.0.1:8697/api/vms/" + vmId + "/power";
    //    Debug.Log(url);

    //    string json = powerStatus;
    //    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

    //    var webRequest = new UnityWebRequest(url, "PUT");

    //    using (UnityWebRequest request = UnityWebRequest.Put(url, bodyRaw))
    //    {
    //        // Set headers
    //        request.SetRequestHeader("Content-Type", "application/vnd.vmware.vmw.rest-v1+json");
    //        request.SetRequestHeader("Accept", "application/vnd.vmware.vmw.rest-v1+json");
    //        request.SetRequestHeader("Authorization", "Basic c2hhd25yaWp1OlNoYXduMTk5OCE=");

    //        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

    //        WebRequest = request;
    //        // Send the request
    //        yield return request.SendWebRequest();

    //        if (!IsTesting)
    //        {
    //            // Handle the response
    //            if (request.result != UnityWebRequest.Result.Success)
    //            {
    //                Debug.Log(request.error);
    //            }
    //            else
    //            {
    //                // Update the power status
    //                isPoweredOn = !isPoweredOn;
    //                vmRenderer.material.color = isPoweredOn ? Color.green : Color.red;
    //                Debug.Log("VM power status toggled successfully. New status: " + (isPoweredOn ? "On" : "Off"));
    //            }
    //        }

    //    }
    //}


    public IEnumerator SendPutRequest()
    {
        // Prepare the PUT request
        string powerStatus = isPoweredOn ? "off" : "on";
        string url = "http://127.0.0.1:8697/api/vms/" + vmId + "/power";

        string json = powerStatus;
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        Debug.Log("Starting PUT request to change power: " + DateTime.Now);
        WebRequest = UnityWebRequest.Put(url, bodyRaw);

        // Set headers
        WebRequest.SetRequestHeader("Content-Type", "application/vnd.vmware.vmw.rest-v1+json");
        WebRequest.SetRequestHeader("Accept", "application/vnd.vmware.vmw.rest-v1+json");
        WebRequest.SetRequestHeader("Authorization", "Basic c2hhd25yaWp1OlNoYXduMTk5OCE=");

        WebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Send the request
        yield return WebRequest.SendWebRequest();

        Debug.Log("Finished PUT request to change power: " + DateTime.Now);

        if (!IsTesting)
        {
            // Handle the response
            if (WebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(WebRequest.error);
            }
            else
            {
                // Update the power status
                isPoweredOn = !isPoweredOn;
                vmRenderer.material.color = isPoweredOn ? Color.green : Color.red;
                Debug.Log("VM power status toggled successfully. New status: " + (isPoweredOn ? "On" : "Off"));
            }
        }
    }
}
