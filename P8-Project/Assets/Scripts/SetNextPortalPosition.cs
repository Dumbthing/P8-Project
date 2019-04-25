using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNextPortalPosition : MonoBehaviour
{
    GameObject[] potentialPortals;
    GameObject portal;
    Material[] otherWorldMaterial;

    private void Update()
    {
        potentialPortals = GameObject.FindGameObjectsWithTag("ExitPortal"); // Find all next portals
        FindStencilPos("MidStencil"); // For forward facing stencils
        if (portal != null)
        {
            Transform portalRenderer = portal.GetComponent<Transform>();

            otherWorldMaterial = GetComponent<Renderer>().sharedMaterials;
            foreach (Material m in otherWorldMaterial)
            {
                m.SetMatrix("_WorldToPortal", portalRenderer.worldToLocalMatrix);
            }
        }
        potentialPortals = GameObject.FindGameObjectsWithTag("EntryPortal"); // Find all previous portals
        FindStencilPos("OppositeMidStencil"); // For bacward facing stencils
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

    private void FindStencilPos(string stencilTag)
    {
        foreach (GameObject p in potentialPortals) // Loop through each portal to check which is active
        {
            for (int i = 0; i < p.transform.childCount; i++)
            {
                Transform child = p.transform.GetChild(i);
                if (child.name == stencilTag)
                {
                    portal = child.gameObject;
                    return;
                }
            }
            // Using tag
            if (p.name == stencilTag)
            portal = p;
        }
    }
}
