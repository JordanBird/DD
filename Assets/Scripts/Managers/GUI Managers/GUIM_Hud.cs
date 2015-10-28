using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GUIM_Hud : MonoBehaviour
{
    public Text ammo;
    public Text health;

	// Use this for initialization
	void Start ()
    {
        NotificationCenter.DefaultCenter().AddObserver(gameObject, "PlayerAmmoChanged");
        NotificationCenter.DefaultCenter().AddObserver(gameObject, "PlayerHealthChanged");
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void PlayerAmmoChanged(NotificationCenter.Notification noti)
    {
        ammo.text = noti.data;
    }

    public void PlayerHealthChanged(NotificationCenter.Notification noti)
    {
        health.text = noti.data;
    }
}
