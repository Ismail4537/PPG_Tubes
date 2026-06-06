using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    int maxScene;

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
        maxScene = SceneManager.sceneCountInBuildSettings - 1;
    }

    public void LoadScene(int sceneID)
    {
        Debug.Log("Loading Scene ID: " + sceneID);
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
    }

    public void RestartScene()
    {
        Debug.Log("Restarting Current Scene: " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.instance.saveCurrStageData();
        GameManager.instance.resetGameData();
        Time.timeScale = 1;
    }

    public void ToMainMenu()
    {
        Debug.Log("Returning to Main Menu");
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        GameManager.instance.resetGameData();
        if (GameUIManager.instance.gameObject != null)
            Destroy(GameUIManager.instance.gameObject);
    }

    public void ToGameScene(int sceneID, bool resetData)
    {
        Debug.Log("Loading Game Scene ID: " + sceneID);
        SceneManager.LoadScene(sceneID);
        Time.timeScale = 1;
        if (resetData)
        {
            GameManager.instance.resetGameData();
        }
    }

    public void ToNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log("Next Scene Index: " + nextSceneIndex);

        if (nextSceneIndex <= maxScene && nextSceneIndex >= 0)
        {
            ToGameScene(nextSceneIndex, true);
        }
        else
        {
            ToMainMenu();
        }
    }

    public void ToNextScene(bool resetData)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log("Next Scene Index: " + nextSceneIndex);

        if (nextSceneIndex <= maxScene && nextSceneIndex >= 0)
        {
            ToGameScene(nextSceneIndex, resetData);
        }
        else
        {
            LoadScene(1);
        }
    }
}