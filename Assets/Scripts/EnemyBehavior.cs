using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10;
    public float attackRange = 2f;
    public float hostileRange = 15f;
    public int damageAmount = 20;
    public int totalHealth = 100;
    public float attackCooldown = 3f;
    public float attackDuration = 1f;
    public GameObject swordPrefab;

    int currentHealth;
    bool alive;
    bool canAttack;
    float attackTimer;


    void Start()
    {
        currentHealth = totalHealth;
        alive = true;
        canAttack = true;
        attackTimer = attackCooldown;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
        Debug.Log("timer" + attackTimer);
        if(alive)
        {
            float step = moveSpeed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.position);

            if(distance > attackRange && distance <= hostileRange)
            {
                transform.LookAt(player);
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }

            if(!canAttack)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    attackTimer = 0f;
                    canAttack = true;
                }
            }

            if(distance <= attackRange)
            {
                transform.LookAt(player);
                if(canAttack)
                {
                    attackPlayer();
                }
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

    private void attackPlayer()
    {
        canAttack = false;
        attackTimer = attackCooldown;

        GameObject sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
        sword.transform.SetParent(transform);

        
        sword.transform.position = transform.position + transform.right * 1.5f;

        sword.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 45, 0);

        float swingTimer = 0f;
        Quaternion startRotation = sword.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, transform.eulerAngles.y - 45, 0);

        while (swingTimer < attackDuration)
        {
            swingTimer += Time.deltaTime;
            sword.transform.rotation = Quaternion.Slerp(startRotation, endRotation, swingTimer / attackDuration);
        }

        Destroy(sword);
        
    }

    void EnemyDies()
    {
        alive = false;
        transform.Rotate(-90, 0, 0, Space.Self);

        Destroy(gameObject, 3);
    }

    private void updateTimers()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            canAttack = true;
        }
    }
}
