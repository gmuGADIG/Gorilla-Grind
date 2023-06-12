using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaveShopButton : MonoBehaviour
{
    public Animator MenuAnimator;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(
            () => {
                MenuAnimator.Play("MenuClosed");
            }
        );
    }
}
