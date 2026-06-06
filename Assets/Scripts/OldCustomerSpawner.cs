using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCustomerSpawner : MonoBehaviour
{
    public GameObject[] seats;
    public GameObject customerPrefs;
    public float spawnDuration;
    public float spawnTimer;

    void Start()
    {
        spawnTimer = spawnDuration;
        SpawnCustomer();
    }

    void Update()
    {
        if (HasEmptySeat())
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnCustomer();
                spawnTimer = spawnDuration;
            }
        }
        else
        {
            spawnTimer = spawnDuration;
        }
    }

    bool HasEmptySeat()
    {
        if (seats == null || seats.Length == 0)
            return false;

        foreach (GameObject seat in seats)
        {
            if (IsSeatEmpty(seat))
                return true;
        }

        return false;
    }

    bool IsSeatEmpty(GameObject seat)
    {
        if (seat == null)
            return false;

        return seat.transform.childCount == 0;
    }

    void SpawnCustomer()
    {
        if (customerPrefs == null)
        {
            Debug.LogWarning("Customer prefab is not assigned.", this);
            return;
        }

        if (seats == null || seats.Length == 0)
        {
            Debug.LogWarning("No seats assigned to CustomerSpawner.", this);
            return;
        }

        foreach (GameObject seat in seats)
        {
            if (IsSeatEmpty(seat))
            {
                GameObject customer = Instantiate(customerPrefs, seat.transform.position, seat.transform.rotation, seat.transform);
                customer.transform.SetParent(seat.transform);
                return;
            }
        }
    }
}
