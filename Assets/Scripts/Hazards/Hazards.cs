using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            print("Murdering Players");
            col.GetComponent<PlayerMovement>().Murder();
        }
    }
}
