using UnityEngine;

public class PizzaModel : MonoBehaviour
{
    public GameObject[] toppings;
    public bool cooked;
    public bool served;
    public Sprite cookedSprite;

    public void AddTopping(GameObject topping)
    {
        if (cooked)
        {
            return;
        }
        if (toppings != null)
        {
            foreach (GameObject t in toppings)
            {
                if (t.name == topping.name)
                {
                    return;
                }
            }
        }
        GameObject newTopping = Instantiate(topping, transform.position, Quaternion.identity);
        newTopping.name = SepparateName(newTopping.name);
        newTopping.transform.parent = transform;
        toppings = GameObject.FindGameObjectsWithTag("Topping");
    }

    string SepparateName(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }
}