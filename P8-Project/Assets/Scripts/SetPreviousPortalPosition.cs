using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreviousPortalPosition : MonoBehaviour
{
    GameObject[] potentialPortals;
    GameObject portal;
    Material[] otherWorldMaterial;

    private void Update()
    {
        UpdateActivePreviousPortalPos();
    }

    public void UpdateActivePreviousPortalPos()
    {
        potentialPortals = GameObject.FindGameObjectsWithTag("EntryPortal"); // Find all next portals
        FindMidStencilPos();
        if (portal != null)
        {
            Transform portalRenderer = portal.GetComponent<Transform>();

            otherWorldMaterial = GetComponent<Renderer>().sharedMaterials;
            foreach (Material m in otherWorldMaterial)
            {
                m.SetMatrix("_WorldToPortal", portalRenderer.worldToLocalMatrix);
            }
        }
    }

    private void FindMidStencilPos()
    {
        foreach (GameObject p in potentialPortals) // Loop through each portal to check which is active
        {
            for (int i = 0; i < p.transform.childCount; i++)
            {
                Transform child = p.transform.GetChild(i);
                if (child.name == "MidStencil")
                {
                    portal = child.gameObject;
                    return;
                }
            }
            // Using tag
            if (p.name == "MidStencil")
                portal = p;
        }
    }
}
