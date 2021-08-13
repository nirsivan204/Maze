using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMGR : MonoBehaviour
{
    [SerializeField] GameMGR gm;
    [SerializeField] Text stepsCounterText;
    [SerializeField] Text starsCounterText;
    [SerializeField] Text gameOverText;
    [SerializeField] Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        gm.stepsCounterUpdateEvent.AddListener(updateSteps);
        gm.starsCounterUpdateEvent.AddListener(updateStars);
        gm.gameOverEvent.AddListener(gameOver);
        restartButton.onClick.AddListener(gm.restart);
    }

    private void updateSteps(int steps)
    {
        stepsCounterText.text = steps.ToString();
    }
    private void updateStars(int stars)
    {
        starsCounterText.text = stars.ToString();
    }

    private void gameOver(bool isWin)
    {
        if (isWin)
        {
            gameOverText.text = "the player win";
        }
        else
        {
            gameOverText.text = "the player lose";
        }
        //gameOverText.gameObject.SetActive(true);
        gameOverText.enabled = true;
    }
}
