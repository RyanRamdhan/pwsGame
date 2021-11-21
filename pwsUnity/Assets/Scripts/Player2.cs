using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public HealthBar healthBar;
    public Player1 enemy;

    public float dashDistance = 15f;

    int maxHealth = 100;
    int curHealth;
    int damage;
    bool isBlocking;
    bool isDashing;

    public Vector2 playerLocation;
    
    
    void Start()
    {
        damage = 0;
        curHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        isBlocking = false;
    }
    void Update()
    {
        playerLocation = transform.position;
        
        

        Debug.Log(curHealth);
        Debug.Log(isBlocking);
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    

    

    


    public bool Blocking()
    {
        //isBlocking = true;
        return isBlocking;
    }

    public void GetHealth(int damage)
    {
        curHealth -= damage;
        healthBar.SetHealth(curHealth);
        
    }
}
