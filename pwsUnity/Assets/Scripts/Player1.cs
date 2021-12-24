using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{   

    PlayerInputs input;
    public HealthBar1 healthBar;
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb;
    public Player2 enemy;

    Vector2 currentMovement;
    Vector2 currentAttack;

    
    private float horizontalMove;
    private float nextAttackTime;
    private float nextJumpTime;
    private bool isJumping;
    private bool isTouching;
    private bool attackPressed;   
    private int currentHealth;
    public bool isBlocking = false;


    void Awake()
    {
        input = new PlayerInputs();

        input.CharacterControls.Movement.performed += ctx => currentMovement = ctx.ReadValue<Vector2>();
        input.CharacterControls.Movement.canceled += ctx => currentMovement = Vector2.zero;

        input.CharacterControls.Attack.performed += ctx =>
        {
            currentAttack = ctx.ReadValue<Vector2>();
            attackPressed = currentAttack.x != 0 || currentAttack.y != 0;
        };
        input.CharacterControls.Attack.canceled += ctx => currentAttack = Vector2.zero;
    }

    private void Start()
    {
        int maxHealth = 100;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnEnable() {
        input.CharacterControls.Enable();
    }

    void OnDisable() {
        input.CharacterControls.Disable();
    }

    void Update() {
        
        Move();
        AttackInput();
        Blocking();
        
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, isJumping);
        isJumping = false;
    }

    public void GetHealth(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetHealth(currentHealth);
    }

    void Move(){
        float runSpeed = 10f;

    horizontalMove = currentMovement.x * runSpeed;

        bool isCrouching = false;
        float cooldownTime = 1.4f;
        if (Time.time > nextJumpTime)
        {
            if (currentMovement.y > 0)
            {
                isJumping = true;
                animator.SetBool("IsJumping", true);

                nextJumpTime = Time.time + cooldownTime;
            }
        }
        if (currentMovement.y < 0)
        {
            isCrouching = true;
            animator.SetBool("IsCrouching", true);
        }
        if (currentMovement.y >= 0)
        {
            isCrouching = false;

            animator.SetBool("IsCrouching", false);
        }
        animator.SetFloat("PlayerMovement", currentMovement.x);
    }

    void AttackInput()
    {
        int damage = 0;
        float cooldownTime = 0.2f;

        if (Time.time > nextAttackTime)
        {
            if (currentAttack.x == -1)
            {         
                damage = 10;
            }
            if (currentAttack.y == 1)
            {          
                damage = 20;
            }
            if (currentAttack.x == 1)
            {           
                damage = 30;
            }

            animator.SetInteger("PlayerDamage", damage);
            if (attackPressed)
            {
                if (isTouching && enemy.Blocking())
                {
                    enemy.GetHealth(damage);
                    
                    nextAttackTime = Time.time + cooldownTime;
                }
                
            }
        }
        
        
    }

    public bool Blocking()
    {
        float playerLocation = transform.position.x;
        float enemyLocation = enemy.transform.position.x;
        
    float playerDistance = enemyLocation - playerLocation;
        if(playerDistance <= 0)
        {
            if (currentMovement.x < 0)
            {
                isBlocking = true;
            }
            else if (currentMovement.x > 0)
            {
                isBlocking = false;
            }
        }
        if (playerDistance > 0)
        {
            if (currentMovement.x > 0)
            {
                isBlocking = true;
            }
            else if (currentMovement.x < 0)
            {
                isBlocking = false;
            }
        }


        return isBlocking;
    }



    public void OnLanding()
    {
        isJumping = false;
        animator.SetBool("IsJumping", false);
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouching = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouching = false;
        }
    }
}
