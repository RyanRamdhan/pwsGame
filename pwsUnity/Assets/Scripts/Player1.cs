using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public Player2 enemy;

    int maxHealth = 100;
    int curHealth;
    int damage;
    bool attacking;
    bool isTouching;
    void Start()
    {
        damage = 0;
        attacking = false;
        isTouching = true;
    }
    void Update()
    {
        Attack();
        
        
    }
    public void GetHealth()
    {
        curHealth = maxHealth;
    }
    public void Attack()
    {
        if (Input.GetKeyDown("j"))
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
            enemy.GetHealth(damage);
            damage = 0;
            attacking = false;
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
}
