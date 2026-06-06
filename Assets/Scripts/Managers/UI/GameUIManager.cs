using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject gameOverScreen;
    // [SerializeField] Button nextStageButton;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        toggleHUD(true);
    }

    public void toggleHUD(bool isActive)
    {
        Debug.Log("Toggling HUD: " + isActive);
        HUD.SetActive(isActive);
    }

    public void ShowGameOverScreen()
    {
        toggleHUD(false);

        // if (isWin)
        // {
        //     nextStageButton.gameObject.SetActive(true);
        // }
        // else
        // {
        //     nextStageButton.gameObject.SetActive(false);
        // }

        gameOverScreen.SetActive(true);
        Debug.Log("Game Over screen displayed");
    }

    public bool isGameOverScreenActive()
    {
        // Debug.Log("Checking if Game Over screen is active: " + gameOverScreen.activeSelf);
        return gameOverScreen.activeSelf;
    }

    public void RestartBtn()
    {
        Debug.Log("Restart Button Clicked");
        gameOverScreen.SetActive(false);
        SceneController.instance.RestartScene();
        toggleHUD(true);
    }

    public void MainMenuBtn()
    {
        Debug.Log("Main Menu Button Clicked");
        gameOverScreen.SetActive(false);
        SceneController.instance.ToMainMenu();
    }

    // public void NextStageBtn()
    // {
    //     gameOverScreen.SetActive(false);
    //     SceneController.instance.ToNextScene();
    // }
}
