using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [Header("Start time (MM:SS)")]
    public int startMinutes = 5;
    public int startSeconds = 58;

    [Header("Target time (MM:SS)")]
    public int targetMinutes = 6;
    public int targetSeconds = 0;

    [Tooltip("Allowed time window in seconds around the exact target. Set to 0 for exact match (may be difficult in practice).")]
    public float toleranceSeconds = 0.1f;

    [Header("UI")]
    public TextMeshProUGUI timeTextTMP;
    public Text timeTextLegacy;

    float currentTimeSeconds;
    float targetTimeSeconds;
    bool hasPressed = false;
    bool outcomeTriggered = false;

    void Start()
    {
        currentTimeSeconds = startMinutes * 60 + startSeconds;
        targetTimeSeconds = targetMinutes * 60 + targetSeconds;
        UpdateDisplay();
    }

    void Update()
    {
        if (outcomeTriggered) return;

        currentTimeSeconds += Time.deltaTime;
        UpdateDisplay();

        // Check for input (spacebar or left mouse) or mouse/tap on object
        if (!hasPressed && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            OnUserPress();
        }

        // Mobile touch input
        if (!hasPressed && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                OnUserPress();
            }
        }

        // If time passed target + tolerance with no correct press -> game over
        if (!hasPressed && currentTimeSeconds > targetTimeSeconds + toleranceSeconds)
        {
            TriggerGameOver("Missed the exact 6:00 window");
        }
    }

    void UpdateDisplay()
    {
        int minutes = (int)(currentTimeSeconds / 60f);
        int seconds = (int)(currentTimeSeconds % 60f);
        string txt = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        if (timeTextTMP != null) timeTextTMP.text = txt;
        if (timeTextLegacy != null) timeTextLegacy.text = txt;
    }

    // Call this from UI Button or when the player clicks/taps the clock GameObject
    public void OnUserPress()
    {
        if (outcomeTriggered) return;
        hasPressed = true;

        float diff = Mathf.Abs(currentTimeSeconds - targetTimeSeconds);
        if (diff <= toleranceSeconds)
        {
            TriggerSuccess();
        }
        else
        {
            TriggerGameOver("Pressed at wrong time: " + FormatTime(currentTimeSeconds));
        }
    }

    void OnMouseDown()
    {
        OnUserPress();
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60f);
        int s = (int)(t % 60f);
        return string.Format("{0:D2}:{1:D2}", m, s);
    }

    void TriggerSuccess()
    {
        outcomeTriggered = true;
        // Fallback behavior if you have a SceneController
        if (TryGetSceneControllerNext()) return;
        Debug.Log("Success! Pressed exactly at target time.");
    }

    void TriggerGameOver(string reason = null)
    {
        outcomeTriggered = true;
        if (!string.IsNullOrEmpty(reason)) Debug.Log("GameOver: " + reason);
        // Fallback behavior if you have a GameManager
        if (TryGetGameManagerTrigger()) return;
    }

    bool TryGetSceneControllerNext()
    {
        try
        {
            var sc = SceneController.instance;
            if (sc != null)
            {
                sc.ToNextScene(false);
                return true;
            }
        }
        catch { }
        return false;
    }

    bool TryGetGameManagerTrigger()
    {
        try
        {
            var gm = GameManager.instance;
            if (gm != null)
            {
                gm.triggerGameOver();
                return true;
            }
        }
        catch { }
        return false;
    }
}
