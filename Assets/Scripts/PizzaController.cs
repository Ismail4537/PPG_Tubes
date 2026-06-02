using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PizzaController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 initialPos;
    PizzaModel pizzaModel;
    public LayerMask plateLayer;
    void Start()
    {
        initialPos = transform.position;
        pizzaModel = GetComponent<PizzaModel>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pizzaModel.cooked)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (pizzaModel.cooked)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, plateLayer);
            if (hit)
            {
                transform.position = hit.collider.transform.position;
                transform.parent = hit.collider.transform;
                FindObjectOfType<CookingBoard>().currPizza = null;
            }
            else
            {
                transform.position = initialPos;
            }
        }
    }
}
