using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Clock : MiniGame
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
    float currentTimeSeconds;
    float targetTimeSeconds;
    Vector2 SafeZoneRange;

    void Start()
    {
        currentTimeSeconds = startMinutes * 60 + startSeconds;
        targetTimeSeconds = targetMinutes * 60 + targetSeconds;
        SafeZoneRange = new Vector2(targetTimeSeconds - toleranceSeconds, targetTimeSeconds + toleranceSeconds);
        // Debug.Log(SafeZoneRange);
        UpdateDisplay();
    }

    void Update()
    {
        if (outcomeTriggered) return;

        currentTimeSeconds += Time.deltaTime;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        string txt = FormatTime(currentTimeSeconds);
        if (timeTextTMP != null) timeTextTMP.text = txt;
    }

    // Call this from UI Button or when the player clicks/taps the clock GameObject
    public void OnUserPress()
    {
        if (outcomeTriggered) return;
        if (currentTimeSeconds < SafeZoneRange.x || currentTimeSeconds > SafeZoneRange.y)
        {
            // Debug.Log(currentTimeSeconds);
            // Debug.Log("Fail");
            TriggerGameOver("Pressed at wrong time: " + FormatTime(currentTimeSeconds));
            Debug.Log("Cur Time : " + currentTimeSeconds + " Safe zone time : " + SafeZoneRange.x + " ," + SafeZoneRange.y);
        }
        else
        {
            // Debug.Log(currentTimeSeconds);
            // Debug.Log("Success");
            TriggerSuccess();
        }
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60f);
        int s = (int)(t % 60f);
        return string.Format("{0:D2}:{1:D2}", m, s);
    }
}
