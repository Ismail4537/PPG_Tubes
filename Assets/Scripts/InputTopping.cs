using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTopping : MonoBehaviour
{
    public InputAction touch, pos;
    Camera mainCam;
    Vector3 currPos;
    public GameObject[] Toppings;
    void Awake()
    {
        mainCam = Camera.main;
        touch.Enable();
        pos.Enable();
    }

    void Start()
    {
        pos.performed += context =>
        {
            currPos = context.ReadValue<Vector2>();
        };
    }

    void Update()
    {
        touch.performed += _ =>
        {
            Debug.Log("clicked");
            checkClick();
        };
    }

    void checkClick()
    {
        Ray ray = mainCam.ScreenPointToRay(currPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            for (int i = 0; i < Toppings.Length; i++)
            {
                if (hit.collider.gameObject.name == Toppings[i].name)
                {
                    Toppings[i].GetComponent<Topping>().InstantiateTopping();
                }
            }
        }
    }
}
