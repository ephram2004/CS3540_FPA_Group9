using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1UltBehavior : MonoBehaviour
{
    public float ultDuration;
    public float damageInterval;
    public int damageAmount;

    float ultTimer;
    float damageTimer;
    // Start is called before the first frame update
    void Start()
    {
        ultTimer = ultDuration;
        damageTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
    }

    private void applyDamage()
    {
        Debug.Log("damaging");
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);

        foreach (Collider collider in collidersInRadius)
        {
            if(collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyBehavior>().TakeDamage(damageAmount);
            }
        }   
    }

    private void updateTimers()
    {
        ultTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;
        if (ultTimer <= 0)
        {
            Debug.Log("ult over");
            Destroy(gameObject);
        }
        if(damageTimer <= 0)
        {
            applyDamage();
            damageTimer = damageInterval;
        }
    }
}
