using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToppingCase : MonoBehaviour, IInteractAble
{
    public GameObject ToppPref;

    public void NotInteract()
    {
        // throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        Instantiate(ToppPref, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + new Vector3(0, 0, 10), Quaternion.identity);
    }


}
