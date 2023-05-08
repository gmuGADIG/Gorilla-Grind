using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundMaker : MonoBehaviour
{
    public GameObject[] BackgroundObj;
    public GameObject[] ForegroundObj;

    private void OnEnable()
    {
        BackgroundObjectScript.newBackground += MakeBackground;
    }

    private void OnDisable()
    {
        BackgroundObjectScript.newBackground -= MakeBackground;
    }

    public void MakeBackground(bool isBackground)
    {
        GameObject obj;
        if (isBackground)
        {
            obj = BackgroundObj[Random.Range(0, BackgroundObj.Length)];
        }
        else
        {
            obj = ForegroundObj[Random.Range(0, ForegroundObj.Length)];
        }

        Instantiate(obj, new Vector3(80, 1, 10), Quaternion.identity);
    }
}