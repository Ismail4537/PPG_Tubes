using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public bool servingAproval = false;
    public bool served;
    public bool ordering;
    public float unsatisfactionPenalty = 1;
    public float unsatisfactionMultiplier = 1;
    public float patienceTimer;
    public float patienceDuration;
    public Slider patienceBar;
    public GameObject tipPref;
    public GameObject aprov;
    public float minTip = 1f;
    public float maxTip = 10f;
    public GameObject orderItemPref;
    public GameObject orderUI;
    public GameObject[] wantedToppings;
    public GameObject[] optionalToppings;
    public GameObject[] alwaysToppings; //Topping yang selalu ada di pizza, misal saus dan keju
    GameObject[] possibleToppings; //Semua topping yang mungkin ada di pizza, termasuk alwaysToppings dan optionalToppings
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SetPossibleTopping();
        ReGenerateWantedTopping();
        ResetPatience();
    }

    // Update is called once per frame
    void Update()
    {
        PatienceCountdown();
    }

    void PatienceCountdown()
    {
        if (!ordering)
            return;
        if (patienceBar == null)
            return;

        if (patienceDuration <= 0f)
            return;
        if (patienceTimer > 0f)
            patienceTimer -= Time.deltaTime;

        patienceBar.value = patienceTimer;

        if (patienceTimer <= 0f)
        {
            Debug.Log("Customer patience ran out.");
            OutOfPatience();
        }
    }

    void ResetPatience()
    {
        patienceTimer = patienceDuration;
        SetupPatienceBar();
    }

    void SetupPatienceBar()
    {
        if (patienceBar == null)
            return;

        patienceBar.maxValue = patienceDuration;
        patienceBar.value = patienceTimer;
    }

    void DisplayOrder()
    {
        RectTransform orderTransform = orderUI.GetComponent<RectTransform>();
        orderTransform.sizeDelta = new Vector2(5, 5);
        // Clear existing order UI
        foreach (Transform child in orderUI.transform)
        {
            Destroy(child.gameObject);
        }

        // Display wanted toppings
        for (int i = 0; i < wantedToppings.Length; i++)
        {
            if (wantedToppings[i] != null)
            {
                GameObject item = Instantiate(orderItemPref, orderUI.transform);
                item.GetComponent<Image>().sprite = wantedToppings[i].GetComponent<ToppingOn>().iconSprite;
            }
        }
        ;
        orderTransform.sizeDelta = new Vector2(orderTransform.sizeDelta.x, orderTransform.sizeDelta.y * wantedToppings.Length);
    }
    public void CheckingPizza(PizzaModel pizza)
    {
        if (!ordering || served || !servingAproval) return;
        if (!pizza.cooked || pizza.burnt)
        {
            NotSastified();
            return;
        }
        if (pizza == null || pizza.toppings == null)
        {
            Debug.Log("CheckingPizza: pizza or pizza.toppings is null");
            NotSastified();
            return;
        }

        string wantedNames = "";
        for (int i = 0; i < wantedToppings.Length; i++)
        {
            wantedNames += (wantedToppings[i] != null) ? wantedToppings[i].name : "<null>";
            if (i < wantedToppings.Length - 1) wantedNames += ", ";
        }

        string pizzaNames = "";
        for (int i = 0; i < pizza.toppings.Length; i++)
        {
            pizzaNames += (pizza.toppings[i] != null) ? pizza.toppings[i].name : "<null>";
            if (i < pizza.toppings.Length - 1) pizzaNames += ", ";
        }

        Debug.Log($"Checking pizza: wanted [{wantedNames}] vs pizza [{pizzaNames}]");

        var isToppingExist = new bool[possibleToppings.Length];
        for (int i = 0; i < pizza.toppings.Length; i++)
        {
            if (pizza.toppings[i] == null) continue;
            for (int j = 0; j < possibleToppings.Length; j++)
            {
                if (possibleToppings[j] == null) continue;
                // Compare by name to handle instantiated copies vs prefab references
                if (pizza.toppings[i].name == possibleToppings[j].name)
                {
                    isToppingExist[j] = true;
                    Debug.Log($"Pizza has topping: {possibleToppings[j].name}");
                    break;
                }
            }
        }

        bool isSatisfied = true;
        for (int i = 0; i < wantedToppings.Length; i++)
        {
            if (wantedToppings[i] == null)
            {
                Debug.Log("Wanted topping is null, marking not satisfied.");
                isSatisfied = false;
                break;
            }

            bool toppingFound = false;
            for (int j = 0; j < possibleToppings.Length; j++)
            {
                if (possibleToppings[j] == null) continue;
                if (wantedToppings[i].name == possibleToppings[j].name && isToppingExist[j])
                {
                    toppingFound = true;
                    Debug.Log($"Wanted topping found: {wantedToppings[i].name}");
                    break;
                }
            }

            if (!toppingFound)
            {
                Debug.Log($"Missing wanted topping: {wantedToppings[i].name}");
                isSatisfied = false;
                break;
            }
        }

        if (isSatisfied)
        {
            Debug.Log("Customer satisfied: all wanted toppings present.");
            Sastified();
        }
        else
        {
            Debug.Log("Customer not satisfied: some wanted toppings missing.");
            NotSastified();
        }
    }
    void SetPossibleTopping()
    {
        possibleToppings = new GameObject[optionalToppings.Length + alwaysToppings.Length];
        for (int i = 0; i < optionalToppings.Length; i++)
        {
            possibleToppings[i] = optionalToppings[i];
        }
        for (int i = 0; i < alwaysToppings.Length; i++)
        {
            possibleToppings[optionalToppings.Length + i] = alwaysToppings[i];
        }
    }
    void ReGenerateWantedTopping()
    {
        wantedToppings = new GameObject[0];
        int optionalCount = Random.Range(1, optionalToppings.Length + 1);
        int toppingCount = alwaysToppings.Length + optionalCount;
        wantedToppings = new GameObject[toppingCount];

        int index = 0;
        for (int i = 0; i < alwaysToppings.Length; i++)
        {
            wantedToppings[index++] = alwaysToppings[i];
        }

        var usedOptional = new bool[optionalToppings.Length];
        for (int i = 0; i < optionalCount; i++)
        {
            int rand;
            do
            {
                rand = Random.Range(0, optionalToppings.Length);
            } while (usedOptional[rand]);

            usedOptional[rand] = true;
            wantedToppings[index++] = optionalToppings[rand];
        }
        DisplayOrder();
    }

    void Sastified()
    {
        served = true;
        animator.SetBool("Served", true);
        animator.SetBool("Sastified", true);
        float tipAmount = CalculateTipAmount();
        GiveTips(tipAmount);
        Debug.Log("Thank You");
    }

    float CalculateTipAmount()
    {
        if (patienceDuration <= 0f)
            return minTip;

        float speedFactor = Mathf.Clamp01(patienceTimer / patienceDuration);
        float tipAmount = Mathf.Lerp(minTip, maxTip, speedFactor);
        return Mathf.Max(0f, Mathf.Round(tipAmount));
    }

    void NotSastified()
    {
        animator.SetTrigger("Unsatisfied");
        patienceTimer -= unsatisfactionPenalty * unsatisfactionMultiplier;
        unsatisfactionMultiplier += 1f;
        Debug.Log("Its not the Pizza that i wanted");
    }

    void OutOfPatience()
    {
        served = true;
        animator.SetBool("Served", true);
        animator.SetBool("Sastified", false);
        SwitchView.instance.SwitchTo(1);
        Debug.Log("This takes too long im leaving");
    }

    public void Leave()
    {
        Destroy(gameObject);
        FindAnyObjectByType<NewCustomerSpawner>().SpawnCutomer();
    }

    void GiveTips(float amount)
    {
        var tipPosObject = GameObject.Find("TipPos");
        var spawnPosition = tipPosObject != null ? tipPosObject.transform.position : transform.position;
        GameObject tips = Instantiate(tipPref, spawnPosition, Quaternion.identity);
        tips.GetComponent<Tips>().SetValue(amount);
    }

    public void Ordering()
    {
        ordering = true;
    }

    public void TriggerGameOver()
    {
        GameManager.instance.triggerGameOver();
    }

    public void Aproval(bool status)
    {
        Destroy(aprov);
        if (status)
        {
            servingAproval = true;
        }
        else
        {
            servingAproval = false;
            animator.SetBool("Aproval", false);
        }
    }
}
