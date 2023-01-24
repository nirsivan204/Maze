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
    [SerializeField] ButtonHandler settingsButton;
    [SerializeField] ButtonHandler backButton;
    [SerializeField] GameObject SettingsManu;

    public static Action RestartEvent;

    private void OnEnable()
    {
        GameMGR.stepsCounterUpdateEvent += UpdateSteps;
        GameMGR.starsCounterUpdateEvent += UpdateStars;
        GameMGR.gameOverEvent += GameOver;
        restartButton.onClick += OnRestart;
        settingsButton.onClick += OnSettingsClick;
        backButton.onClick += OnBackClick;
    }

    private void OnSettingsClick()
    {
        restartButton.gameObject.SetActive(false);
        SettingsManu.SetActive(true);
    }

    private void OnBackClick()
    {
        restartButton.gameObject.SetActive(true);
        SettingsManu.SetActive(false);
    }

    private void OnDisable()
    {
        GameMGR.stepsCounterUpdateEvent -= UpdateSteps;
        GameMGR.starsCounterUpdateEvent -= UpdateStars;
        GameMGR.gameOverEvent -= GameOver;
        restartButton.onClick -= OnRestart;
        settingsButton.onClick -= OnSettingsClick;
        backButton.onClick -= OnBackClick;
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
        gameOverText.transform.parent.gameObject.SetActive(true);
    }
}
