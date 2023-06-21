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
        Vector3 target = new Vector3(xOffset, player.GroundYPosition + yOffset, -10);

        // print($"CameraFollow target: {target.y}");

        if (player.IsGrounded) {
            transform.position = Vector3.Lerp(transform.position, target, 10 * Time.deltaTime);;
        } else {
            // Make the camera move faster when it's far away from the target
            float urgencyModifier = Mathf.Min(Mathf.Abs((target - transform.position).magnitude * 10), 1); 
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * urgencyModifier * Time.deltaTime);
        }       
    }
}
