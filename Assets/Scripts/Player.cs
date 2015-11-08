using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    int health = 100;
    int maxHealth = 100;

    public Weapon activeWeapon;

	// Use this for initialization
	void Start ()
    {
        NotificationCenter.DefaultCenter().PostNotification(gameObject, "PlayerHealthChanged", health.ToString());
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 0 for auto, 1 for manual
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            activeWeapon.Shoot();

        if (Input.GetKeyDown(KeyCode.R))
            activeWeapon.Reload();
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

        NotificationCenter.DefaultCenter().PostNotification(gameObject, "PlayerHealthChanged", health.ToString());

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
