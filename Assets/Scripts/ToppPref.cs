using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToppPref : MonoBehaviour
, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject toppingOn;
    [SerializeField] LayerMask pizzalayer;
    bool dragging;

    void FixedUpdate()
    {
        if (dragging) return;
        if (InputController.touchDown == true)
        {
            transform.position = Camera.main.ScreenToWorldPoint(InputController.touchPos) + new Vector3(0, 0, 10);
        }
        if (InputController.touchDown == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, pizzalayer);
            if (hit)
            {
                SFXManager.instance.PlayClip2D("Toppings");
                PizzaModel tarPiz = hit.collider.GetComponent<PizzaModel>();
                tarPiz.AddTopping(toppingOn);
            }
            Destroy(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        transform.position = Camera.main.ScreenToWorldPoint(InputController.touchPos) + new Vector3(0, 0, 10);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, pizzalayer);
        if (hit)
        {
            SFXManager.instance.PlayClip2D("Toppings");
            PizzaModel tarPiz = hit.collider.GetComponent<PizzaModel>();
            tarPiz.AddTopping(toppingOn);
        }
        Destroy(gameObject);
    }
}
