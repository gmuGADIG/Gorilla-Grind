using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script allows for printing messages to the player during gameplay.
/// </summary>
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] GameObject popup;
    [SerializeField] float popupTime = 5f;

    bool popupActive = false;
    Queue<string> messageQueue = new Queue<string>();

    TMP_Text popupText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        popupText = popup.GetComponentInChildren<TMP_Text>();
        popup.transform.parent = null;
        DontDestroyOnLoad(popup);
        popup.SetActive(false);
    }

    public void SendPopupMessage(string message)
    {
        messageQueue.Enqueue(message);
        if (!popupActive)
        {
            StartCoroutine(PopupCoroutine());
        }
    }

    IEnumerator PopupCoroutine()
    {
        popupActive = true;
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            popupText.text = message;
            popup.SetActive(true);
            yield return new WaitForSeconds(popupTime);
        }
        popup.SetActive(false);
        popupActive = false;
    }
}