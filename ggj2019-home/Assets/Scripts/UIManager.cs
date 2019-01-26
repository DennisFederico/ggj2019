using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        GameController.GetInstance().SetUIManager(this);
    }

    public void StartGame()
    {
        GameController.GetInstance().StartGame();
    }
}
