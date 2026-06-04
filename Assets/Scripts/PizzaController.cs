using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PizzaController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 initialPos;
    PizzaModel pizzaModel;
    public LayerMask placeableLayer;
    void Start()
    {
        initialPos = transform.position;
        pizzaModel = GetComponent<PizzaModel>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, placeableLayer);
        if (hit)
        {
            if (pizzaModel.cooked && hit.collider.CompareTag("Plate"))
            {
                transform.position = hit.collider.transform.position;
                transform.parent = hit.collider.transform;
                pizzaModel.served = true;
                FindObjectOfType<CookingBoard>().currPizza = null;
            }
            else if (pizzaModel.served && hit.collider.GetComponent<Customer>() != null)
            {
                Customer cus = hit.collider.GetComponent<Customer>();
                cus.CheckingPizza(pizzaModel);
                Destroy(gameObject);
            }
            else if (hit.collider.CompareTag("Trash"))
            {
                FindObjectOfType<CookingBoard>().currPizza = null;
                FindObjectOfType<CookingBoard>().NewPizza();
                Destroy(gameObject);
            }
            else
            {
                transform.position = initialPos;
            }
        }
        else
        {
            transform.position = initialPos;
        }
    }
}