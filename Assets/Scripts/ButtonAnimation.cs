using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float maxSize = 1.2f;
    [SerializeField] float normalSize = 1;
    [SerializeField] float percentSpeed = 0.1f;
    Vector3 targetSize;
    bool shifting;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetSize = new Vector3(maxSize, maxSize, maxSize);
        shifting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetSize = new Vector3(normalSize, normalSize, normalSize);
        shifting = true;
    }
    
    private void Update()
    {
        if (shifting)
        {
            if (transform.localScale != targetSize)
            {
                transform.localScale += (targetSize - transform.localScale) * percentSpeed;
            }
            else
            {
                shifting = false;
            }
        }
    }
}
