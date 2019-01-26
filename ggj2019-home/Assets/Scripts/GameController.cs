using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameController GetInstance()
    {
        return instance;
    }
}
