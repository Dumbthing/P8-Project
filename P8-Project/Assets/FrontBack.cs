using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBack : MonoBehaviour
{
    Vector3 toTarget;
    Vector3 fromTarget;
    bool once = false;
    float offset;


    

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
        if (!once)
        {
            Debug.Log("IN");
            toTarget = (other.gameObject.transform.position);
            once = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("x") || other.name.Contains("X"))
        {
            if (once)
            {
                fromTarget = other.gameObject.transform.position;
                offset = toTarget.x - fromTarget.x;
                //Debug.Log(offset); // Enable this to debug the Offset and adjust accordingly below
                if (offset >= 0.3f || offset <= -0.3f) //Might have to adjust this depending on our Collider + Player size. 
                {
                    Debug.Log("New Way");
                }
                else
                {
                    Debug.Log("Same way");
                }
            }
        }

        if (other.name.Contains("z") || other.name.Contains("Z"))
        {
            if (once)
            {
                fromTarget = other.gameObject.transform.position;
                offset = toTarget.z - fromTarget.z;
                //Debug.Log(offset); // Enable this to debug the Offset and adjust accordingly below
                if (offset >= 0.3f || offset <= -0.3f) //Might have to adjust this depending on our Collider + Player size. 
                {
                    Debug.Log("New Way");
                }
                else
                {
                    Debug.Log("Same way");
                }
            }
        }
        once = false;
    }

}
