using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour
{
    PeopleManager peopleManager;
    public GameObject head;

    Person person;
    public DDPlayerController controller;
    public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start ()
    {
        peopleManager = FindObjectOfType<PeopleManager>();

        head = transform.FindChild("Head").gameObject;
        person = GetComponent<Person>();
        controller = GetComponent<DDPlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        RandomiseInternalSpriteColours();
        StartCoroutine(MoveAroundArea());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RandomiseInternalSpriteColours()
    {
        SpriteRenderer topLevelSprite = GetComponent<SpriteRenderer>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        if (topLevelSprite != null)
        {
            topLevelSprite.color = new Color(RandomColourF(), RandomColourF(), RandomColourF());
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = new Color(RandomColourF(), RandomColourF(), RandomColourF());
        }
    }

    private float RandomColourF()
    {
        return Random.Range(0f, 1f);
    }

    void OnMouseDown()
    {
        peopleManager.MoveMainCameraTowardsPerson(person);
    }

    public IEnumerator MoveAroundArea()
    {
        while (true)
        {
            Vector3 destination = peopleManager.GetRandomPositionInSpawnArea();

            gameObject.transform.LookAt(destination);

            while (gameObject.transform.position != destination)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, 2.5f * Time.deltaTime);

                yield return null;
            }
        }
    }
}
