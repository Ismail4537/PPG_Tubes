using UnityEngine;

public class MiniGame : MonoBehaviour
{
    protected bool outcomeTriggered;
    protected bool TryWin()
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

    protected bool TryLose(string reason = null)
    {
        if (!string.IsNullOrEmpty(reason)) Debug.Log("GameOver: " + reason);
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

    protected void TriggerSuccess()
    {
        outcomeTriggered = true;
        // Fallback behavior if you have a SceneController
        if (TryWin()) return;
        Debug.Log("Success! Mini-game completed.");
    }

    protected void TriggerGameOver(string reason = null)
    {
        outcomeTriggered = true;
        if (!string.IsNullOrEmpty(reason)) Debug.Log("GameOver: " + reason);
        // Fallback behavior if you have a GameManager
        if (TryLose(reason)) return;
    }
}
