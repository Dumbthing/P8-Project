using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

    Transform player;
    Transform playerCamera;
    GameObject portalCamera;
    Transform entrance; // Portal parent
    Transform reciever;

    public Transform destination; // Other portal parents

    private bool playerIsOverlapping = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
        entrance = transform.parent;
        portalCamera = entrance.gameObject.GetComponentInChildren<Camera>().gameObject;

        reciever = destination.GetComponentInChildren<Collider>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f)
            {
                // Teleport him!
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = reciever.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
    }

    void LateUpdate() // Camera related
    {
        //Vector3 pos = destination.transform.InverseTransformPoint(playerCamera.position);
        //transform.localPosition = new Vector3(-pos.x, transform.localPosition.y, -pos.z);
        Vector3 playerOffsetFromPortal = playerCamera.position - entrance.position; //offset 
        portalCamera.transform.position = destination.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(destination.rotation, entrance.rotation);
        angularDifferenceBetweenPortalRotations += 180f; //Our offset was wrong when we changed parents. So might have to delete this if we get errors later. 

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameradirection = (portalRotationalDifference * playerCamera.forward);
        portalCamera.transform.rotation = Quaternion.LookRotation(newCameradirection, Vector3.up);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Enter collision detected");
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Exit collision detected");
            playerIsOverlapping = false;
        }
    }
}