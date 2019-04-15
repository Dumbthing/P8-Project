using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNextPortalPosition : MonoBehaviour
{
    GameObject[] potentialPortals;
    GameObject portal;
    Material otherWorldMaterial;

    public void UpdateActiveNextPortalPos()
    {
        potentialPortals = GameObject.FindGameObjectsWithTag("ExitPortal"); // Find all next portals
        foreach (GameObject p in potentialPortals) // Loop through each portal to check which is active
        {
            if (p.activeSelf)
                portal = p;
        }
        if (portal != null)
        {
            Transform portalRenderer = portal.GetComponent<Transform>();
            otherWorldMaterial = GetComponent<Renderer>().sharedMaterial;
            otherWorldMaterial.SetMatrix("_WorldToPortal", portalRenderer.worldToLocalMatrix);
            Debug.Log("Set Next portal pos to: " + portalRenderer.name);
        }
    }
}
