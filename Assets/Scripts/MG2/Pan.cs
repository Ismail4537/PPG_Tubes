using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using System;

public class Pan : MiniGame
{
    [SerializeField] SpriteRenderer eggColour;
    [SerializeField] Color burntColour = new Color(0.25f, 0.12f, 0.05f, 1f);
    [Tooltip("If set, use this curve to control how color changes over time (optional)")]
    [SerializeField] AnimationCurve cookCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [Tooltip("Intermediate color when the egg is cooked but not yet burnt")]
    [SerializeField] Color cookedColour = new Color(1f, 0.9f, 0.6f, 1f);
    [SerializeField] Slider cookProgressSlider;
    [SerializeField] Slider safeZoneSlider;
    [SerializeField] GameObject debugPoint;
    public float cookValue;
    public Vector2 safeZoneRange;
    public float cookingSpeed = 5f; // how fast the egg cooks
    public float safeZoneTolerance = 0.1f; // How much time before/after the perfect cook time is still considered "safe"
    private bool isHolding;
    private bool eggBurned;
    private bool eggCooked;
    private Color initialEggColour;
    // Threshold in the cook curve to determine when the egg is considered undercooked vs cooked vs burnt
    public InputAction press, screenPos;
    Vector3 curScreenPos;
    Camera mainCam;

    void Awake()
    {
        safeZoneRange = new Vector2(safeZoneSlider.value - safeZoneTolerance, safeZoneSlider.value + safeZoneTolerance);
        cookValue = cookProgressSlider.value;
    }

    void Start()
    {
        mainCam = Camera.main;
        screenPos.Enable();
        press.Enable();
        screenPos.performed += context =>
        {
            curScreenPos = context.ReadValue<Vector2>();
        };
        if (eggColour != null)
        {
            initialEggColour = eggColour.color;
        }
    }

    void Update()
    {
        press.performed += _ =>
        {
            if (checkClick())
                OnTouchDown();
        };

        press.canceled += _ =>
        {
            if (checkClick())
                OnTouchUp();
        };

        if (isHolding)
        {
            cookProgressSlider.value += Time.deltaTime * cookingSpeed;
            cookValue = cookProgressSlider.value;
            float t = Mathf.Clamp01(cookProgressSlider.value);
            float eval = cookCurve.Evaluate(t);
            if (eggColour != null)
            {
                // Two-stage interpolation: initial -> cooked -> burnt
                Color target;
                if (eval <= 0.5f)
                {
                    float n = eval / 0.5f; // 0..1 over first half
                    target = Color.Lerp(initialEggColour, cookedColour, n);
                }
                else
                {
                    float n = (eval - 0.5f) / 0.5f; // 0..1 over second half
                    target = Color.Lerp(cookedColour, burntColour, n);
                }

                eggColour.color = target;
            }

            if (cookProgressSlider.value > safeZoneRange.y)
            {
                fail(true);
            }
        }
    }

    void OnTouchDown()
    {
        if (!eggBurned && !eggCooked)
        {
            isHolding = true;
            Debug.Log("Touch holding started.");
        }
    }

    void OnTouchUp()
    {
        if (eggBurned && eggCooked) return;
        isHolding = false;
        Debug.Log("Touch holding stopped.");
        if (cookProgressSlider.value < safeZoneRange.x)
        {
            fail(false);

        }
        else if (cookProgressSlider.value > safeZoneRange.y)
        {
            fail(true);
        }
        else
        {
            Cooked();
        }
    }

    void Cooked()
    {
        eggCooked = true;
        screenPos.Disable();
        press.Disable();
        TriggerSuccess();
    }

    void fail(bool isBurnt)
    {
        string msg;
        if (isBurnt)
        {
            msg = "Egg is burnt!";
        }
        else
        {
            msg = "Egg is Udercooked";
        }
        screenPos.Disable();
        press.Disable();
        TriggerGameOver(msg);
    }

    bool checkClick()
    {
        Ray ray = mainCam.ScreenPointToRay(curScreenPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            if (hit.collider.transform == transform)
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }
}
