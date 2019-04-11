using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour
{
    GameObject player;
    private float portalCubeScale = 1f;
    BoxCollider staticCollider;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        staticCollider = GetComponent<BoxCollider>();
        originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localEulerAngles.y == 90f || transform.localEulerAngles.y == 270f) // x axis
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(portalCubeScale, transform.localScale.y, transform.localScale.z);
            staticCollider.size = new Vector3(0.01f, 1, 1);
            staticCollider.center = new Vector3(originalPos.x - transform.position.x, staticCollider.center.y,staticCollider.center.z);
        }
        else // z axis 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, portalCubeScale);
            staticCollider.size = new Vector3(1, 1, 0.01f);
            staticCollider.center = new Vector3(staticCollider.center.x, staticCollider.center.y, originalPos.z - transform.position.z);
        }
    }
}
