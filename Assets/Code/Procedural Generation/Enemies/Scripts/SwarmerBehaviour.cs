using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwarmerBehaviour : BaseMovementBehaviour
{
    public float speed = 1.2f;
    public float stunDuration = 0.4f;
    public float chillOutTimer = 0.0f;
    private float maxChillOutTimer = 0.0f;
    public float chillOutDuration = 0.0f;
    private float maxChillOutDuration = 0.0f;
    public List<GridCell> path = new List<GridCell>();
    GridCell nextCell = null;
    public EnemyBase eb;
    public bool isChillOut = true;

    protected void Start()
    {
        base.Start();
        eb = GetComponent<EnemyBase>();
        chillOutTimer = Random.Range(4.0f, 5.0f);
        chillOutDuration = Random.Range(0.5f, 1.0f);
    }
    protected override void OnHitAction()
    {
        StartCoroutine(Stun(stunDuration));
    }

    protected override Vector2 GetMovement()
    {
        if(isChillOut)
        {
            chillOutTimer -= Time.deltaTime;
            if (chillOutTimer <= 0.0f)
            {
                chillOutDuration -= Time.deltaTime;
                if(chillOutDuration <= 0.0f)
                {
                    chillOutTimer = Random.Range(4.0f, 5.0f);
                    chillOutDuration = Random.Range(0.5f, 1.0f);
                }
                return Vector2.zero;
            }
        }
        return MovementFunctions.FollowPlayer(speed, transform.position,eb.rs, path, ref nextCell);
    }

    private IEnumerator Stun(float duration)
    {
        canMove = false;
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
    }
}

