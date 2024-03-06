using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f; // Speed at which the enemy moves towards the player
    private Transform player; // Reference to the player's position

    Animator animator;

    public float attackCooldown = 1.0f; // Time between attacks, in seconds
    private float attackTimer = 0.0f; // Timer to keep track of when the enemy can attack again
    public int attackDamage = 1; // Amount of damage the enemy deals to the player
    private bool easyMode = false;
    private bool stopAttacking = false;
    public float attackRange = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Knight").transform; // Assuming the player GameObject has the tag "Player"
        animator = GetComponent<Animator>();
        GameController gameController = GameObject.Find("UI").GetComponent<GameController>();
        easyMode = gameController.easyMode;
    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the enemy is more than 1 tile away from the player, move towards the player
        if (distanceToPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (distanceToPlayer < attackRange && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown; // Reset the attack timer
        }
        else
        {
            attackTimer -= Time.deltaTime; // Decrease the timer by the time since the last frame
        }
        Flip();
    }
    void Flip()
    {
        if (transform.position.x > player.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void Attack()
    {
        if(stopAttacking)
        {
            return;
        }
        if(easyMode)
        {
            if(player.GetComponent<PlayerController>().GetPlayerHealth() <= 1)
            {
                stopAttacking = true;
                return;
            }
        }
        // This is where the enemy's attack logic would go
        // For example, you could check if the player is within a certain distance and then deal damage to the player
        // 
        // For now, we'll just print a message to the console
        animator.SetTrigger("Attack");
        // Deal damage to the player
        player.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }
}