using UnityEngine;

public class Customer : MonoBehaviour
{
    public GameObject[] wantedToppings;
    public GameObject[] optionalToppings;
    public GameObject[] alwaysToppings; //Topping yang selalu ada di pizza, misal saus dan keju
    GameObject[] possibleToppings; //Semua topping yang mungkin ada di pizza, termasuk alwaysToppings dan optionalToppings

    // Start is called before the first frame update
    void Start()
    {
        SetPossibleTopping();
        ReGenerateWantedTopping();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CheckingPizza(PizzaModel pizza)
    {
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
    }

    void Sastified()
    {
        SwitchView.instance.SwitchTo(0);
        CookingBoard.instance.newPizza();
        ReGenerateWantedTopping();
        Debug.Log("Thank You");
    }

    void NotSastified()
    {
        SwitchView.instance.SwitchTo(0);
        CookingBoard.instance.newPizza();
        Debug.Log("Its not the Pizza that i wanted");
    }
}
