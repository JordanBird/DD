using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldManager : MonoBehaviour
{
    private EnemyManager enemyManager;
    private PeopleManager peopleManager;

    public GameObject playerPrefab;
    public GameObject hellPrefab;

    public GameObject earthContainer;
    public GameObject hellContainer;

    public Image blackFade;

    private GameObject currentHell;

    private Player player;

	// Use this for initialization
	void Start ()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        peopleManager = FindObjectOfType<PeopleManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void DestroyEarth()
    {
        Destroy(earthContainer);
    }

    public IEnumerator StartGame()
    {
        DestroyEarth();
        yield return StartCoroutine(FadeTo());
        DestroyMainCameraAndInstantiatePlayer();
        peopleManager.ClearPeople();
        GenerateHell();
        player.transform.position = new Vector3(0, 100, 0);
        yield return StartCoroutine(FadeFrom());
        SpawnEnemies();
    }

    public IEnumerator FadeTo()
    {
        while (blackFade.color.a < 1)
        {
            blackFade.color = new Color(0, 0, 0, blackFade.color.a + 0.5f * Time.deltaTime);

            yield return null;
        }
    }

    public IEnumerator FadeFrom()
    {
        while (blackFade.color.a > 0)
        {
            blackFade.color = new Color(0, 0, 0, blackFade.color.a - 0.1f * Time.deltaTime);

            yield return null;
        }
    }

    public void DestroyMainCameraAndInstantiatePlayer()
    {
        player = ((GameObject)Instantiate(playerPrefab, Camera.main.transform.position, Quaternion.identity)).GetComponent<Player>();

        enemyManager.player = player;

        Destroy(Camera.main.gameObject);
    }

    public void DestroyHell()
    {
        Destroy(currentHell);
    }

    public void GenerateHell()
    {
        currentHell = (GameObject)Instantiate(hellPrefab, Vector3.zero, Quaternion.identity);
    }

    public void SpawnEnemies()
    {
        enemyManager.SpawnEnemies();
    }
}
