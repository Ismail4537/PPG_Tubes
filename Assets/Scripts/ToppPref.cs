using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToppPref : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject toppingOn;
    [SerializeField] LayerMask pizzalayer;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + new Vector3(0, 0, 10);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, pizzalayer);
        if (hit)
        {
            PizzaModel tarPiz = hit.collider.GetComponent<PizzaModel>();
            tarPiz.AddTopping(toppingOn);
        }
        Destroy(gameObject);
    }
}
