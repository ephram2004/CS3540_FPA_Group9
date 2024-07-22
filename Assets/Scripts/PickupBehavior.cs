using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public static int totalCoins = 0;
    public float rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        totalCoins++;
        Debug.Log("Total Coins " + totalCoins);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " triggered me!");
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        totalCoins--;
        Debug.Log("Pickups remaining " + totalCoins);
    }
}
