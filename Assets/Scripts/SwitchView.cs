using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchView : MonoBehaviour
{
    Camera mainCam;
    public Transform[] targetPos;
    static public SwitchView instance;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        instance = this;
    }

    public void SwitchTo(int i)
    {
        mainCam.transform.position = new Vector3(targetPos[i].position.x, targetPos[i].position.y, mainCam.transform.position.z);
    }
}
