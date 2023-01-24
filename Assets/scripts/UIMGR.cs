using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMGR : MonoBehaviour
{
    [SerializeField] GameMGR gm;
    [SerializeField] Text stepsCounterText;
    [SerializeField] Text starsCounterText;
    [SerializeField] Text gameOverText;
    [SerializeField] ButtonHandler restartButton;

    public static Action RestartEvent;

    private void OnEnable()
    {
        GameMGR.stepsCounterUpdateEvent += UpdateSteps;
        GameMGR.starsCounterUpdateEvent += UpdateStars;
        GameMGR.gameOverEvent += GameOver;
        restartButton.onClick += OnRestart;
    }

    private void OnDisable()
    {
        GameMGR.stepsCounterUpdateEvent -= UpdateSteps;
        GameMGR.starsCounterUpdateEvent -= UpdateStars;
        GameMGR.gameOverEvent -= GameOver;
        restartButton.onClick -= OnRestart;
    }

    void OnRestart()
    {
        RestartEvent?.Invoke();
    }

    private void UpdateSteps(int steps)
    {
        stepsCounterText.text = steps.ToString();
    }
    private void UpdateStars(int stars)
    {
        starsCounterText.text = stars.ToString();
    }

    private void GameOver(bool isWin)
    {
        if (isWin)
        {
            gameOverText.text = "The player win";
        }
        else
        {
            gameOverText.text = "The player lose";
        }
        gameOverText.enabled = true;
    }
}
