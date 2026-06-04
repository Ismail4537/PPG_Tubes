using UnityEngine;

public class ToppingOn : MonoBehaviour
{
    public string toppingName;
    // Start is called before the first frame update
    void Start()
    {
        toppingName = gameObject.name;
        Debug.Log(toppingName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
