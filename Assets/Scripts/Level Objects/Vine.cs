using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public float TiltDegreesPerSecond = 90;

    public static void OnPlayerOnVine() {
        print("Player on vine");
    }
    
    public static void OnPlayerOffVine() {
        print("Player off vine");
    }
}
