using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if this object comes into contact with a gameobject that has the specified layer
        if (mask == (mask | (1 << collision.transform.gameObject.layer)))
        {
            //print message to console for now
            Debug.Log("Hit object");
        }
    }
}
