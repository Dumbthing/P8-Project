using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilSwitch : MonoBehaviour
{
    GameObject room;
    Renderer rend;
    Material newMat;
    // Start is called before the first frame update
    void Start()
    {
        room = GameObject.FindGameObjectWithTag("Room1_2");

        foreach (Renderer r in room.GetComponentsInChildren<Renderer>())
        {
            r.material = new Material(newMat);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       
        foreach (Renderer r in room.GetComponentsInChildren<Renderer>())
        {
            r.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");
        }
       
        
        //room.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");
        //for (int i = 0; i < room.transform.childCount; i++)
       
        //    Transform child = room.transform.GetChild(i);
        //    child.GetComponent<Renderer>().material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");

        //    //rend.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");

        //}
    }
}   
