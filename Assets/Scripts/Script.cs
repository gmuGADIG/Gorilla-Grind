using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    GameObject triangle;
    Transform triangleTransform;
    // Start is called before the first frame update
    void Start()
    {
        triangle = GameObject.Find("Triangle");
        triangleTransform = triangle.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        triangleTransform.Rotate(new Vector3(0,0,0.1f), Space.Self);
    }
}
