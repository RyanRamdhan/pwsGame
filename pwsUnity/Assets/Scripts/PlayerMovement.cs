using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{  
    public Player2 enemy;
    //public Controls controls;
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb;
    
    public float runSpeed = 10f;
    public int maxHealth = 100;
    public float dashDistance = 1f;

    
    float horizontalMove = 0f;   
    int curHealth;
    int damage = 0;
    int buttonCount = 0;
    bool attacking;
    bool isJumping;
    bool isCrouching;
    bool isTouching;
    bool isBlocking;
    bool isDashing;
    float doubleTapTime = 0.3f;
    float playerDistance;
    float dashDirection;

    

    KeyCode lastKeyCode;

    Vector2 move;
    Vector2 attackBtn;
    Vector2 playerLocation;

    void Awake()
    {
        //controls = new Controls();

        //controls.Gameplay.Move.performed += context => move = context.ReadValue<Vector2>();
        //controls.Gameplay.Move.canceled += context => move = Vector2.zero;

        //controls.Gameplay.Attack.performed += context => attackBtn = context.ReadValue<Vector2>();
        //controls.Gameplay.Attack.canceled += context => attackBtn = Vector2.zero;

        

       
        

    }

    void OnEnable()
    {
        //controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        //controls.Gameplay.Disable();
    }


    // Update is called once per frame
    void Update()
    {    
        
        
        //Dashing();
        playerLocation = transform.position;
        float enemyLocation = enemy.transform.position.x;
        playerDistance = enemyLocation - playerLocation.x;
        Attack();

        
        
        
        Move();
        Blocking();
        

    }
    void FixedUpdate()
    {
        
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, isJumping);
        isJumping = false;
        
    }

    void Move(){
        horizontalMove = move.x * runSpeed;
         animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(move.y > 0){
            isJumping = true;
            //animator.SetBool("IsJumping", true);
        }
        if(move.y < 0){
            isCrouching = true;
            animator.SetBool("IsCrouching", true);
        }
        if(move.y >= 0){
            isCrouching = false;
            
            animator.SetBool("IsCrouching", false);
        } 
    }
    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void Attack()
    {
        //var attackBtn = controls.Gameplay.Attack.ReadValue<Vector2>();
        Debug.Log(attacking);
        Debug.Log(attackBtn);

        if (attackBtn.x == -1)
        {
            attacking = true;
            damage = 10;
        }
        if (attackBtn.y == 1)
        {
            attacking = true;
            damage = 20;
        }
        if (attackBtn.x == 1)
        {
            attacking = true;
            damage = 30;
        }

        if (attacking == true)
        {
            if(isTouching == true)
            {
                enemy.GetHealth(damage);
                
                damage = 0;
                attacking = false;       
            }
        }

        attackBtn = Vector2.zero;
        
    }

    void Blocking(){
        if(playerDistance >= 0){
            if(move.x < 0){
                isBlocking = true;
            }
            else if(move.x > 0){
                isBlocking = false;
            }
        }
        else if(playerDistance < 0){
            if(move.x > 0){
                isBlocking = true;
            }
            else if(move.x < 0){
                isBlocking = false;
            }
        }
    }

    public void Dashing()
    {
        //float dashDirection = controls.Gameplay.Dash.ReadValue<float>();
        Debug.Log(dashDirection);

        if (dashDirection == 1){
            
            if (doubleTapTime > 0 && buttonCount == 1/*Number of Taps you want Minus One*/){
              StartCoroutine(Dash(1f));
             }else{
             doubleTapTime = 0.4f ; 
            buttonCount += 1 ;
             }
        }

        if (dashDirection == -1){
 
            if (doubleTapTime > 0 && buttonCount == 1/*Number of Taps you want Minus One*/){
              StartCoroutine(Dash(-1f));
             }else{
             doubleTapTime = 0.4f ; 
            buttonCount += 1 ;
             }
        }
 
        if (doubleTapTime > 0)
        {
 
            doubleTapTime -= 1 * Time.deltaTime;
 
        }else{
            buttonCount = 0 ;
        }
        
    }

    // health
    public void GetHealth()
    {
        curHealth = maxHealth;
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
    
    IEnumerator Dash(float direction){
        isDashing = true;
        
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.15f);
        isDashing = false;
        
        rb.gravityScale = gravity;
    }

    

    

    
}
