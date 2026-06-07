using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour, IInteractAble
{
    public void NotInteract()
    {
        // throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        SceneController.instance.ToMainMenu();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
