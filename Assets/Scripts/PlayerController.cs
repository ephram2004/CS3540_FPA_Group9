using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float jumpHeight = 10;
    public float gravity = 9.81f;
    public float airControl = 10;
    public GameObject weaponPrefab;
    public GameObject ultAbilityPrefab;
    public float projectileSpeed = 100;

    public float attackCooldown = 2f;
    public float ultAbilityCooldown = 25f;

    float attackTimer;
    float ultAbilityTimer;

    bool canAttack;
    bool canUseUltAbility;
    bool isAttacking;

    Animator animator;

    CharacterController controller;
    Transform cameraTransform;
    Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = transform.Find("Player1Model").GetComponent<Animator>();
        cameraTransform = transform.Find("Main Camera").transform;

        attackTimer = attackCooldown;
        ultAbilityTimer = 0;
        bool isAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
        if(Input.GetButtonDown("Fire1") && canAttack)
        {
            Debug.Log("attacking");
            attack();
            isAttacking = true;
            weaponPrefab.SetActive(true);
            weaponPrefab.GetComponent<Collider>().enabled = true;
        }
        if(Input.GetKeyDown(KeyCode.Q) && canUseUltAbility)
        {
            Debug.Log("using ult");
            useUltAbility();
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward.normalized;
        Vector3 camRight = cameraTransform.right.normalized;

        input = camRight * moveHorizontal + camForward * moveVertical;
        input *= moveSpeed;

        if(controller.isGrounded) 
        {
            moveDirection = input;
            
            if(Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        if (!isAttacking)
        {
            if (Mathf.Abs(input.x) > 0.1f || Mathf.Abs(input.z) > 0.1f)
            {
                animator.SetInteger("animState", 1); // Running
            }
            else
            {
                animator.SetInteger("animState", 0); // Idle
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
        {
            isAttacking = false;
            weaponPrefab.GetComponent<Collider>().enabled = false;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void attack()
    {
        canAttack = false;
        attackTimer = attackCooldown;

        animator.SetInteger("animState", 2);
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
