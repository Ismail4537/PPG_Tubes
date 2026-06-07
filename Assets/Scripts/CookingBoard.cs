using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CookingBoard : MonoBehaviour, IDragHandler, IInteractAble, IEndDragHandler
{
    bool isHolded;
    Vector2 initPos;
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
        MusicManager.instance.PlayMusicTrack("Game");
        instance = this;
        currPizza = Instantiate(pizzaPref, newPizzaPos.position, Quaternion.identity);
        currPizza.transform.parent = transform;
    }

    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isHolded && currPizza != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(InputController.touchPos);
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
        transform.position = initPos;
    }

    public void NewPizza()
    {
        if (currPizza != null)
        {
            currPizza = null;
        }
        currPizza = Instantiate(pizzaPref, newPizzaPos.position, Quaternion.identity);
        currPizza.transform.SetParent(transform);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapBack();
    }
}
