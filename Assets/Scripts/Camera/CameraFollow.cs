using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Controls how quickly the camera moves to the player")]
    [SerializeField] float smoothSpeed = 5f;
    
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;

    Transform playerTransform;

    PlayerMovement player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        Debug.Assert(player != null);
    }

    private void FixedUpdate()
    {
        // var target = Vector3.Lerp(transform.position, playerTransform.position + new Vector3(xOffset, yOffset), smoothSpeed * Time.deltaTime);
        // target.y = Mathf.Lerp(yTarget, target.y, yTolerance);
        // target.z = -10;
        
        // transform.position = target;

        Vector3 target = new Vector3(xOffset, player.GroundYPosition + yOffset, -10);

        print($"CameraFollow target: {target.y}");

        if (player.IsGrounded) {
            transform.position = target;
        } else {
            // Make the camera move faster when it's far away from the target
            float urgencyModifier = Mathf.Min(Mathf.Abs((target - transform.position).magnitude * 3), 1); 
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * urgencyModifier * Time.deltaTime);
        }       
    }
}
