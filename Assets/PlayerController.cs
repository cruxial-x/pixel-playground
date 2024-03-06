using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int maxHitPoints = 3;
    public float hitStunDuration = 0.5f; // Duration of hitstun in seconds
    public int hitPoints = 3;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float restoreDelay = 5f; // Delay time in seconds
    public int restoreAmount = 1; // Amount of hitpoints to restore
    private float lastDamageTime; // Time when the player last took damage
    private float xAxis;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private Animator animator;
    public bool isFacingLeft;
    public GameObject attackHitbox;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackHitbox.SetActive(false);
        maxHitPoints = hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.10f, whatIsGround);
        GetInput();
        Flip();
        Move(moveSpeed, rb, isGrounded);
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
        Jump(rb, isGrounded);
        Stomp();
    }
    
    void GetInput()
    {
        xAxis = Input.GetAxis("Horizontal");
    }
    public void StartRestore()
    {
        StopCoroutine("RestoreAfterDelay"); // Stop any existing restore process
        StartCoroutine("RestoreAfterDelay"); // Start a new restore process
    }

    IEnumerator RestoreAfterDelay()
    {
        while (hitPoints < maxHitPoints) // Assuming maxHitPoints is the maximum health
        {
            float waitUntil = lastDamageTime + restoreDelay; // Time to wait until

            while (Time.time < waitUntil)
            {
                yield return null; // Wait for the next frame
            }

            hitPoints += restoreAmount;
            lastDamageTime = Time.time; // Update the last damage time
        }
    }
    // Flip when the player is moving left
    void Flip()
    {
        if (xAxis < 0)
        {
            isFacingLeft = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (xAxis > 0)
        {
            isFacingLeft = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(EnableHitboxDuringAttack());
    }

    IEnumerator EnableHitboxDuringAttack()
    {
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Adjust this to match the duration of the attack animation
        attackHitbox.SetActive(false);
        animator.ResetTrigger("Attack");
    }

    // Basic platformer movement
    public void Move(float moveSpeed, Rigidbody2D rb, bool isGrounded)
    {
        rb.velocity = new Vector2(xAxis * moveSpeed, rb.velocity.y);
        animator.SetBool("Walking", xAxis != 0 && isGrounded);
    }
    public void TakeDamage(int damage)
    {
        StartCoroutine(Ouch());
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
        lastDamageTime = Time.time; // Update the last damage time
        StartRestore();
    }
    IEnumerator Ouch()
    {
        animator.SetBool("Ouch", true);
        // Change the player's color to red
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(hitStunDuration);
        // Change the player's color back to white
        GetComponent<SpriteRenderer>().color = Color.white;
        animator.SetBool("Ouch", false);
    }
    public int GetPlayerHealth()
    {
        return hitPoints;
    }
    public void Die()
    {
        // This is where you would handle the player's death
        // For example, you could play a death animation and then reload the level
        // 
        // For now, we'll just destroy the player GameObject
        Destroy(gameObject);
    }
    public void Stomp()
    {
        // Check if groundCheck overlaps with any enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.10f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // If the collider is an enemy, destroy it
                Bounce();
                Destroy(collider.gameObject);
            }
        }
    }
    public void Jump(Rigidbody2D rb, bool isGrounded)
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // Half vertical velocity when jump button is released
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    public void Bounce()
    {
        float bounceForce;
        // Check if Jump button is held down
        if (Input.GetButton("Jump"))
        {
            bounceForce = jumpForce;
        }
        else
        {
            bounceForce = jumpForce * 0.5f;
        }

        rb.velocity = new Vector2(rb.velocity.x, bounceForce);
    }
}