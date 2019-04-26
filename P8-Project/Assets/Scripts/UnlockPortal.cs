using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UnlockPortal : MonoBehaviour
{
    Collider portalCol;
    public GameObject portalToUnlock;
    CircularDrive CircularDrive_;
    float rotationStart;
	public float  doorThreshold = 40;
    Renderer rend;
    Interactable interactGO;
    
    string currentChecker = "_Current";
    //string currentChecker2 = "GateFrame_Current";
    // Start is called before the first frame update
    void Start()
    {   
        rend = GetComponentInChildren<Renderer>();
        interactGO = GetComponent<Interactable>();
/*         portalCol = portalToUnlock.GetComponent<BoxCollider>();
        CircularDrive_ = GetComponent<CircularDrive>();
        rotationStart = transform.localEulerAngles.y;
		//Debug.Log(rotationStart);
        

		if(portalCol.enabled) {
			portalCol.enabled = false;
		} */
    }

    // Update is called once per frame
    void Update()
    {       
        // Could use the OnTriggerExit of a portal to start this process. 
        /* if(gameObject.transform.root.gameObject.activeInHierarchy) {} */
        if(!rend.material.name.Contains("_Current") && interactGO.enabled) {
            interactGO.enabled = false;
            Debug.Log("Current material shader is:" + rend.material.name + "Child found is: " + rend + "Has been disabled");
        } else if(rend.material.name.Contains("_Current") && !interactGO.enabled) {
            interactGO.enabled = true;
            Debug.Log("Current material shader is:" + rend.material.name + "Child found is: " + rend + "Has been enabled");
        } 
/*         // Debug.Log("Y Rotation: " + transform.localEulerAngles.y + ", while rotation start was: " + rotationStart + ", and the threshold is: " + doorThreshold);
        if(!portalCol.enabled && (transform.localEulerAngles.y - rotationStart > doorThreshold || transform.localEulerAngles.y - rotationStart < -doorThreshold)) {
            portalCol.enabled = true;
        //    Debug.Log("Portal Enabled");
        }
        else if(portalCol.enabled && transform.localEulerAngles.y - rotationStart < doorThreshold && transform.localEulerAngles.y - rotationStart > -doorThreshold) {
            portalCol.enabled = false;
        //    Debug.Log("Portal Disabled");
        } */



    } 
}