using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreviousPortalPosition : MonoBehaviour
{
    GameObject[] potentialPortals;
    GameObject portal;
    Material otherWorldMaterial;

    public void UpdateActivePreviousPortalPos()
    {
        potentialPortals = GameObject.FindGameObjectsWithTag("EntryPortal"); // Find all next portals
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
        }
    }
}
