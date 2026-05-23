using UnityEngine;

public class Pan : MonoBehaviour
{
    public float burnTime = 5f;
    [SerializeField] SpriteRenderer eggColour;
    [SerializeField] Color burntColour = new Color(0.25f, 0.12f, 0.05f, 1f);
    [Tooltip("If set, use this curve to control how color changes over time (optional)")]
    [SerializeField] AnimationCurve cookCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [Tooltip("Intermediate color when the egg is cooked but not yet burnt")]
    [SerializeField] Color cookedColour = new Color(1f, 0.9f, 0.6f, 1f);

    private bool isHolding;
    private bool eggBurned;
    private bool eggCooked;
    private float holdTimer;
    private Color initialEggColour;
    // Threshold in the cook curve to determine when the egg is considered undercooked vs cooked vs burnt
    float undercookedThreshold;

    void Awake()
    {
        undercookedThreshold = burnTime / 2f;
        if (eggColour != null)
        {
            initialEggColour = eggColour.color;
        }
    }

    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            float t = Mathf.Clamp01(holdTimer / burnTime);
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

            if (holdTimer >= burnTime)
            {
                BurnEgg();
            }
        }
    }

    void OnMouseDown()
    {
        if (!eggBurned && !eggCooked)
        {
            isHolding = true;
            Debug.Log("Holding started.");
        }
    }

    void OnMouseUp()
    {
        if (eggBurned && eggCooked) return;
        isHolding = false;
        Debug.Log("Holding stopped.");
        if (holdTimer < undercookedThreshold)
        {
            UnderCooked();
        }
        else if (holdTimer < burnTime)
        {
            Cooked();
        }
        // If holdTimer >= burnTime, the egg is already burned in Update(), so no need to handle it here.
    }

    void UnderCooked()
    {
        Debug.Log("Egg is undercooked!");
    }

    void Cooked()
    {
        Debug.Log("Egg is cooked!");
        eggCooked = true;
        // You can add additional logic here for when the egg is perfectly cooked
    }

    private void BurnEgg()
    {
        if (eggColour == null)
        {
            Debug.LogWarning("Egg SpriteRenderer is not assigned on the Pan script.");
            return;
        }

        // Ensure final burnt color is applied and stop further cooking updates
        eggColour.color = burntColour;
        eggBurned = true;
        Debug.Log("Egg burned!");
    }
}
