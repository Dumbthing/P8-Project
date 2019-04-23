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

    
    // Start is called before the first frame update
    void Start()
    {
        portalCol = portalToUnlock.GetComponent<BoxCollider>();
        CircularDrive_ = GetComponent<CircularDrive>();
        rotationStart = transform.localEulerAngles.y;
		//Debug.Log(rotationStart);

		if(portalCol.enabled) {
			portalCol.enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Y Rotation: " + transform.localEulerAngles.y + ", while rotation start was: " + rotationStart + ", and the threshold is: " + doorThreshold);
        if(!portalCol.enabled && (transform.localEulerAngles.y - rotationStart > doorThreshold || transform.localEulerAngles.y - rotationStart < -doorThreshold)) {
            portalCol.enabled = true;
        //    Debug.Log("Portal Enabled");
        }
        else if(portalCol.enabled && transform.localEulerAngles.y - rotationStart < doorThreshold && transform.localEulerAngles.y - rotationStart > -doorThreshold) {
            portalCol.enabled = false;
        //    Debug.Log("Portal Disabled");
        }

    } 
}