using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        Vector3 toTarget = (other.gameObject.transform.position - transform.position).normalized;

        if (Vector3.Dot(toTarget, gameObject.transform.forward) > 0)
        {
            Debug.Log("from front");
        }
        else
        {
            Debug.Log("from back");
        }
    }


}
