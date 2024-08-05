using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject ultAbilityPrefab;
    public float projectileSpeed = 100;

    public float attackCooldown = 2f;
    public float ultAbilityCooldown = 25f;

    float attackTimer;
    float ultAbilityTimer;

    bool canAttack;
    bool canUseUltAbility;
    
    void Start()
    {
        attackTimer = attackCooldown;
        ultAbilityTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
        if(Input.GetButtonDown("Fire1") && canAttack)
        {
            attack();
        }
        if(Input.GetKeyDown(KeyCode.Q) && canUseUltAbility)
        {
            Debug.Log("using ult");
            useUltAbility();
        }
    }

    private void attack()
    {
        canAttack = false;
        attackTimer = attackCooldown;

        GameObject projectile = Instantiate(projectilePrefab, 
            transform.position + transform.forward, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(
            GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

    private void useUltAbility()
    {
        canUseUltAbility = false;
        ultAbilityTimer = ultAbilityCooldown;

        GameObject ult = Instantiate(ultAbilityPrefab, 
            transform.position, transform.rotation) as GameObject;

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        ult.transform.SetParent(player);
        ult.transform.localPosition = Vector3.zero;
    }

    private void updateTimers()
    {
        attackTimer -= Time.deltaTime;
        ultAbilityTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            canAttack = true;
        }
        if (ultAbilityTimer <= 0)
        {
            canUseUltAbility = true;
        }
    }
}
