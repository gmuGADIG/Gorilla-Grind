using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BananaCountView : MonoBehaviour
{
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponentInChildren<TMP_Text>();
        text.text = Inventory.getBananasInInventory().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Inventory.getBananasInInventory().ToString();
    }
}
