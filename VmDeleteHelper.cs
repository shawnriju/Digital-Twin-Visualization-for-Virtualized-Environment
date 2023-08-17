using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VmDeleteHelper : MonoBehaviour
{
    public string vmId;
    public Button deleteButton;
    public UnityWebRequest WebRequest { get; private set; }
    public bool IsTesting { get; set; } = false;


    public void Initialize()
    {
        deleteButton.onClick.AddListener(HandleDeleteButtonClick);
        Debug.Log("Initialize: Number of listeners = " + deleteButton.onClick.GetPersistentEventCount());
    }

    void HandleDeleteButtonClick() // handle delete button click
    {
        StartCoroutine(DeleteVm());
    }

    //public IEnumerator DeleteVm() // coroutine to delete VM
    //{
    //    string url = "http://127.0.0.1:8697/api/vms/" + vmId;

    //    using (UnityWebRequest request = UnityWebRequest.Delete(url))
    //    {
    //        request.SetRequestHeader("Accept", "application/vnd.vmware.vmw.rest-v1+json");
    //        request.SetRequestHeader("Authorization", "Basic c2hhd25yaWp1OlNoYXduMTk5OCE=");

    //        WebRequest = request;
    //        yield return request.SendWebRequest();

    //        if (request.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("ERROR: " + request.error);
    //        }
    //        else
    //        {

    //            if (!IsTesting)
    //            {
    //                Debug.Log("VM deleted successfully.");
    //                // Delete the cube from the scene
    //                Destroy(gameObject);
    //            }
    //        }
    //    }
    //}

    public IEnumerator DeleteVm() // coroutine to delete VM
    {
        string url = "http://127.0.0.1:8697/api/vms/" + vmId;

        Debug.Log("Starting DELETE request to delete VM: " + DateTime.Now);

        UnityWebRequest request = UnityWebRequest.Delete(url);
        request.SetRequestHeader("Accept", "application/vnd.vmware.vmw.rest-v1+json");
        request.SetRequestHeader("Authorization", "Basic c2hhd25yaWp1OlNoYXduMTk5OCE=");

        WebRequest = request;
        //Debug.Log("URL inside coroutine: " + WebRequest.url);
        yield return request.SendWebRequest();

        Debug.Log("Finished DELETE request to delete VM: " + DateTime.Now);

        if (!IsTesting)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("ERROR: " + request.error);
            }
            else
            {
                Debug.Log("VM deleted successfully.");
                // Delete the cube from the scene
                Destroy(gameObject);

            }
            request.Dispose(); // Manually dispose of the request object

        }


    }
}
