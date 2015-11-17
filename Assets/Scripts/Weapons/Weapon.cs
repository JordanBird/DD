using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Pool))]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int _magazineCapacity = 30; // 30 rounds in a clip
    private int _roundsRemaining; // Rounds remaining in magazine.
    [SerializeField]
    private float _rearmDelay = 0.1f; // 600 rounds shot per minute
    private bool _rearming = false;
    private Coroutine _rearmCoroutine = null;
    [SerializeField]
    private float _reloadDelay = 2.5f; // 2.5 seconds to reload
    private bool _reloading = false;
    private Coroutine _reloadCoroutine = null;

    #region Events
    [SerializeField]
    private UnityEvent _onShootSucceed;
    [SerializeField]
    private UnityEvent _onShootFail;
    [SerializeField]
    private UnityEvent _onReloadStart;
    [SerializeField]
    private UnityEvent _onReloadEnd;
    #endregion

    #region Connected Components
    [SerializeField]
    private Transform[] _muzzles; // Specification for position/direction of shots.
    private Pool _projectilePool; // All projectiles this weapon can use.
    #endregion

    void Awake()
    {
        _roundsRemaining = _magazineCapacity;
        _projectilePool = GetComponent<Pool>();

        // If there are no muzzles attached, use the gun as the point to shoot from.
        if (_muzzles.Length == 0)
        {
            Debug.Log(name + ": No muzzle(s) specified. Using the gun as the muzzle.");
            _muzzles = new Transform[1];
            _muzzles[0] = transform;
        }
    }

    public virtual void Shoot()
    {
        if (!rearming) {
            if (reloading)
            {
                _onShootFail.Invoke();
            }
            else
            {
                if (_muzzles.Length > 1)
                {
                    IList<GameObject> projectiles = _projectilePool.GetItems(_muzzles.Length);
                    for (int i = 0; i < projectiles.Count; ++i)
                    {
                        projectiles[i].transform.position = _muzzles[i].position;
                        projectiles[i].transform.rotation = _muzzles[i].rotation;
                        projectiles[i].SetActive(true);
                    }
                    Debug.Log(name + ": Shooting succeeded, attempting to fire " + _muzzles.Length + " rounds, firing " + projectiles.Count + " rounds.");
                }
                else
                {
                    GameObject projectile = _projectilePool.GetItem();
                    if (projectile != null)
                    {
                        projectile.transform.position = _muzzles[0].position;
                        projectile.transform.rotation = _muzzles[0].rotation;
                        projectile.SetActive(true);
                    }
                    Debug.Log(name + ": Shooting succeeded, attempting to fire a round, fired " + (projectile != null ? "a round" : "no rounds."));
                }
                _rearmCoroutine = StartCoroutine(_Rearm());
                _onShootSucceed.Invoke();
            }
        }
    }

    public void Reload()
    {
        if (!reloading)
        {
            if (rearming)
            {
                Debug.Log(name + ": Reload called but currently rearming. Stopping rearm coroutine and starting reload coroutine.");
                StopCoroutine(_rearmCoroutine);
                _rearmCoroutine = null;
            }
            _reloadCoroutine = StartCoroutine(_Reload());
        }
    }

    private IEnumerator _Rearm()
    {
        Debug.Log(name + ": Start Rearming");
        yield return new WaitForSeconds(_rearmDelay);
        _rearmCoroutine = null;
        Debug.Log(name + ": End Rearming");
    }

    private IEnumerator _Reload()
    {
        Debug.Log(name + ": Start Reloading");
        _onReloadStart.Invoke();
        yield return new WaitForSeconds(_reloadDelay);
        _roundsRemaining = _magazineCapacity;
        _reloadCoroutine = null;
        _onReloadEnd.Invoke();
        Debug.Log(name + ": End Reloading");
    }

    public bool rearming
    {
        get { return _rearmCoroutine != null; }
    }

    public bool reloading
    {
        get { return _reloadCoroutine != null; }
    }
}