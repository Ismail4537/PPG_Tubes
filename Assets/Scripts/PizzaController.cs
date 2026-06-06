using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PizzaController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 initialPos;
    PizzaModel pizzaModel;
    public LayerMask placeableLayer;
    public float cookTarget;
    public float burntTarget;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        initialPos = transform.position;
        pizzaModel = GetComponent<PizzaModel>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                initialPos = hit.collider.transform.position;
                transform.position = hit.collider.transform.position;
                transform.parent = hit.collider.transform;
                pizzaModel.served = true;
                FindObjectOfType<CookingBoard>().currPizza = null;
            }
            else if (pizzaModel.served && hit.collider.GetComponent<Customer>() != null)
            {
                Customer cus = hit.collider.GetComponent<Customer>();
                if (cus.servingAproval)
                {
                    cus.CheckingPizza(pizzaModel);
                    DestroyMe();
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
        else
        {
            transform.position = initialPos;
        }
    }

    void DestroyMe()
    {
        CookingBoard.instance.NewPizza();
        Destroy(GameObject.Find("Tutorial"));
        Destroy(gameObject);
    }

    public void CookPizza()
    {
        pizzaModel.cookTime += Time.deltaTime;
        if (pizzaModel.cookTime > cookTarget)
        {
            pizzaModel.cooked = true;
            spriteRenderer.sprite = pizzaModel.cookedSprite;

        }
        if (pizzaModel.cookTime > burntTarget)
        {
            pizzaModel.burnt = true;
            spriteRenderer.color = pizzaModel.burntColor;
        }
    }
}