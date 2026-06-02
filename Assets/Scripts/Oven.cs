using UnityEngine;

public class Oven : MonoBehaviour
{
    CookingBoard plate;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Plate")
        {
            if (collision.GetComponentInChildren<PizzaModel>() != null)
            {
                if (collision.GetComponentInChildren<PizzaModel>().cooked)
                {
                    return;
                }
            }
            if (collision.GetComponent<CookingBoard>() != null)
            {
                plate = collision.GetComponent<CookingBoard>();
                plate.isCooking = true;
                Invoke("CookPizza", 3f);
            }
        }
    }

    void CookPizza()
    {
        if (plate != null)
        {
            plate.isCooking = false;
            plate.SnapBack();
            if (plate.transform.childCount > 0)
            {
                for (int i = 0; i < plate.transform.childCount; i++)
                {
                    if (plate.transform.GetChild(i).tag == "Pizza")
                    {
                        PizzaModel pizza = plate.transform.GetChild(i).GetComponent<PizzaModel>();
                        if (pizza != null)
                        {
                            pizza.cooked = true;
                            pizza.GetComponent<SpriteRenderer>().sprite = pizza.cookedSprite;
                        }
                    }
                }
            }
        }
    }
}
