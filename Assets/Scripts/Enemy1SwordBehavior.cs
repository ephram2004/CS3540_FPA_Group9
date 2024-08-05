using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1SwordBehavior : MonoBehaviour
{
    public int damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // apply damage
            var playerHealth = other.gameObject.GetComponent<playerHealth>();
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
