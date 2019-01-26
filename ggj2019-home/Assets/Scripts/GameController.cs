using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    private UIManager _uimanager;

    public enum Scenes 
    {
        UI,
        Level
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    public static GameController GetInstance()
    {
        return _instance;
    }

    void Start()
    {
        //LoadScene(Scenes.UI);
    }

    private void LoadScene(Scenes scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene));
    }

    IEnumerator LoadSceneCoroutine(Scenes scene)
    {
        yield return SceneManager.LoadSceneAsync(scene.ToString());
    }

    public void StartGame()
    {

    }

    public void SetUIManager(UIManager uimanager)
    {
        _uimanager = uimanager;
    }

}
