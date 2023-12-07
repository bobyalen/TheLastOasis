using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public PlayerAttack instance;

    private void Start()
    {
        instance = GetComponentInParent<PlayerAttack>();
    }
    private void Update()
    {

    }

    private void CursorRotate()
    {
        if (PlayerController.Instance.currentState != CURRENT_STATE.DASHING)
        {
            // Rotate sword to face mouse pointer
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 lookDir = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Compares the target layer to the object that was hit.
        if ((instance.targetLayer & 1 << other.gameObject.layer) != 0)
        {
            EventManager.TriggerEvent(Event.PlayerHitEnemy, new PlayerHitPacket()
            {
                //Sword audio here
                damage = instance.swingDamage,
                enemy = other.gameObject
            });
            
        }
    }

    // Debugging purposes to slay enemies faster. Can be removed once we are happy with the combat system.
    private void OnTriggerStay2D(Collider2D other)
    {
        // Compares the target layer to the object that was hit.
        if (instance.canAttack && (instance.targetLayer & 1 << other.gameObject.layer) != 0)
        {
            EventManager.TriggerEvent(Event.PlayerHitEnemy, new PlayerHitPacket()
            {
                damage = instance.swingDamage,
                enemy = other.gameObject
            });
        }
    }
}