using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    [Header("Enemy Health and Damage")]

    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage;
    public GameObject enemy;

    [Header("Enemy Things")]

    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public Camera ShootingRayCastArea;
    public Transform playerBody;
    public LayerMask playerLayer;

    [Header("Enemy Guarding War")]

    public GameObject[] walkPoints;
    int currentEnemyPosition = 0;
    public float enemySpeed;
    float walkingPointRadius = 2;

    [Header("Sounds and UI")]

    [Header("Enemy Shooting ")]

    public float timeBetweenShoot;
    bool previouslyShot;

    [Header("Enemy Animation and Spark Effect")]

    public Animator anim;

    [Header("Enemy mood/situation")]

    public float visionRadius; //Sight radius for enemy
    public float shootingRadius; //when enemy comes to the shooting radius enemy starts shooting
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;

    private void Awake()
    {
        presentHealth = enemyHealth;
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<GameObject>();
    }

    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position , visionRadius , playerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position , shootingRadius , playerLayer);

        if(!playerInVisionRadius && !playerInShootingRadius)
        {
            Guard();
        }

        if(playerInVisionRadius && !playerInShootingRadius)
        {
            PursuePlayer();
        }

        if(playerInVisionRadius && playerInShootingRadius)
        {
            shootAtPlayer();
        }
    }

    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentEnemyPosition].transform.position , transform.position) < walkingPointRadius)
        {
            currentEnemyPosition =  Random.Range(0 , walkPoints.Length);
            if(currentEnemyPosition >= walkPoints.Length) 
            {
                currentEnemyPosition = 0;
            }
            
        }
        transform.position = Vector3.MoveTowards(transform.position , walkPoints[currentEnemyPosition].transform.position , Time.deltaTime * enemySpeed);

        transform.LookAt(walkPoints[currentEnemyPosition].transform.position); //after enemy walk to one walkpoint he will turn back and wakl back agein
           
    }

    private void PursuePlayer()
    {
        if(enemyAgent.SetDestination(playerBody.position))
        {
            //animations
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Shoot", false);
            //anim.SetBool("AimDie", false);
            anim.SetBool("Die", false);

            //vision and shooting radius increase code
            visionRadius = 30f;
            shootingRadius = 16f;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            //anim.SetBool("AimDie", true);
            anim.SetBool("Die", true);
        }
    }

    private void shootAtPlayer()
    {
        enemyAgent.SetDestination(transform.position); //Enemy will stop to shoot

        transform.LookAt(LookPoint); //makes Enemy look at player

        if(!previouslyShot) //if we did not shot a raycast then we want to fire a raycast
        {
            RaycastHit hit;
            
            if(Physics.Raycast(ShootingRayCastArea.transform.position , ShootingRayCastArea.transform.forward, out hit , shootingRadius))
            {
                //The limit of  the raycast will be the shooting radius

                Debug.Log("shooting" + hit.transform.name);

                MovementStateManager playerBody = hit.transform.GetComponent<MovementStateManager>();

                if(playerBody != null) 
                {
                    playerBody.playerHitDamage(5f);
                }

                anim.SetBool("Walk", true);
                anim.SetBool("AimRun", false);
                anim.SetBool("Shoot", false);
                //anim.SetBool("AimDie", false);
                anim.SetBool("Die", false);

            }

            //this will fire a raycast in every timeBetweenShoot
            //If we set the timeBetweenShoot to 5 then it will fire a raycast in every five seconds
            previouslyShot = true; //after we fire a raycast then the shooting is done
            Invoke(nameof(activeShooting), timeBetweenShoot);


        }
    }

    private void activeShooting()
    {
        //we have to set the previouslyShot to false because then the enemy can shoot again
        previouslyShot = false; 
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if(presentHealth <= 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            //anim.SetBool("AimDie", true);
            anim.SetBool("Die", true);

            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius= 0f;
        playerInVisionRadius = false;
        playerInShootingRadius = false;
        //Object.Destroy(enemy , 3f);
    }
}
