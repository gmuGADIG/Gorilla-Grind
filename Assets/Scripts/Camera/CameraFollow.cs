using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Controls how quickly the camera moves to the player")]
    [SerializeField] float smoothSpeed = 5f;
    
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    
    [Tooltip("The y value which the camera prefers to be close to")]
    public float yTarget;
    
    [Tooltip("Controls interpolation between target y and player y. 0 = ignore player, 1 = ignore target")]
    [SerializeField] float yTolerance;

    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        var target = Vector3.Lerp(transform.position, playerTransform.position + new Vector3(xOffset, yOffset), smoothSpeed * Time.deltaTime);
        target.y = Mathf.Lerp(yTarget, target.y, yTolerance);
        target.z = -10;
        
        transform.position = target;
    }
}
