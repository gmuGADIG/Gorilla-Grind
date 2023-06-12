using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenuAnimator : MonoBehaviour
{
    void LeaveMenu() {
        SceneManager.LoadScene("Menu");
    }
}
