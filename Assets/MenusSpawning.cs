using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusSpawning : MonoBehaviour
{
    [SerializeField] GameObject MenusPrefab;
    private void Start()
    {
        Instantiate(MenusPrefab);
    }
}
