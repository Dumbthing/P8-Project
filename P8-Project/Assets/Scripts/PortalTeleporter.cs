using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {
    
    
    Material material;
    Camera portalCamera;
    Transform player, playerCamera, entrance, reciever;
    public Transform destination; // Other portal parents, need new method for finding destination procedurally

    private bool playerIsOverlapping = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
        entrance = transform;
        //reciever.position = transform.position + new Vector3 (5f, 0f, 0f);
        
        reciever = destination.GetComponent<Collider>().transform;

        portalCamera = GetComponentInChildren<Camera>();
        //GetComponent<Renderer>().material = new Material(Shader.Find("ScreenCutoutShader"));
        material = GetComponent<Renderer>().material;


        if (portalCamera.targetTexture != null)
        {
            portalCamera.targetTexture.Release();
        }

        portalCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.mainTexture = portalCamera.targetTexture;
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
                player.position = reciever.position;

                playerIsOverlapping = false;
            }
        }
    }

    void LateUpdate() // Camera update loop
    {
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
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}