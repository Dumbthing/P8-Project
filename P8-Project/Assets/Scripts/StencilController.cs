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
            if (r.tag != "EntryPortal" && r.tag != "ExitPortal" && r.tag != "Stencil")
            {
                /// Instaniating a new material (copy of an existing)
                //r.material.shader = Shader.Find("Stencils/Materials/StencilBufferCurrent");
                /// Setting the shader of the current material across the project, meaning for all objects using that material (also prefabs)
                r.sharedMaterial.shader = Resources.Load("Standard", typeof(Shader)) as Shader;
                /// Setting the material for all objects in the loop.
                //r.sharedMaterial = Resources.Load("Materials/DefaultStencilCurrent", typeof(Material)) as Material;
            }
        }

        /// Next room set to stencil ref 2
        if (currentRoom < layout.layoutList.Count - 1)
        {
            foreach (Renderer r in layout.layoutList[currentRoom + 1].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal" && r.tag != "Stencil")
                {
                    r.sharedMaterial.shader = Shader.Find("Stencils/Materials/StencilBufferNext");
                    //r.sharedMaterial = Resources.Load("Materials/DefaultStencilNext", typeof(Material)) as Material;
                }
            }
        }

        /// Previous room set to stencil ref 1
        if (currentRoom > 0)
        {
            foreach (Renderer r in layout.layoutList[currentRoom - 1].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal" && r.tag != "Stencil")
                {
                    r.sharedMaterial.shader = Shader.Find("Stencils/Materials/StencilBufferPrevious");
                    r.sharedMaterial = Resources.Load("Materials/DefaultStencilPrevious", typeof(Material)) as Material;
                }
            }
        }
    }
}
