using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private States _state;
    private bool _winState;

    [Header("Canvas Groups")]
    public CanvasGroup MainMenuCG;
    public CanvasGroup PlayHudCG;
    public CanvasGroup GameOverCG;

    [Header("Text Fields")]
    public Text FollowersText;
    public Text TimeText;
    public Text BestScoreText;
    public Text YourScoreText;

    [Header("Win Images")]
    public Image GameOverImage;
    public Image LevelCompletedImage;

    public enum States
    {
        MainMenu,
        PlayHud, 
        GameOver
    }

    private void Awake()
    {
        GameController.GetInstance().SetUIManager(this);
        SetState(States.MainMenu);
    }

    public void StartGame()
    {
        GameController.GetInstance().StartGame();
    }

    public void SetState(States newState)
    {
        _state = newState;
        switch (_state)
        {
            case States.MainMenu:
                MainMenuCG.alpha = 1;
                MainMenuCG.interactable = true;
                PlayHudCG.alpha = 0;
                PlayHudCG.interactable = false;
                GameOverCG.alpha = 0;
                GameOverCG.interactable = false;
                GameOverCG.blocksRaycasts = false;
                break;
            case States.PlayHud:
                MainMenuCG.alpha = 0;
                MainMenuCG.interactable = false;
                PlayHudCG.alpha = 1;
                PlayHudCG.interactable = true;
                GameOverCG.alpha = 0;
                GameOverCG.interactable = false;
                GameOverCG.blocksRaycasts = false;
                break;
            case States.GameOver:
                MainMenuCG.alpha = 0;
                MainMenuCG.interactable = false;
                PlayHudCG.alpha = 0;
                PlayHudCG.interactable = false;
                GameOverCG.alpha = 1;
                GameOverCG.interactable = true;
                GameOverCG.blocksRaycasts = true;
                break;
            default:
                break;
        }
    }

    public States GetCurrentState()
    {
        return _state;
    }

    public void SetFollowers(int followers)
    {
        FollowersText.text = "Left:" + followers;
    }

    public void SetTime(int timeLeft)
    {
        TimeText.text = "Time:" + timeLeft;
    }

    public void SetBestScore(int score)
    {
        BestScoreText.text = "" + score;
    }

    public void SetYourScore(int score)
    {
        YourScoreText.text = "" + score;
    }

    public void SetWinStatus(bool win)
    {
        _winState = win;
        GameOverImage.gameObject.SetActive(!win);
        LevelCompletedImage.gameObject.SetActive(win);
    }

    public void Retry()
    {
        GameController.GetInstance().PlayAgain();
    }
}
