using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;

public class APIHelper : MonoBehaviour
{
    [SerializeField] public GameObject vmPrefab;
    private const string BaseUri = "http://127.0.0.1:8697/api/vms";
    private const string AcceptHeader = "application/vnd.vmware.vmw.rest-v1+json";
    private const string AuthorizationHeader = "Basic c2hhd25yaWp1OlNoYXduMTk5OCE=";
    public bool IsTesting { get; set; } = false;
    public UnityWebRequest WebRequest { get; private set; }

    void Start()
    {
        StartCoroutine(GetAndProcessVirtualMachines());
    }

    public IEnumerator GetAndProcessVirtualMachines()
    {
        Debug.Log("Starting GetAndProcessVirtualMachines request: " + DateTime.Now);
        var webRequest = MakeWebRequest(BaseUri);
        //AttachHeader(webRequest, "Accept", AcceptHeader);
        //AttachHeader(webRequest, "Authorization", AuthorizationHeader);

        WebRequest = webRequest;
        yield return webRequest.SendWebRequest();

        Debug.Log("Finished GetAndProcessVirtualMachines request: " + DateTime.Now);

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ERROR: " + webRequest.error);
            yield break;
        }

        if (!IsTesting)
        {
            List<VmData> vmDataList = JsonConvert.DeserializeObject<List<VmData>>(webRequest.downloadHandler.text);
            var position = -5;

            if (vmDataList == null)
            {
                Debug.LogError("Failed to deserialize VM data.");
                yield break; 
            }
            foreach (VmData vmData in vmDataList)
            {
                GameObject vm = Instantiate(vmPrefab, new Vector3(position, 0, 0), Quaternion.identity);
                yield return ProcessVmData(vmData, vm, BaseUri);

                position = position + 5;
            }
        }
      
    }

    public UnityWebRequest MakeWebRequest(string uri)
    {
        var webRequest = UnityWebRequest.Get(uri);
        AttachHeader(webRequest, "Accept", AcceptHeader);
        AttachHeader(webRequest, "Authorization", AuthorizationHeader);
        return webRequest;
    }

    IEnumerator ProcessVmData(VmData vmData, GameObject vm, string baseUri)
    {
        string id = vmData.id;
        string path = vmData.path;
        string vmDetailsUri = baseUri + "/" + id;

        yield return GetVmDetails(vm, vmDetailsUri, path);
        yield return GetVmPower(vm, vmDetailsUri, id);
        yield return InitializeVmPowerInfo(vm, id);
        yield return InitializeVmDeleteHelper(vm, id);
    }

    IEnumerator GetVmDetails(GameObject vm, string vmDetailsUri, string path)
    {

        DateTime getStartTime = DateTime.Now;
        string format = "yyyy-MM-dd HH:mm:ss.fff"; // fff for milliseconds
        string getStartTimeResult = getStartTime.ToString(format);
        Debug.Log("Starting GetVmDetails request: " + getStartTimeResult);
        var vmDetailsRequest = MakeWebRequest(vmDetailsUri);

        yield return vmDetailsRequest.SendWebRequest();

        DateTime getEndTime = DateTime.Now;
        string getEndTimeResult = getEndTime.ToString(format);
        Debug.Log("Finished GetVmDetails request: " + getEndTimeResult);


        if (vmDetailsRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ERROR: " + vmDetailsRequest.error);
            yield break;
        }

        VmDetails vmDetails = JsonConvert.DeserializeObject<VmDetails>(vmDetailsRequest.downloadHandler.text);

        string vmDetailsDisplay = GetVmDetailsDisplay(vmDetails);
        string vmName = Path.GetFileName(path);
        Canvas canvas = vm.GetComponentInChildren<Canvas>();
        canvas.GetComponentInChildren<TextMeshProUGUI>().text = vmName + "\n" + vmDetailsDisplay;

        Transform cubeTransform = vm.transform.Find("VmCube");
        if (vmDetails.memory < 5000)
        {
            cubeTransform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
        else if (vmDetails.memory > 12000)
        {
            cubeTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
    }

    string GetVmDetailsDisplay(VmDetails vmDetails)
    {
        if (vmDetails.cpu != null)
        {
            return "ID: " + vmDetails.id + "\n" + "Processors: " + vmDetails.cpu.processors
                        + "\n" + "Memory: " + vmDetails.memory;
        }
        else
        {
            return "ID: " + vmDetails.id + "\n" + "Memory: " + vmDetails.memory;
        }
    }

    IEnumerator GetVmPower(GameObject vm, string vmDetailsUri, string id)
    {
        string vmPowerUri = vmDetailsUri + "/power";

        Debug.Log("Starting GetVmPower request: " + DateTime.Now);
        var vmPowerRequest = UnityWebRequest.Get(vmPowerUri);
        AttachHeader(vmPowerRequest, "Accept", AcceptHeader);
        AttachHeader(vmPowerRequest, "Authorization", AuthorizationHeader);

        yield return vmPowerRequest.SendWebRequest();
        Debug.Log("Finished GetVmPower request: " + DateTime.Now);

        if (vmPowerRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ERROR: " + vmPowerRequest.error);
            yield break;
        }

        VmPower vmPower = JsonConvert.DeserializeObject<VmPower>(vmPowerRequest.downloadHandler.text);
        string vmPowerDisplay = vmPower.power_state;

        Renderer rend = vm.GetComponentInChildren<Renderer>();
        if (vmPowerDisplay == "poweredOn")
        {
            rend.material.color = Color.green;
        }
        else if (vmPowerDisplay == "poweredOff")
        {
            rend.material.color = Color.red;
        }
    }

    IEnumerator InitializeVmPowerInfo(GameObject vm, string id)
    {
        //get script component VmPowerInfo from prefab/gameobject created on start
        VmPowerInfo vmPowerInfo = vm.GetComponent<VmPowerInfo>();
        Renderer rend = vm.GetComponentInChildren<Renderer>();

        vmPowerInfo.vmId = id;
        vmPowerInfo.vmRenderer = rend;
        vmPowerInfo.Initialize();

        yield return null;
    }

    IEnumerator InitializeVmDeleteHelper(GameObject vm, string id)
    {
        //get script component VmDeleteHelper from prefab/gameobject created on start
        VmDeleteHelper vmDeleteHelper = vm.GetComponent<VmDeleteHelper>();
        vmDeleteHelper.vmId = id;
        vmDeleteHelper.Initialize();

        yield return null;
    }

    void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }
}
