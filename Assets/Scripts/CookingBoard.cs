using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CookingBoard : MonoBehaviour, IDragHandler
{
    public InputAction touch, pos;
    Vector3 currPos;
    Camera mainCam;
    bool isHolded;
    Vector2 initPos;
    public bool isCooking = false;
    public GameObject currPizza;
    public GameObject pizzaPref;
    public Transform newPizzaPos;
    void Awake()
    {
        mainCam = Camera.main;
        touch.Enable();
        pos.Enable();
        initPos = transform.position;
    }
    void Start()
    {
        currPizza = Instantiate(pizzaPref, newPizzaPos.position, Quaternion.identity);
        currPizza.transform.parent = transform;
        pos.performed += context =>
        {
            currPos = context.ReadValue<Vector2>();
        };
    }

    void Update()
    {
        touch.performed += _ =>
        {
            if (checkClick())
            {
                isHolded = true;
            }
        };
        touch.canceled += _ =>
        {
            isHolded = false;
            if (!isCooking)
            {
                transform.position = initPos;
            }
        };
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

    bool checkClick()
    {
        Ray ray = mainCam.ScreenPointToRay(currPos);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            if (hit.collider.transform == transform)
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }
}
