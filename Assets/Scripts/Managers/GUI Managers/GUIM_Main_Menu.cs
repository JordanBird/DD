using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIM_Main_Menu : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void LoadGame()
    {
        Application.LoadLevel("Gameplay Scene");
    }

    public void LoadTestScene()
    {
        Application.LoadLevel("Test");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
