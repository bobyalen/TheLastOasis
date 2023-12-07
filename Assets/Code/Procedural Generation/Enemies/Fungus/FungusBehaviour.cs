using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum FungusState
{
    MOVING,
    WINDUP,
    FARTING,
    STUN
}
public class FungusBehaviour : BaseMoveAndAttackBehaviour
{
    [SerializeField] private AudioClip clip;

    [Header("Attack Variables")]
    [SerializeField]
    FungusAttack attackHitbox;
    public float windUpTime;
    public float strikeTime;

    [Header("Timer Variables")]
    public float timer;
    public float maxDistanceCap;
    public float maxDistanceDeplete;
    public float minDistanceCap;
    public float minDistanceDeplete;
    private float maxTimer;


    [Header("Push back properties")]
    public float time;
    public float magnitude;
    public float stunTime;
    public float knockBackTime;

    [Header("Pathfinding Variables")]
    public List<GridCell> path = new List<GridCell>();
    GridCell nextCell = null;
    public EnemyBase eb;
    [SerializeField]
    FungusState currState = FungusState.MOVING;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        maxTimer = timer;
        eb = GetComponent<EnemyBase>();
        attackHitbox.GetComponent<Collider2D>().enabled = false;
    }


    protected override void DoAct()
    {
        rb.velocity = GetMovement();
        if (timer > 0)
            timer -= GetDepletionTime();
        else if (!isAttacking)
            DoAttack();
    }
    float GetDepletionTime()
    {
        float sqDistance = (this.transform.position - PlayerController.Instance.transform.position).sqrMagnitude;
        float sqMax = maxDistanceCap * maxDistanceCap;
        float sqMin = minDistanceCap * minDistanceCap;
        sqDistance = Mathf.Clamp(sqDistance, sqMin, sqMax);

        return Time.deltaTime * Mathf.Lerp(maxDistanceDeplete, minDistanceDeplete, (sqDistance - sqMax)/(sqMin - sqMax));
    }

    protected override void DoAttack()
    {
        currState = FungusState.WINDUP;
        GetComponent<SpriteRenderer>().color = new Color(1,0,1,1);
        StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
        isAttacking = true;
    }

    protected override void DoBeginAttack()
    {
        currState = FungusState.FARTING;
        GetComponent<SpriteRenderer>().color = Color.white;
        attackHitbox.GetComponent<Collider2D>().enabled = true;
        attackHitbox.IsAttacking = true;
        attackHitbox.GetComponent<SpriteRenderer>().enabled = true;
    }


    protected override void DoStopAttack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        attackHitbox.GetComponent<SpriteRenderer>().enabled = false;
        attackHitbox.GetComponent<Collider2D>().enabled = false;
        attackHitbox.StopAttack();
        isAttacking = false;
        attackHitbox.IsAttacking = false;
        if (currState != FungusState.STUN)
        {
            currState = FungusState.MOVING;
            timer = maxTimer;
        }
    }

    protected override Vector2 GetMovement()
    {
        switch(currState)
        {
            case FungusState.MOVING:
                return MovementFunctions.FollowPlayer(speed, transform.position, eb.rs, path, ref nextCell);
            case FungusState.STUN:
                return rb.velocity;
            case FungusState.WINDUP:
                return Vector2.zero;
            case FungusState.FARTING:
                return Vector2.zero;
        }
        return Vector2.zero;
    }

    protected override void OnHitAction()
    {
        AudioManager.instance.PlayFungusSound(clip);
        if(currState != FungusState.FARTING && currState != FungusState.WINDUP)
            StartCoroutine(PushBack());
    }

    IEnumerator PushBack()
    {
        currState = FungusState.STUN;
        canMove = false;
        DoStopAttack();
        Vector2 spiderPos = transform.position;
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 dir = (spiderPos - playerPos).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * magnitude, ForceMode2D.Impulse);
        for (float i = 0; i <= knockBackTime; i += Time.deltaTime)
        {
            yield return null;
        }
        rb.velocity = Vector2.zero;
        for (float i = 0; i <= stunTime; i += Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
        if(timer <= 0)
        {
            currState = FungusState.FARTING;
        }
        else
        {
            currState = FungusState.MOVING;
        }
    }

    protected override void DoSetAnimatorVariables()
    {
    }
    protected override void AddAdditionalEventListeners()
    {
    }
    protected override void RemoveAdditionalEventListeners()
    {
    }

}
