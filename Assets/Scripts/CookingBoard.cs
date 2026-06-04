using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CookingBoard : MonoBehaviour, IDragHandler, IInteractAble
{
    bool isHolded;
    Vector2 initPos;
    public bool isCooking = false;
    public GameObject currPizza;
    public GameObject pizzaPref;
    public Transform newPizzaPos;
    static public CookingBoard instance;
    void Awake()
    {
        initPos = transform.position;
    }
    void Start()
    {
        instance = this;
        currPizza = Instantiate(pizzaPref, newPizzaPos.position, Quaternion.identity);
        currPizza.transform.parent = transform;
    }

    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isHolded && !isCooking && currPizza != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector2(mousePos.x, 0);
        }
    }

    public void SnapBack()
    {
        transform.position = initPos;
    }

    public void OnInteract()
    {
        isHolded = true;
    }

    public void NotInteract()
    {
        isHolded = false;
        if (!isCooking)
        {
            transform.position = initPos;
        }
    }

    public void NewPizza()
    {
        currPizza = Instantiate(pizzaPref, newPizzaPos.position, Quaternion.identity);
        currPizza.transform.parent = transform;
    }
}
