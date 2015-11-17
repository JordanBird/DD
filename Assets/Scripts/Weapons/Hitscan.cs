using UnityEngine;
using UnityEngine.Events;
using System.Collections;

// A raycast that causes damage. No rigidbody interaction.
[RequireComponent(typeof(DamageSource))]
public class Hitscan : MonoBehaviour
{
    private Vector3 _scanPosition;
    private Vector3 _scanDirection;

    [SerializeField]
    private float _speed = 100;

    [SerializeField]
    private float _distance = 100;
    private float _distanceTravelled = 0;

    private bool _travelling = false;

    [SerializeField]
    private LayerMask[] _layers;
    private LayerMask _combinedLayerMask;

    #region Events
    [SerializeField]
    private UnityEvent _onHit;
    #endregion

    #region Connected Components
    private DamageSource _damageSource;
    #endregion

    private void Awake()
    {
        _damageSource = GetComponent<DamageSource>();
        foreach (LayerMask mask in _layers)
        {
            _combinedLayerMask |= mask;
        }
    }

    private void OnEnable()
    {
        _travelling = true;
        _distanceTravelled = 0;
        _scanPosition = transform.position;
        _scanDirection = transform.forward;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, _scanPosition);

        if (_travelling)
        {
            float distanceToMove;
            if (_speed > 0)
            {
                distanceToMove = _speed * Time.deltaTime;
                float distanceRemaining = _distance - _distanceTravelled;
                if (distanceToMove >= distanceRemaining)
                {
                    distanceToMove = distanceRemaining;
                    _travelling = false;
                }
            }
            else
            {
                // Speeds of 0 and below will just make the scan instant.
                distanceToMove = _distance;
                _travelling = false;
            }

            RaycastHit hit;
            if (Physics.Raycast(_scanPosition, _scanDirection, out hit, distanceToMove, _combinedLayerMask.value))
            {
                Debug.Log(name + ": Hit " + hit.transform.name + " at " + hit.point.ToString());

                _scanPosition = hit.point;
                _travelling = false;

                _damageSource.SendDamage(hit.transform.gameObject);
                _onHit.Invoke();
            }
            else
            {
                // Move the scan position for the next frame since there was not a hit.
                _scanPosition += _scanDirection * distanceToMove;
                _distanceTravelled += distanceToMove;
            }
        }
    }
}
