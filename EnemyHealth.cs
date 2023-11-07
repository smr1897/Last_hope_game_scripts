using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    RagdollManager ragdollManager;
    [HideInInspector] public bool isDead;
    public Animator enemyAnim;

    private void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                EnemyDeath();
            }
            else 
            { 
                Debug.Log("Hit");
            }
        }
    }

    void EnemyDeath()
    {
        enemyAnim.SetBool("Die", true);
        //ragdollManager.TriggerRagdoll();
        Debug.Log("Death");
        Object.Destroy(gameObject,1f);
    }
}
