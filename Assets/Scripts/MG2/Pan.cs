using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Pan : MonoBehaviour
{
    [SerializeField] SpriteRenderer eggColour;
    [SerializeField] Color burntColour = new Color(0.25f, 0.12f, 0.05f, 1f);
    [Tooltip("If set, use this curve to control how color changes over time (optional)")]
    [SerializeField] AnimationCurve cookCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [Tooltip("Intermediate color when the egg is cooked but not yet burnt")]
    [SerializeField] Color cookedColour = new Color(1f, 0.9f, 0.6f, 1f);
    [SerializeField] Slider cookProgressSlider;
    [SerializeField] Slider safeZoneSlider;
    public float cookValue;
    public Vector2 szValue;
    public float cookingSpeed = 5f; // how fast the egg cooks
    public float safeZoneTolerance = 0.1f; // How much time before/after the perfect cook time is still considered "safe"
    private bool isHolding;
    private bool eggBurned;
    private bool eggCooked;
    private Color initialEggColour;
    // Threshold in the cook curve to determine when the egg is considered undercooked vs cooked vs burnt

    void Awake()
    {
        if (eggColour != null)
        {
            initialEggColour = eggColour.color;
        }
        szValue = new Vector2(safeZoneSlider.value - safeZoneTolerance, safeZoneSlider.value + safeZoneTolerance);
        cookValue = cookProgressSlider.value;
    }

    void Update()
    {
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

            if (cookProgressSlider.value > szValue.y)
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
        if (cookProgressSlider.value < szValue.x)
        {
            UnderCooked();
        }
        else if (cookProgressSlider.value > szValue.y)
        {
            BurnEgg();
        }
        else
        {
            Cooked();
        }
        // If holdTimer >= burnTime, the egg is already burned in Update(), so no need to handle it here.
    }

    void UnderCooked()
    {
        if (TryGetGameManagerTrigger()) return;
        Debug.Log("Egg is undercooked!");
    }

    void Cooked()
    {
        Debug.Log("Egg is cooked!");
        eggCooked = true;
        if (TryGetSceneControllerNext()) return;
        Debug.Log("Success! Egg cooked within safe zone.");
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
        eggBurned = true;
        if (TryGetGameManagerTrigger()) return;
        Debug.Log("Egg burned!");
    }

    bool TryGetSceneControllerNext()
    {
        try
        {
            var sc = SceneController.instance;
            if (sc != null)
            {
                sc.ToNextScene(false);
                return true;
            }
        }
        catch { }
        return false;
    }

    bool TryGetGameManagerTrigger()
    {
        try
        {
            var gm = GameManager.instance;
            if (gm != null)
            {
                gm.triggerGameOver();
                return true;
            }
        }
        catch { }
        return false;
    }
}
