using UnityEngine;
using System.Collections;

public class DamageSource : MonoBehaviour
{
    public enum DamageMode { MANUAL, COLLISION_ENTER, COLLISION_STAY }

    [SerializeField]
    private float _damage = 0;
    [SerializeField]
    private DamageMode _damageMode = DamageMode.MANUAL;

    // Whether to pass damage to triggers (if damage mode is not manual).
    [SerializeField]
    private bool _hitTriggers = false;

    void OnCollisionEnter(Collision collision)
    {
        if (_damageMode == DamageMode.COLLISION_ENTER)
        {
            SendDamage(collision.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (_damageMode == DamageMode.COLLISION_STAY)
        {
            SendDamage(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (_hitTriggers && _damageMode == DamageMode.COLLISION_ENTER)
        {
            SendDamage(trigger.gameObject);
        }
    }

    void OnTriggerStay(Collider trigger)
    {
        if (_hitTriggers && _damageMode == DamageMode.COLLISION_STAY)
        {
            SendDamage(trigger.gameObject);
        }
    }

    public void SendDamage(Component receiver)
    {
        receiver.SendMessage("ReceiveDamage", this, SendMessageOptions.DontRequireReceiver);
    }

    public void SendDamage(GameObject receiver)
    {
        receiver.SendMessage("ReceiveDamage", this, SendMessageOptions.DontRequireReceiver);
    }

    public float damage
    {
        get { return _damage; }
    }
}