using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilController : MonoBehaviour
{
    public ProceduralLayoutGeneration layout;   // Assign gameobject with this script in the inspector
    void Start()
    {
        SetStencilShader(0);
    }

    public void SetStencilShader(int currentRoom)
    {
        /// Set materials in next and previous rooms to be visible through stencil mask only
        GameObject[] goArray = GameObject.FindGameObjectsWithTag("Room");
        int layer = layout.layoutList[currentRoom].layer;
        
        /// Current room set to stencil ref 0
        foreach (Renderer r in layout.layoutList[currentRoom].GetComponentsInChildren<Renderer>())
        {
            if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
            {
                //r.material.shader = Shader.Find("Stencils/Material/StencilBufferCurrent");
                r.material.shader = Shader.Find("Standard");
            }
        }

        /// Next room set to stencil ref 2
        if (currentRoom < layout.layoutList.Count - 1)
        {
            foreach (Renderer r in layout.layoutList[currentRoom + 1].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
                {
                    r.material.shader = Shader.Find("Stencils/Material/StencilBufferNext");
                }
            }
        }

        /// Previous room set to stencil ref 1
        if (currentRoom > 0)
        {
            foreach (Renderer r in layout.layoutList[currentRoom - 1].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
                {
                    r.material.shader = Shader.Find("Stencils/Materials/StencilBufferPrevious");
                }
            }
        }
    }
}
