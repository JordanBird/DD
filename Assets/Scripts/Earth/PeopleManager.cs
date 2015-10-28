using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    public WorldManager worldManager;

    public GameObject personPrefab;
    public Rect spawnZone = new Rect(-10, -10, 20, 20);

    [SerializeField]
    List<Person> people = new List<Person>();

	// Use this for initialization
	void Start ()
    {
        worldManager = FindObjectOfType<WorldManager>();

        int peopleToSpawn = Random.Range(5, 15);

        for (int i = 0; i < peopleToSpawn; i++)
        {
            SpawnPerson(GetRandomPositionInSpawnArea());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void SpawnPerson(Vector3 spawnPosition)
    {
        GameObject person = Instantiate(personPrefab, spawnPosition, Quaternion.identity) as GameObject;
        Person pComponent = person.GetComponent<Person>();

        people.Add(pComponent);
    }

    void DrawGizmosOnSelected()
    {
        Gizmos.DrawWireCube(new Vector3(spawnZone.width - spawnZone.x, 0, spawnZone.height - spawnZone.y), new Vector3((spawnZone.width - spawnZone.x) / 2, 0, (spawnZone.height - spawnZone.y) / 2));
    }

    public void MoveMainCameraTowardsPerson(Person person)
    {
        StopAllCoroutines();
        StartCoroutine(MoveMainCameraToView(person));
    }

    private IEnumerator MoveMainCameraToView(Person person)
    {
        Camera.main.transform.parent = person.head.transform;

        while (Camera.main.transform.position != person.head.transform.position)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, person.head.transform.position, 5f * Time.deltaTime);

            Vector3 relativePos = person.head.transform.position - Camera.main.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotation, Time.deltaTime);

            yield return null;
        }

        while (Camera.main.transform.rotation != person.transform.rotation)
        {
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, person.transform.rotation, 50f * Time.deltaTime);

            yield return null;
        }

        person.controller.enabled = true;
        person.StopAllCoroutines();
        person.GetComponent<Rigidbody>().useGravity = true;

        StartCoroutine(RemoveUnseenPeople(person));
    }

    public Vector3 GetRandomPositionInSpawnArea()
    {
        return new Vector3(Random.Range(spawnZone.x, spawnZone.width), 1.8f, Random.Range(spawnZone.y, spawnZone.height));
    }

    public IEnumerator RemoveUnseenPeople(Person currentPerson)
    {
        people.Remove(currentPerson);

        while(people.Count > 0)
        {
            for (int i = 0; i < people.Count; i++)
            {
                if (people[i].spriteRenderer.isVisible)
                    continue;

                Destroy(people[i].gameObject);
                people.RemoveAt(i);
                i--;
            }

            yield return null;
        }

        people.Add(currentPerson);

        yield return new WaitForSeconds(Random.Range(4f, 7f));

        StartCoroutine(worldManager.StartGame());
    }

    public void ClearPeople()
    {
        while (people.Count > 0)
        {
            Destroy(people[0].gameObject);
            people.RemoveAt(0);
        }
    }
}
