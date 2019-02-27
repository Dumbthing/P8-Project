using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

    Transform playerCamera;
    Transform entrance;
    public Transform destination;
    


    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
        entrance = transform.parent;
    }
    
    void LateUpdate () {

        //Vector3 pos = destination.transform.InverseTransformPoint(playerCamera.position);
        //transform.localPosition = new Vector3(-pos.x, transform.localPosition.y, -pos.z);
        Vector3 playerOffsetFromPortal = playerCamera.position - entrance.position; //offset 
        transform.position = destination.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(destination.rotation, entrance.rotation);
        angularDifferenceBetweenPortalRotations += 180f; //Our offset was wrong when we changed parents. So might have to delete this if we get errors later. 

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameradirection = (portalRotationalDifference * playerCamera.forward);
        transform.rotation = Quaternion.LookRotation(newCameradirection, Vector3.up);


    }
}
