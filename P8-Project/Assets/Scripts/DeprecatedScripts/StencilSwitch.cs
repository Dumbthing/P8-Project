using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilSwitch : MonoBehaviour
{
    GameObject room;
    GameObject room2;
    Renderer rend;
    Material newMat;
    Camera PreviousCam;
    Camera CurrentCam;
    Camera NextCam;

    // Start is called before the first frame update
    void Start()
    {

        room = GameObject.FindGameObjectWithTag("Room1_2");
        room2 = GameObject.FindGameObjectWithTag("Room3_2");
     


    }



    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {


        Vector3 toTarget = (other.gameObject.transform.position - transform.position).normalized;

        if (Vector3.Dot(toTarget, gameObject.transform.forward) > 0)
        {

            foreach (Renderer r in room2.GetComponentsInChildren<Renderer>())
            {
                r.material.shader = Shader.Find("Stencils/Portal_1/Diffuse-Always");

            }

            foreach (Renderer f in room.GetComponentsInChildren<Renderer>())
            {
                f.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Equal");
            }
        }
        else
        {
            //Enter from front
            
            foreach (Renderer t in room.GetComponentsInChildren<Renderer>())
            {
                t.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");
            }

            foreach (Renderer g in room2.GetComponentsInChildren<Renderer>())
            {
                g.material.shader = Shader.Find("Stencils/Portal_1/Diffuse-Equal");
            }
        }



        
       
        
        //room.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");
        //for (int i = 0; i < room.transform.childCount; i++)
       
        //    Transform child = room.transform.GetChild(i);
        //    child.GetComponent<Renderer>().material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");

        //    //rend.material.shader = Shader.Find("Stencils/Portal_2/Diffuse-Always");

        //}
    }
}   
