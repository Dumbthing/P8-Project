using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

    Transform playerCamera;
    public Transform destination;
    public Transform entrance;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void LateUpdate () {

        //Vector3 pos = destination.transform.InverseTransformPoint(playerCamera.position);
        //transform.localPosition = new Vector3(-pos.x, transform.localPosition.y, -pos.z);
        Vector3 playerOffsetFromPortal = playerCamera.position - entrance.position; //offset 
        transform.position = destination.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(destination.rotation, entrance.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameradirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameradirection, Vector3.up);


    }
}
