using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipJar : MonoBehaviour
{
    public static TipJar instance;
    public Sprite[] variants;
    public float ammount = 0;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTips(float value)
    {
        ammount += value;
        if (ammount > 0 && ammount < 50)
        {
            changeSprite(1);
        }
        else if (ammount > 50 && ammount < 100)
        {
            changeSprite(2);
        }
        else if (ammount > 100)
        {
            changeSprite(3);
        }
    }

    void changeSprite(int i)
    {
        spriteRenderer.sprite = variants[i];
    }
}
