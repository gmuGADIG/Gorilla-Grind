using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 1f;

    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        float yValue = Mathf.Lerp(transform.position.y, playerTransform.position.y, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
    }
}
