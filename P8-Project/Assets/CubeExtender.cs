using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeExtender : MonoBehaviour
{
    Vector3 orgScale;
    Vector3 orgPosi;
    bool doOnce = false;
    Mesh mesh;
    int[] orgMesh;
    float offsetToPortal;
    Vector3 offsetPortal;

    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;

    public GameObject Portal;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!doOnce)
        {
            Mesh mesh = Portal.GetComponent<MeshFilter>().mesh;
            orgMesh = mesh.triangles;
            mesh.triangles = mesh.triangles.Reverse().ToArray();

            orgScale = Portal.transform.localScale;
            orgPosi = Portal.transform.localPosition;
            doOnce = true;
        }
    }


    void OnTriggerStay(Collider other)
    {
        scaleCube();

    }
    private void OnTriggerExit(Collider other)
    {

        Portal.transform.localScale = orgScale;
        Portal.transform.localPosition = orgPosi;

        //mesh.triangles = orgMesh;

        doOnce = false;
    }



    private void scaleCube()
    {
        //offsetToPortal = transform.position.z - Portal.transform.position.z;
        offsetPortal = Portal.transform.position - transform.position;

        if (Portal.transform.eulerAngles.y == zeroF || Portal.transform.eulerAngles.y == oneEightyF) //Stretch on Z
        {
            if (Portal.transform.localScale.z <= 10)
            {
                Portal.transform.localScale += new Vector3(Portal.transform.localScale.x, Portal.transform.localScale.y, transform.localPosition.z+1);
                //Portal.transform.localScale += new Vector3(0, 0, 10);
                //Portal.transform.localPosition += new Vector3(0, 0, 5);
            }
        }

        if (Portal.transform.eulerAngles.y == ninetyF || Portal.transform.eulerAngles.y == twoSeventyF) //Stretch on X
        {
            if (transform.localScale.x <= 10)
            {
                Portal.transform.localScale += new Vector3(transform.localPosition.x+1, Portal.transform.localScale.y, Portal.transform.localScale.z);

                //Portal.transform.localScale += new Vector3(10, 0, 0);
                //Portal.transform.localPosition += new Vector3(5, 0, 0);
            }
        }
    }
}
