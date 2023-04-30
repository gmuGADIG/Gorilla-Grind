using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardShop : MonoBehaviour
{
    private Animator ShopAnimator;

    // Start is called before the first frame update
    void Start()
    {
        ShopAnimator = gameObject.GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu() {
        ShopAnimator.SetTrigger("ToggleMenu");
    }
}
