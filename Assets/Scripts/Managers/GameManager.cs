using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Slider timeSlider;
    public TextMeshProUGUI timeText;
    public float timeLimit = 100f;
    float duration = 100f;
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
        duration = timeLimit;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !GameUIManager.instance.isGameOverScreenActive())
        {
            CountdownTimer();
        }
    }

    public void triggerGameOver()
    {
        Debug.Log("Game Over Triggered");
        if (GameUIManager.instance.isGameOverScreenActive())
        {
            // Debug.Log("Game Over screen already active, ignoring duplicate trigger.");
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
        duration = timeLimit;

    }

    public void saveCurrStageData()
    {
        Debug.Log("Saving Current Stage Data");
        string stageKey = "Stage_" + SceneManager.GetActiveScene().buildIndex;
    }

    void CountdownTimer()
    {
        duration -= Time.deltaTime;
        timeSlider.value = duration;
        timeText.text = Mathf.CeilToInt(duration).ToString();
        if (duration <= 0)
        {
            GameManager.instance.triggerGameOver();
        }
    }
}
