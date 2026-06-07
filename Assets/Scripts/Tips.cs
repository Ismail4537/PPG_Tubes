using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour, IInteractAble
{
    public float value;
    public Sprite[] variants;
    SpriteRenderer spriteRenderer;

    public void NotInteract()
    {
    }

    public void OnInteract()
    {
        TipJar.instance.AddTips(value);
        SFXManager.instance.PlayClip2D("Money");
        Destroy(gameObject);
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetValue(float ammount)
    {
        value = ammount;
        if (value > 3 && value < 6)
        {
            spriteRenderer.sprite = variants[1];
        }
        else if (value > 6 && value < 9)
        {
            spriteRenderer.sprite = variants[2];
        }
        else if (value > 9)
        {
            spriteRenderer.sprite = variants[3];
        }
        else
        {
            spriteRenderer.sprite = variants[0];
        }
    }
}
