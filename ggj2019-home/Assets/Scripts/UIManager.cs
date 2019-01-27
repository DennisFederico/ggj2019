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
    public CanvasGroup CreditsCG;

    [Header("Text Fields")]
    public Text FollowersText;
    public Text TimeText;
    public Text BestScoreText;
    public Text YourScoreText;

    [Header("Win Images")]
    public Image GameOverImage;
    public Image LevelCompletedImage;

    private CanvasGroup[] canvasGroups = new CanvasGroup[4];

    public enum States
    {
        MainMenu,
        PlayHud, 
        GameOver, 
        Credits
    }

    private void Awake()
    {
        canvasGroups[(int)States.MainMenu] = MainMenuCG;
        canvasGroups[(int)States.PlayHud] = PlayHudCG;
        canvasGroups[(int)States.GameOver] = GameOverCG;
        canvasGroups[(int)States.Credits] = CreditsCG;

        GameController.GetInstance().SetUIManager(this);
        ShowMainMenu();
    }

    private void SetState(States newState)
    {
        _state = newState;
        SetCanvasGroup(_state);
    }

    private void SetCanvasGroup(States state)
    {
        int stateNum = (int)state;
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = stateNum == i ? 1 : 0;
            canvasGroups[i].interactable = stateNum == i ? true : false;
            canvasGroups[i].blocksRaycasts = stateNum == i ? true : false;
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

    public void ShowMainMenu()
    {
        SetState(States.MainMenu);
        AudioManager.Instance.PlayIntroMusic();
    }

    public void StartGame()
    {
        GameController.GetInstance().StartGame();
    }

    public void ShowPlayGameHUD()
    {
        SetState(States.PlayHud);
    }

    public void ShowGameOver()
    {
        SetState(States.GameOver);
    }

    public void ShowCredits()
    {
        SetState(States.Credits);
    }

    public void BackFromCredits()
    {
        SetState(States.MainMenu);
    }

    public void Retry()
    {
        GameController.GetInstance().PlayAgain();
    }

    public void PlayClickSFX()
    {
        AudioManager.Instance.PlaySFX("MenuClick");
    }
}
