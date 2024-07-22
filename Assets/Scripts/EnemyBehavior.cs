using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10;
    public float minDistance = 2;
    public int damageAmount = 20;
    public int totalHealth = 100;
    int currentHealth;
    bool alive;

    void Start()
    {
        currentHealth = totalHealth;
        alive = true;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            float step = moveSpeed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.position);

            if(distance > minDistance)
            {
                transform.LookAt(player);
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player") && alive)
        {
            // apply damage
            var playerHealth = other.gameObject.GetComponent<playerHealth>();
            playerHealth.TakeDamage(damageAmount);

            Debug.Log("hit the layer");

        }
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }

        if(currentHealth <= 0)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        alive = false;
        transform.Rotate(-90, 0, 0, Space.Self);

        Destroy(gameObject, 3);
    }
}
