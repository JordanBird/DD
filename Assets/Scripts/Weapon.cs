using UnityEngine;
using System.Collections;

public class Weapon: MonoBehaviour
{
    public enum FireType { Raycast, Projectile };

    FireType fireType = FireType.Raycast; //Debug.

    string weaponName = "";
    float damage = 0;
    int ammoInCurrentMagazine = 0;
    int totalAmmo = 0;

    float reloadTime = 0;
    float fireSpeed = 0;
    int magazineCapacity = 0;

    Sprite normal;
    Sprite firing;
    Sprite reloadingSprite;

    string imageSetLocation = "";

    AudioClip fireSound;
    AudioClip reloadSound;
    string pickupImage = "";

    bool busy = false;

    //Cached Objects
    AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        Start_GetAudioSource();
    }
	
    private void Start_GetAudioSource()
    {
        try
        {
            audioSource = GetComponent<AudioSource>();
        }
        catch { }
    }

	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void Fire()
    {
        //Instatiate Bullet
        //Raycast Code
        //Play Fire Sound
        if (ammoInCurrentMagazine <= 0)
            return;

        Fire_ChangeAmmo();
    }

    public void Fire_ChangeAmmo()
    {
        ammoInCurrentMagazine--;
    }

    private void Fire_PlayFireSound()
    {
        try
        {
            audioSource.clip = fireSound;
            audioSource.Play();
        }
        catch { Debug.LogWarning("An error occurred while trying to play the fire sound."); }
    }

    private void Fire_Raycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            hit.transform.gameObject.SendMessage("ReceiveDamage", damage);
        }
    }

    public void Reload()
    {
        //Play Reload Sound
        int difference = magazineCapacity - ammoInCurrentMagazine;

        if (difference == 0)
            return;

        Reload_ChangeAmmo();
        Reload_PlayReloadSound();
    }

    private void Reload_ChangeAmmo()
    {
        int difference = magazineCapacity - ammoInCurrentMagazine;

        if (totalAmmo > magazineCapacity - difference)
        {
            ammoInCurrentMagazine += difference;
            totalAmmo -= difference;
        }
        else
        {
            ammoInCurrentMagazine += totalAmmo;
            totalAmmo = 0;
        }
    }

    private void Reload_PlayReloadSound()
    {
        try
        {
            audioSource.clip = reloadSound;
            audioSource.Play();
        }
        catch { Debug.LogWarning("An error occurred while trying to play the reload sound."); }
    }

    private IEnumerator SetWeaponToBusy(float time)
    {
        busy = true;

        yield return new WaitForSeconds(time);

        busy = false;
    }
}
