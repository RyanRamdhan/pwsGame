using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    bool jump = false;
    bool crouch = false;

    float horizontalMove = 0f;
    

    // Update is called once per frame
    void Update()
    {
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
            animator.SetBool("IsJumping", true);
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
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        jump = false;
        
    }
}
