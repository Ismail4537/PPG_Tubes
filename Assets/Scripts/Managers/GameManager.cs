using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
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

    }

    public void triggerGameOver()
    {
        Debug.Log("Game Over Triggered");
        if (GameUIManager.instance.isGameOverScreenActive())
        {
            Debug.Log("Game Over screen already active, ignoring duplicate trigger.");
            return;
        }
        MusicManager.instance.StopMusicTrack(0.8f);
        saveCurrStageData();
        SFXManager.instance.PlayClip2D("GameOver", 1.0f);
        GameUIManager.instance.ShowGameOverScreen();
        Debug.Log("Game Over screen displayed");
    }

    public void triggerWin(string title = "Score")
    {
        Debug.Log("Win Triggered");
        if (GameUIManager.instance.isGameOverScreenActive())
        {
            Debug.Log("Game Over screen already active, ignoring duplicate win trigger.");
            return;
        }
        saveCurrStageData();
        SFXManager.instance.PlayClip2D("Win", 1.0f);
        // PlayerPrefs.SetString("Stage_" + SceneManager.GetActiveScene().buildIndex + "_Win", "Yes");
        GameUIManager.instance.ShowGameOverScreen();
        Debug.Log("Win screen displayed with title: " + title);
    }

    public void resetGameData()
    {
        Debug.Log("Resetting Game Data");
        // Reset relevant game data here

    }

    public void saveCurrStageData()
    {
        Debug.Log("Saving Current Stage Data");
        string stageKey = "Stage_" + SceneManager.GetActiveScene().buildIndex;
    }
}
