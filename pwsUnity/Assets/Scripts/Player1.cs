using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



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

    private float runSpeed = 10f;
    private float horizontalMove;
    private float nextAttackTime;
    private float nextJumpTime;
    private bool isJumping;
    private bool isTouching;
    private bool attackPressed;
    private int damage;
    private float playerLocation;
    private float enemyLocation;
    
    private int currentHealth;
    private int maxHealth = 100;
    public bool isBlocking;

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
        //Debug.Log(currentMovement);
        
        Move();
        AttackInput();
        Blocking();
        playerLocation = transform.position.x;
        enemyLocation = enemy.transform.position.x;
        Debug.Log(currentAttack);
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

    public void GetHealth(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void Move(){

        
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
        
    }

    void AttackInput()
    {
        
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

            if (attackPressed)
            {
                if (isTouching && enemy.Blocking())
                {
                    enemy.GetHealth(damage);
                    damage = 0;
                    nextAttackTime = Time.time + cooldownTime;
                }
                
            }
        }
    }

    public bool Blocking()
    {
        
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
