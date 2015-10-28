using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<Enemy> enemies = new List<Enemy>();

    public Player player;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnEnemies()
    {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            Vector3 position = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

            Enemy enemy = ((GameObject)Instantiate(enemyPrefab, position, Quaternion.identity)).GetComponent<Enemy>();

            enemies.Add(enemy);
        }
    }
}
