using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    static public Vector2 touchPos = Vector2.zero;
    Camera mainCam;
    void Awake()
    {
        mainCam = Camera.main;
    }
    public void OnTouch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            // Debug.Log("Started");

        }
        if (ctx.performed)
        {
            // Debug.Log("Performed");
            if (checkInteractAble() != null)
            {
                checkInteractAble().OnInteract();
            }
        }
        if (ctx.canceled)
        {
            // Debug.Log("Canceled");
            if (checkInteractAble() != null)
            {
                checkInteractAble().NotInteract();
            }
        }
    }

    public void TouchPosition(InputAction.CallbackContext ctx)
    {
        touchPos = ctx.ReadValue<Vector2>();
        // Debug.Log(touchPos);
    }

    IInteractAble checkInteractAble()
    {
        Ray ray = mainCam.ScreenPointToRay(touchPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            IInteractAble interactAble = hit.collider.GetComponent<IInteractAble>();
            if (interactAble != null)
            {
                return interactAble;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
