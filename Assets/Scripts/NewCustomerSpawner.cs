using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefs;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCutomer();
    }

    public void SpawnCutomer()
    {
        Instantiate(customerPrefs, transform.position, transform.rotation, transform);
    }
}
