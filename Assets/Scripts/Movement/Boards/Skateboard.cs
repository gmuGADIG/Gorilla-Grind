using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skateboard : MonoBehaviour
{
    [SerializeField] GameObject boardSideView;
    [SerializeField] GameObject boardCosmeticView;

    public void ShowCosmeticView()
    {
        boardSideView.SetActive(false);
        boardCosmeticView.SetActive(true);
    }

    public void ShowSideView()
    {
        boardSideView.SetActive(true);
        boardCosmeticView.SetActive(false);
    }

    public abstract void ApplyBonuses(PlayerMovement movement);
    public abstract void RemoveBonuses(PlayerMovement movement);
}
