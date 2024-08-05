using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public int damageAmount = 25;
    public AudioClip fireSFX;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(fireSFX, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            // apply damage
            var enemyHealth = other.gameObject.GetComponent<EnemyBehavior>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
