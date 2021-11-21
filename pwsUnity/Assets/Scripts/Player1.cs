using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;



public class Player1 : MonoBehaviour
{
    public Player2 enemy;
    public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D rb;
    
    
    
    public float runSpeed = 10f;
    public int maxHealth = 100;
    public float dashDistance = 10f;

    bool jump = false;
    bool crouch = false; 
    float horizontalMove = 0f;   
    int curHealth;
    int damage;
    bool attacking;
    bool isTouching;
    bool isBlocking;
    bool isDashing;
    float doubleTapTime;

   
    

    KeyCode lastKeyCode;

    



    void Start()
    {
        damage = 0;
        

    }
    void Update()
    {
        Attack();
        Move();
        Debug.Log(jump);
        Dashing();
    }

    

    void Move(){

        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;     
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Horizontal"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            //animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        } else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }
        

    }
    

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        if(!isDashing)    
            {controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);}
        
        jump = false;
        
    }

    // health
    public void GetHealth()
    {
        curHealth = maxHealth;
    }

    //attacking and damage
    public void Attack()
    {
        /*if (Input.GetKeyDown("j"))
        {
            attacking = true;
            damage = 10;
        }
        if (Input.GetKeyDown("k"))
        {
            attacking = true;
            damage = 20;
        }
        if (Input.GetKeyDown("l"))
        {
            attacking = true;
            damage = 30;
        }

        if (attacking == true)
        {
            if(enemy.Blocking() == false)
            {
                enemy.GetHealth(damage);
                
                damage = 0;
                attacking = false;       
            }
        }
        */
    }

    public void Dashing()
    {
        //Dashing Left
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(doubleTapTime > Time.time && lastKeyCode == KeyCode.A){
                StartCoroutine(Dash(-1f));
            }else{
                doubleTapTime = Time.time + 0.2f;
            }
            lastKeyCode = KeyCode.A;
        }

        //Dashing Right
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(doubleTapTime > Time.time && lastKeyCode == KeyCode.D){
                StartCoroutine(Dash(1f));
            }else{
                doubleTapTime = Time.time + 0.2f;
            }
            lastKeyCode = KeyCode.D;
        }
    }


    public void Blocking()
    {
        if(Input.GetAxisRaw("Horizontal") == -1)
        {
            isBlocking = true;


        }else
        {
            isBlocking = false;
        }
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
