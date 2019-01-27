using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    private static string BestCoreKey = "best_score";

    public float GameTime = 100;

    private UIManager _uimanager;

    private int _followersCaptured;
    private int _followersKilled;
    private int _score;
    private float _timeLeft;
    private int _bestScore;

    private int _initialFollowers;

    private bool _isGameRunning = false;

    public enum Scenes 
    {
        Boot,
        UI,
        Level1,
        Main
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    public static GameController GetInstance()
    {
        return _instance;
    }

    public void SetUIManager(UIManager uimanager)
    {
        _uimanager = uimanager;
    }

    void Start()
    {
#if !DENNIS
        StartCoroutine(LoadScene(Scenes.UI, LoadSceneMode.Additive));
#endif
    }

    public void SetInitialFollowers(int initialFollowers)
    {
        _initialFollowers = initialFollowers;
        _isGameRunning = false;
    }

    public void IncreaseCapturedFollowers()
    {
        _followersCaptured++;
    }

    public void IncreaseKilledFollowers()
    {
        _followersKilled++;
    }

    IEnumerator LoadScene(Scenes scene, LoadSceneMode sceneMode, System.Action onLoaded = null)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.ToString(), sceneMode);
        while(!op.isDone)
        {
            yield return null;
        }

        onLoaded?.Invoke();
    }

    public void StartGame()
    {
        _bestScore = PlayerPrefs.GetInt("best_score", 0);
        _timeLeft = GameTime;
        _followersCaptured = 0;
        _followersKilled = 0;

        StartCoroutine(
            LoadScene(
                Scenes.Level1, 
                LoadSceneMode.Additive, 
                () =>
                {
                    _uimanager.SetState(UIManager.States.PlayHud);
                }
            )
        );
    }

    public void GameOver(bool win)
    {
        _score = (int) _timeLeft + _followersCaptured;
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt(BestCoreKey, _bestScore);
        }
        _uimanager.SetState(UIManager.States.GameOver);
        _uimanager.SetBestScore(_bestScore);
        _uimanager.SetYourScore(_score);
        _uimanager.SetWinStatus(win);
        _isGameRunning = false;
    }

    public void PlayAgain()
    {
        StartCoroutine(
            LoadScene(
                Scenes.Boot, 
                LoadSceneMode.Single));
    }

    void Update()
    {
        if (_uimanager != null && _isGameRunning)
        {
            int leftFollowers = _initialFollowers - _followersCaptured - _followersKilled;
            _uimanager.SetFollowers(leftFollowers);

            _timeLeft -= Time.deltaTime;
            _uimanager.SetTime((int) _timeLeft);

            if (_timeLeft <= 0)
            {
                GameOver(false);
            }
            else if (leftFollowers <= 0)
            {
                GameOver(true);
            }
        }
    }

}
