using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float zoomOutSpeed = 1f;
    [SerializeField] float zoomInSpeed = 3f;
    [SerializeField] float maxZoom = 50f;

    PlayerMovement player;
    Camera mainCam;
    float startingY;
    float minZoom;
    bool recordYPosition = true;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        mainCam = Camera.main;
        minZoom = mainCam.orthographicSize;
    }

    void Update()
    {
        if (!player.IsGrounded)
        {
            if (recordYPosition)
            {
                startingY = player.transform.position.y;
                recordYPosition = false;
            }
            float zoom = minZoom + (player.transform.position.y - startingY) * zoomOutSpeed;
            zoom = Mathf.Clamp(zoom, mainCam.orthographicSize, maxZoom);
            mainCam.orthographicSize = zoom;
        }
        else
        {
            recordYPosition = true;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, minZoom, zoomInSpeed * Time.deltaTime);
        }
    }

}
