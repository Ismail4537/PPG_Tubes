using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    static public Vector2 touchPos = Vector2.zero;
    static public bool touchDown = false;
    Camera mainCam;
    void Awake()
    {
        mainCam = Camera.main;
    }
    void Start()
    {

    }
    public void OnTouch(InputAction.CallbackContext ctx)
    {
        bool isTouching = ctx.ReadValueAsButton();
        // if (ctx.started)
        // {
        //     Debug.Log("Started");

        // }

        if (!isTouching)
        {
            Debug.Log("Touching");
            touchDown = false;
            if (checkInteractAble() != null)
            {
                checkInteractAble().NotInteract();
            }
        }
        else
        {
            Debug.Log("NotTouching");
            touchDown = true;
            if (checkInteractAble() != null)
            {
                checkInteractAble().OnInteract();
            }
        }
        // if (ctx.performed)
        // {
        //     touchDown = true;
        //     Debug.Log("Performed");
        //     if (checkInteractAble() != null)
        //     {
        //         checkInteractAble().OnInteract();
        //     }
        // }
        // if (ctx.canceled)
        // {
        //     touchDown = false;
        //     Debug.Log("Canceled");
        //     if (checkInteractAble() != null)
        //     {
        //         checkInteractAble().NotInteract();
        //     }
        // }
    }

    public void TouchPosition(InputAction.CallbackContext ctx)
    {
        touchPos = ctx.ReadValue<Vector2>();
        // Debug.Log(touchPos);
    }

    IInteractAble checkInteractAble()
    {
        if (mainCam != null)
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
        else
        {
            return null;
        }
    }
}
