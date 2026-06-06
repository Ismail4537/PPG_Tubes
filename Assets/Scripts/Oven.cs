using UnityEngine;

public class Oven : MonoBehaviour
{
    PizzaController pizza;
    void Update()
    {
        if (pizza != null)
        {
            pizza.CookPizza();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pizza")
        {
            Debug.Log(collision.name);
            if (collision.GetComponent<PizzaController>() != null)
            {
                pizza = collision.GetComponent<PizzaController>();
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Pizza")
        {
            if (collision.GetComponent<PizzaController>() != null)
            {
                pizza = null;
            }
        }
    }
}
