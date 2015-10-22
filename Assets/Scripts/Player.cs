using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    int health = 100;
    int maxHealth = 100;

    Weapon activeWeapon;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void IncreaseHealth(int amount)
    {
        ChangeHealth(amount);
    }

    public void ReceiveDamage(int damage)
    {
        ChangeHealth(-damage);
    }

    private void ChangeHealth(int amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        //Die.
    }
}
