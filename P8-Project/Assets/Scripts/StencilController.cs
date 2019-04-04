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
        
        /// Current room set to stencil ref 2
        foreach (Renderer r in layout.layoutList[currentRoom].GetComponentsInChildren<Renderer>())
        {
            if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
            {
                r.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Equal");
            }
        }

        /// Next room set to stencil ref 3
        if (currentRoom < layout.layoutList.Count - 1)
        {
            foreach (Renderer r in layout.layoutList[currentRoom + 1].GetComponentsInChildren<Renderer>())
            {
                if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
                {
                    r.material.shader = Shader.Find("Stencils/Portal_3/Diffuse-Equal");
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
                    r.material.shader = Shader.Find("Stencils/Portal_1/Diffuse-Equal");
                }
            }
        }

        //for (int i = 0; i < goArray.Length; i++)
        //{
        //    if (currentRoom > 0)
        //    {
        //        if (goArray[i].layer == layout.layoutList[currentRoom - 1].layer) // Previous (Portal1)
        //        {
        //            foreach (Renderer r in goArray[i].GetComponentsInChildren<Renderer>())
        //            {
        //                if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
        //                {
        //                    r.material.shader = Shader.Find("Stencils/Portal_1/Diffuse-Equal");
        //                }
        //        }
        //    }
        //    if (goArray[i].layer == layout.layoutList[currentRoom].layer) // Current (Portal2)
        //    {
        //        foreach (Renderer r in goArray[i].GetComponentsInChildren<Renderer>())
        //        {
        //            if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
        //            {
        //                r.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Equal");
        //            }
        //        }
        //    }
        //    if (currentRoom < layout.layoutList.Count - 1)
        //    {
        //        if (goArray[i].layer == layout.layoutList[currentRoom + 1].layer) // Next (Portal3)
        //        {
        //            foreach (Renderer r in goArray[i].GetComponentsInChildren<Renderer>())
        //            {
        //                if (r.tag != "EntryPortal" && r.tag != "ExitPortal")
        //                {
        //                    r.material.shader = Shader.Find("Stencils/Portal_3/Diffuse-Equal");
        //                }
        //            }
        //        }
        //    }
    }
}
