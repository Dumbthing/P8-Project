using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockScifiPortal : MonoBehaviour
{
    Collider portalCol;
    public GameObject portalToUnlock;

    public float  doorThreshold = 0.8f;
    float startPosition;
    // Start is called before the first frame update
    void Start()
    {
        portalCol = portalToUnlock.GetComponent<BoxCollider>();
        startPosition = transform.position.y;

        if(portalCol.enabled) {
		    portalCol.enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(!portalCol.enabled && transform.position.y - startPosition >= doorThreshold) {
            portalCol.enabled = true;
        }
        else if(portalCol.enabled && transform.position.y - startPosition < doorThreshold) {
            portalCol.enabled = false;
        }
    }
}
