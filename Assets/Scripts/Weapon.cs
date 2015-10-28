using UnityEngine;
using System.Collections;

public class Weapon: MonoBehaviour
{
    public enum FireType { Raycast, Projectile };

    FireType fireType = FireType.Raycast; //Debug.

    string weaponName = "";
    int damage = 100;
    int ammoInCurrentMagazine = 10;
    int totalAmmo = 20;

    float reloadTime = 0;
    float fireSpeed = 0.2f;
    int magazineCapacity = 10;

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
        NotificationCenter.DefaultCenter().PostNotification(gameObject, "PlayerAmmoChanged", ammoInCurrentMagazine.ToString() + "/" + totalAmmo.ToString());
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
        if (busy)
            return;

        //Instatiate Bullet
        //Raycast Code
        //Play Fire Sound
        if (ammoInCurrentMagazine <= 0)
            return;

        Fire_Raycast();
        Fire_ChangeAmmo();
        StartCoroutine(SetWeaponToBusy(fireSpeed));
    }

    public void Fire_ChangeAmmo()
    {
        ammoInCurrentMagazine--;
        NotificationCenter.DefaultCenter().PostNotification(gameObject, "PlayerAmmoChanged", ammoInCurrentMagazine.ToString() + "/" + totalAmmo.ToString());
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
            try
            {
                hit.transform.gameObject.GetComponent<Enemy>().ReceiveDamage(damage); //TODO: Create a health object for universal health/damage.
            }
            catch { }
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

        NotificationCenter.DefaultCenter().PostNotification(gameObject, "PlayerAmmoChanged", ammoInCurrentMagazine.ToString() + "/" + totalAmmo.ToString());
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
