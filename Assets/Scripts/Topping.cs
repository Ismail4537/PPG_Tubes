using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Topping : MonoBehaviour
{
    public GameObject ToppPref;

    public void InstantiateTopping()
    {
        Instantiate(ToppPref, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + new Vector3(0, 0, 10), Quaternion.identity);
    }
}
