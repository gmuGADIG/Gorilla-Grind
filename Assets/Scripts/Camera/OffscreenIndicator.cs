using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    public Camera mainCamera;
    public Camera playerCamera;
    RectTransform thisTransform;
    public PlayerMovement player;

    public GameObject[] children;

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the indicator to be inline with the player
        Vector2 playerPosition = mainCamera.WorldToViewportPoint(player.transform.position);
        playerCamera.rect = new Rect(
            playerPosition.x - (playerCamera.rect.width / 2),
            playerCamera.rect.y,
            playerCamera.rect.width,
            playerCamera.rect.height
        );

        thisTransform.anchorMin = new Vector2(playerPosition.x, 0);
        playerCamera.transform.position = player.transform.position + new Vector3(0, 0.5f);

        // Hide the indicator if the player is on screen
        bool active = playerPosition.y > 1.1;
        foreach (GameObject obj in children)
            obj.SetActive(active);
        
        playerCamera.orthographicSize = Mathf.Clamp(playerPosition.y * 1.5f, 2, 5);
    }
}
