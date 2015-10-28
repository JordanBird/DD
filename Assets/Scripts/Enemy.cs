using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    EnemyManager enemyManager;

    [SerializeField]
    int maxHealth = 0;
    [SerializeField]
    int health = 100;

    float speed = 5;

	// Use this for initialization
	void Start ()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        MoveTowardsPlayer();
    }

    public void ReceiveDamage(int amount)
    {
        ChangeHealth(-amount);
    }

    private void ChangeHealth(int amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemyManager.player.transform.position, 5 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.ReceiveDamage(1);
        }
    }
}
