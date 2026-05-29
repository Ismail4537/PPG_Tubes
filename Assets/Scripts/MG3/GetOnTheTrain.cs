using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class GetOnTheTrain : MiniGame, IEndDragHandler, IDragHandler
{
    float currTime = 0;
    public float openTime = 0;
    public float closeTime = 0;
    bool canWin = false;
    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        checkWin(draggedDir);
        return draggedDir;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        GetDragDirection(dragVectorDirection);
    }

    public void OnDrag(PointerEventData eventData) { }

    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= openTime && currTime < closeTime)
        {
            canWin = true;
            Debug.Log("Door Open");
        }
        else
        {
            canWin = false;
            Debug.Log("Door Closed");
        }
    }

    void checkWin(DraggedDirection dragDir)
    {
        if (dragDir == DraggedDirection.Up)
        {
            if (canWin)
            {
                TriggerSuccess();
            }
            else
            {
                TriggerGameOver();
            }
        }
        else
        {
            TriggerGameOver();
        }
    }
}
