using UnityEngine;

public class Clock : MonoBehaviour
{
    public bool isClicked = false;
    void OnMouseDown()
    {
        isClicked = true;
        GameManager.instance.nextMiniGame();
    }
}
