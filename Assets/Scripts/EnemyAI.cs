using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle, 
        Patrol,
        Chase,
        Attack
    }

    public FSMStates currentState;

    public float attackDistance = 2;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public float shootRate = 3;
    public float projectileSpeed = 99;
    GameObject player;
    public Transform enemyEyes;
    public float fieldOfView = 50f;
    public GameObject[] wanderPoints;
    public AudioClip damageSFX;

    NavMeshAgent agent;

    Vector3 nextDestination;
    //ator anim;
    float distanceToPlayer;
    float elapsedTime = 0;
    public GameObject weaponPrefab;

    EnemyHealth enemyHealth;
    int health;
    int currentDestinationIndex = 0;
    Transform enemyTransform;
    Transform deadTransform;
    bool isDead;
    bool isAttacking;

    Animator animator;

    float attackingTimer;
    bool swordActive;

    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        Debug.Log(player);
        animator = GetComponent<Animator>();

        enemyTransform = transform;

        //projectileSpawnPoint = GameObject.FindGameObjectWithTag("ProjectileSpawnPoint");

        enemyHealth = gameObject.GetComponent<EnemyHealth>();

        health = enemyHealth.currentHealth;

        isDead = false;
        isAttacking = false;
        weaponPrefab.SetActive(false);

        Initialize();


    }

    // Update is called once per frame
    void Update()
    {
        updateTimers();
        enemyTransform = transform;
        distanceToPlayer = Vector3.Distance(enemyTransform.position, 
            player.transform.position);
        health = enemyHealth.currentHealth;

        switch(currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
        }

        elapsedTime += Time.deltaTime;
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol;

        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        if(!isDead)
        {
            Debug.Log("destination " + nextDestination + " current position " + enemyTransform.position + " distance " + Vector3.Distance(enemyTransform.position, nextDestination));

            animator.SetInteger("animState", 1);

            if(Vector3.Distance(enemyTransform.position, nextDestination) < 2.1)
            {
                FindNextPoint();

                FaceTarget(nextDestination);

                agent.SetDestination(nextDestination);
            }
            else if(distanceToPlayer <= chaseDistance && IsPlayerInClearFOV())
            {
                nextDestination = player.transform.position;
                agent.SetDestination(nextDestination);
                currentState = FSMStates.Chase;
            }
        }
    }

    void UpdateChaseState()
    {
        animator.SetInteger("animState", 1);
        Debug.Log("chase");

        agent.SetDestination(nextDestination);

        if(!isDead)
        {
            Debug.Log("destination " + nextDestination + " current position " + transform.position + " distance " + distanceToPlayer);


            if(distanceToPlayer <= attackDistance)
            {
                currentState = FSMStates.Attack;
            }
            else if(distanceToPlayer > chaseDistance)
            {
                FindNextPoint();
                currentState = FSMStates.Patrol;
            }

            if(Vector3.Distance(enemyTransform.position, nextDestination) < 1.8)
            {
                if(IsPlayerInClearFOV())
                {
                    Debug.Log("looking at layer");
                    nextDestination = player.transform.position;
                }
                else
                {
                    FindNextPoint();
                    currentState = FSMStates.Patrol;
                }
                FaceTarget(nextDestination);
                Debug.Log("destination " + nextDestination + " current position " + transform.position + " distance " + distanceToPlayer);

                agent.SetDestination(nextDestination);
            }
        }
    }

    void UpdateAttackState()
    {

        if(!isDead)
        {
            nextDestination = player.transform.position;

            if(distanceToPlayer <= attackDistance)
            {
                currentState = FSMStates.Attack;
            }
            else if(distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
            {
                if(IsPlayerInClearFOV())
                {
                    currentState = FSMStates.Chase;
                }
                else
                {
                    FindNextPoint();
                    currentState = FSMStates.Patrol;
                }
            }
            else if(distanceToPlayer > chaseDistance)
            {
                currentState = FSMStates.Patrol;
            }

            FaceTarget(nextDestination);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (attackingTimer >= 0.2f)
            {
                isAttacking = true;
                weaponPrefab.SetActive(true);
                weaponPrefab.GetComponent<Collider>().enabled = true;
                Debug.Log("swing start");
            }
            else if (attackingTimer >= 0.5f)
            {
                isAttacking = false;
                weaponPrefab.SetActive(false);
                weaponPrefab.GetComponent<Collider>().enabled = false;
                Debug.Log("swing over");
            }
            
            EnemyAttack();
        }
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) 
            % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - enemyTransform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        enemyTransform.rotation = Quaternion.Slerp
            (enemyTransform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemyAttack()
    {
        if(!isDead)
        {
            if(elapsedTime >= shootRate)
            {
                Invoke("Attacking", 1);
                elapsedTime = 0.0f;
            }
        }
    }

    void Attacking()
    {
        attackingTimer = 0;
        Debug.Log("attacking layer");
        animator.SetInteger("animState", 2);
        isAttacking = true;

    }

    private void updateTimers()
    {
        attackingTimer += Time.deltaTime;
    }

    public void EnemyDied()
    {
        if(!isDead)
        {
            animator.SetInteger("animState", 3);
            isDead = true;

            Transform enemyTransform = transform;
            
            agent.enabled = false;

            Vector3 currentPosition = enemyTransform.position;
            enemyTransform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);

            Destroy(gameObject, 3);
        }
    }

    void OnDestroy()
    {
        Debug.Log("enemy destroyed");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if(Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if(Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    print("Player in sight");
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
}
