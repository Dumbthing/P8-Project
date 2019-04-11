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


    GameObject player;
    private float portalCubeScale = 1f;
    Vector3 originalPos;

    private float zeroF = 0.0f, ninetyF = 90.0f, oneEightyF = 180.0f, twoSeventyF = 270.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!doOnce)
        {
            originalPos = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            Mesh mesh = other.GetComponent<MeshFilter>().mesh;
            orgMesh = mesh.triangles;
            mesh.triangles = mesh.triangles.Reverse().ToArray();

            orgScale = other.transform.localScale;
            orgPosi = other.transform.localPosition;
            doOnce = true;
        }
    }


    void OnTriggerStay(Collider other)
    {
        BoxCollider staticCollider = other.GetComponent<BoxCollider>();
        ////offsetToPortal = transform.position.z - other.transform.position.z;
        //offsetPortal = other.transform.position - transform.position;

        //if (other.transform.eulerAngles.y == zeroF || other.transform.eulerAngles.y == oneEightyF) //Stretch on Z
        //{
        //    if (other.transform.localScale.z <= 10)
        //    {
        //        other.transform.localScale += new Vector3(other.transform.localScale.x, other.transform.localScale.y, transform.localPosition.z + 1);
        //        //other.transform.localScale += new Vector3(0, 0, 10);
        //        //other.transform.localPosition += new Vector3(0, 0, 5);
        //    }
        //}

        //if (other.transform.eulerAngles.y == ninetyF || other.transform.eulerAngles.y == twoSeventyF) //Stretch on X
        //{
        //    if (transform.localScale.x <= 10)
        //    {
        //        other.transform.localScale += new Vector3(transform.localPosition.x + 1, other.transform.localScale.y, other.transform.localScale.z);

        //        //other.transform.localScale += new Vector3(10, 0, 0);
        //        //other.transform.localPosition += new Vector3(5, 0, 0);
        //    }
        //}


        if (transform.localEulerAngles.y == 90f || transform.localEulerAngles.y == 270f) // x axis
        {
            Debug.Log("Inside x portal");
            other.transform.position = new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z);
            other.transform.localScale = new Vector3(portalCubeScale, other.transform.localScale.y, other.transform.localScale.z);
            staticCollider.size = new Vector3(0.01f, 1, 1);
            staticCollider.center = new Vector3(originalPos.x - other.transform.localPosition.x, staticCollider.center.y, staticCollider.center.z);
        }
        else // z axis 
        {
            Debug.Log("Inside z portal");
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
            other.transform.localScale = new Vector3(other.transform.localScale.x, other.transform.localScale.y, portalCubeScale);
            staticCollider.size = new Vector3(1, 1, 0.01f);
            staticCollider.center = new Vector3(staticCollider.center.x, staticCollider.center.y, originalPos.z - other.transform.localPosition.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.localScale = orgScale;
        other.transform.localPosition = orgPosi;

        mesh.triangles = orgMesh;
        doOnce = false;
    }
}
