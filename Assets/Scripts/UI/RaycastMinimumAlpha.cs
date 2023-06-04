using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastMinimumAlpha : MonoBehaviour
{
    public float minimumAlpha;

    void Start()
    {
        Image image = gameObject.GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = minimumAlpha;
    }

}
