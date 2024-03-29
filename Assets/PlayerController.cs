using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public TMPro.TextMeshProUGUI coinText;
    public int coins = 0;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool easyMode = false;
    private GameController gameController;
    private bool touchJump;
    private bool touchJumpReleased;
    public Transform attackPoint;
    public Vector2 attackArea;
    public float timeBetweenAttack = 0.2f;
    private float timeSinceLastAttack;
    private bool attack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxHitPoints = hitPoints;
        gameController = GameObject.Find("UI").GetComponent<GameController>();
        easyMode = gameController.easyMode;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.position, attackArea);
    }
    void LateUpdate(){
        // Check if the player has fallen off the bottom of the screen
        if (transform.position.y < -5)
        {
            if(easyMode)
            {
            transform.position = new Vector2(transform.position.x, 5);
            }
            else
            {
                Die();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.10f, whatIsGround);
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        GetInput();
        Flip();
        Attack();
        StartJump(rb);
        EndJump(rb);
        Stomp();
        coinText.text = coins.ToString("D2");
    }
    void FixedUpdate()
    {
        Move(moveSpeed, rb);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with " + other.tag);
        // Check if the player collided with a coin
        if (other.gameObject.CompareTag("Coin"))
        {
            // Destroy the coin
            Destroy(other.gameObject);

            // Increment the player's score
            coins++;
        }
    }
    void GetInput()
    {
        xAxis = Input.GetAxis("Horizontal");
        attack = Input.GetButtonDown("Fire1");
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
        timeSinceLastAttack += Time.deltaTime;
        if(timeSinceLastAttack >= timeBetweenAttack && attack)
        {
            animator.SetTrigger("Attack");
            timeSinceLastAttack = 0;
        }
    }
    #pragma warning disable IDE0051 // Used in Knight_Attacking animation event
    private void Hit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackArea, 0);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Destroy(enemy.gameObject);
            }
        }
    }
    #pragma warning restore IDE0051

    // Basic platformer movement
    public void Move(float moveSpeed, Rigidbody2D rb)
    {
        animator.SetBool("Walking", xAxis != 0 && coyoteTimeCounter > 0);

        rb.velocity = new Vector2(xAxis * moveSpeed, rb.velocity.y);

        rb.position = PixelSnapper.SnapToPixelGrid(rb.position);
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
        hitPoints = 0;
        Destroy(gameObject);
        gameController.GameOver();
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
    public void StartJump(Rigidbody2D rb)
    {
        if ((Input.GetButtonDown("Jump") || touchJump) && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void EndJump(Rigidbody2D rb)
    {
        // Half vertical velocity when jump button is released
        if ((Input.GetButtonUp("Jump") || touchJumpReleased) && rb.velocity.y > 0)
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
