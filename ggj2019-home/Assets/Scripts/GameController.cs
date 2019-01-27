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

    public static GameController GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameController>();
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.hideFlags = HideFlags.HideAndDontSave;
                _instance = obj.AddComponent<GameController>();
            }
        }
        return _instance;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUIManager(UIManager uimanager)
    {
        _uimanager = uimanager;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Boot"))
        {
            StartCoroutine(LoadScene(Scenes.UI, LoadSceneMode.Additive));
        }
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
                    _uimanager.ShowPlayGameHUD();
                    _isGameRunning = true;
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
        _uimanager.ShowGameOver();
        _uimanager.SetBestScore(_bestScore);
        _uimanager.SetYourScore(_score);
        _uimanager.SetWinStatus(win);
        _isGameRunning = false;
    }

    public void PlayAgain()
    {
        StartCoroutine(
            LoadScene(
                Scenes.UI, 
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
