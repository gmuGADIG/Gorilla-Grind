using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    private static float currentScrollMult = 1;
    private const float baseScrollSpeed = 1;
    public float scrollRate;

    void Update() {
        this.transform.position += Vector3.left * (baseScrollSpeed * currentScrollMult * this.scrollRate * Time.deltaTime);
    }

    void OnBecameInvisible() {
        Destroy(this.gameObject);
    }
}
