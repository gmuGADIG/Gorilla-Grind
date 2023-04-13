using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    Skateboard currentBoard;
    PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        movement.OnStateChange += UpdateBoardSprite;
    }

    void Start()
    {
        Skateboard board = GetComponentInChildren<Skateboard>();
        ChangeCurrentBoard(board);
    }

    public void ChangeCurrentBoard(Skateboard newBoard)
    {
        if (currentBoard != null)
        {
            currentBoard.RemoveBonuses(movement);
        }
        currentBoard = newBoard;
        currentBoard.ApplyBonuses(movement);
    }

    void UpdateBoardSprite(StateType playerState)
    {
        if (playerState == StateType.InTrick)
        {
            currentBoard.ShowCosmeticView();
        }
        else
        {
            currentBoard.ShowSideView();
        }
    }

}
