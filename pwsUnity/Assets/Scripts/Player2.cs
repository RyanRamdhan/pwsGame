using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    int maxHealth = 100;
    int curHealth;
    int damage;
    void Start()
    {
        damage = 0;
        curHealth = maxHealth;
    }
    void Update()
    {
        Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void GetHealth(int damage)
    {
        curHealth -= damage;
        
    }
}
