using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Slider timeSlider;
    [SerializeField] TextMeshProUGUI timeText;
    // [SerializeField] Button nextStageButton;
    void Awake()
    {
        connectUIObjects();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    void connectUIObjects()
    {
        if (timeSlider != null && timeText != null)
        {
            GameManager.instance.timeSlider = timeSlider;
            GameManager.instance.timeText = timeText;
        }
    }

    // public void NextStageBtn()
    // {
    //     gameOverScreen.SetActive(false);
    //     SceneController.instance.ToNextScene();
    // }
}
