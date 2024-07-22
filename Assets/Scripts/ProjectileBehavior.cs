using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damageAmount = 25;
    static int numHits;
    // Start is called before the first frame update
    void Start()
    {
        numHits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            numHits += 1;
            // apply damage
            var enemyHealth = other.gameObject.GetComponent<EnemyBehavior>();
            enemyHealth.TakeDamage(damageAmount);

            Debug.Log("Hit the enemy " + numHits + " times.");

        }
    }
}
