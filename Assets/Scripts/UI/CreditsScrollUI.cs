using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScrollUI : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] float scrollSpeed = 1f;

    private void Update()
    {
        credits.transform.Translate(scrollSpeed * Time.deltaTime * Vector3.up);
    }
}
