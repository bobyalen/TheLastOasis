using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpiderState
{
    MOVE,
    IDLE,
    ATTACK,
    STUN
}
public class SpiderBehaviour : BaseMoveAndAttackBehaviour
{
    public float DetectionRadius;
    [SerializeField] private AudioClip clip;

    [Header("Attack Properties")]
    [SerializeField]
    AttackBase attackHitbox;
    private BoxCollider2D hitboxCollider;
    public float windUpTime;
    public float strikeTime;
    [Tooltip("Time after attack when enemy doesn't move")]
    public float attackCooldown;

    [Header("Push back properties")]
    public float time;
    public float magnitude;
    public float stunTime;
    public float knockBackTime;

    

    //Base positions for the hitbox to use for reset/rescale
    private float initialXPos;
    private float initialYPos;
    private float initialXScale;
    private float initialYScale;
    //Enemy Direction Facing
    public int xDir, yDir;

    private Coroutine AttackCoroutine;
    //Pathfinding Variables
    public List<GridCell> path = new List<GridCell>();
    GridCell nextCell = null;
    GridCell destcell = null;
    public EnemyBase eb;
    public SpiderState currState = SpiderState.MOVE;
    
    new void Start()
    {
        base.Start();
        hitboxCollider = attackHitbox.GetComponent<BoxCollider2D>();
        initialXScale = hitboxCollider.size.x;
        initialYScale = hitboxCollider.size.y;
        initialXPos = hitboxCollider.offset.x;
        initialYPos = hitboxCollider.offset.y;
        eb = GetComponent<EnemyBase>();
        SetMultipliers();

    }

    void SetMultipliers()
    {

    }
    protected override void DoAct()
    {
        if(GetDistanceToPlayer() < DetectionRadius * DetectionRadius && currState == SpiderState.IDLE)
        {
            currState = SpiderState.MOVE;
        }
        if(GetDistanceToPlayer() > DetectionRadius * DetectionRadius && currState == SpiderState.MOVE)
        {
            if(nextCell != null)
                nextCell = null;
            currState = SpiderState.IDLE;
        }
        rb.velocity = GetNextMovement();
        if (currState != SpiderState.ATTACK)
            TurnAttackHitbox();
    }

    private float GetDistanceToPlayer()
    {
        return (transform.position - PlayerController.Instance.transform.position).sqrMagnitude;
    }
    void TurnAttackHitbox()
    {
        Vector2 dir = PlayerController.Instance.transform.position - transform.position;
        dir = dir.normalized;

        //it is closer on the x axis
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            yDir = 0;
            hitboxCollider.size = new Vector2(initialXScale, initialYScale);
            if (dir.x < 0)
            {
                hitboxCollider.offset = new Vector2(-initialXPos, initialYPos);
                xDir = -1;
            }
            else
            {
                hitboxCollider.offset = new Vector2(initialXPos, initialYPos);
                xDir = 1;
            }
        }
        //closer on the y axis
        else
        {
            xDir = 0;
            hitboxCollider.size = new Vector2(initialYScale, initialXScale);
            if (dir.y < 0)
            {
                hitboxCollider.offset = new Vector2(initialYPos, -initialXPos);
                yDir = -1;
            }
            else
            {
                hitboxCollider.offset = new Vector2(initialYPos, initialXPos);
                yDir = -1;
            }

        }
        SetHitboxSprite();
    }

    private void SetHitboxSprite()
    {
        attackHitbox.hitboxRenderer.transform.localPosition = hitboxCollider.offset;
        attackHitbox.hitboxRenderer.transform.localScale = hitboxCollider.size;
    }
    protected override void DoSetAnimatorVariables()
    {

    }
    protected override void OnHitAction()
    {
        AudioManager.instance.PlaySpiderSound(clip);
        StartCoroutine(PushBack());
    }
    private IEnumerator PushBack()
    {
        currState = SpiderState.STUN;
        canMove = false;
        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
        DoStopAttack();
        Vector2 spiderPos = transform.position;
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 dir = (spiderPos - playerPos).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * magnitude, ForceMode2D.Impulse);
        for(float i  = 0; i <= knockBackTime; i+= Time.deltaTime)
        {
            yield return null;
        }
        rb.velocity = Vector2.zero;
        for (float i = 0; i <= stunTime; i+= Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
        currState = SpiderState.MOVE;

    }

    protected override Vector2 GetMovement()
    {
        switch(currState)
        {
            case SpiderState.MOVE:
                return MovementFunctions.FollowPlayer(speed, transform.position, eb.rs, path, ref nextCell);
            case SpiderState.ATTACK:
                return Vector2.zero;
            case SpiderState.IDLE:
                return MovementFunctions.Skitter(speed/2.0f, transform.position, eb.rs, path, ref nextCell, ref destcell);
            case SpiderState.STUN:
                return rb.velocity;
        }
        return rb.velocity;
    }

    public void OnHitboxEntered(IEventPacket packet)
    {
        EnemyHitboxEnteredPacket ehep = packet as EnemyHitboxEnteredPacket;
        if(ReferenceEquals(ehep.Hitbox, attackHitbox.gameObject) && currState != SpiderState.ATTACK && currState != SpiderState.STUN)
        {
            currState = SpiderState.ATTACK;
            if(!attackHitbox.IsAttacking)
                Attack();
        }
    }
    protected override void DoAttack()
    {
        AttackCoroutine = StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
    }

    protected override void DoBeginAttack()
    {
        attackHitbox.IsAttacking = true;
        attackHitbox.hitboxRenderer.enabled = true;
    }

    protected override void DoStopAttack()
    {
        attackHitbox.IsAttacking = false;
        attackHitbox.hasAttacked = false;
        attackHitbox.hitboxRenderer.enabled = false;
        if (currState != SpiderState.STUN)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        for(float i = 0; i < attackCooldown; i += Time.deltaTime)
        {
            yield return null;
        }
        currState = SpiderState.MOVE;
    }

    protected override void AddAdditionalEventListeners()
    {
        EventManager.StartListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }

    protected override void RemoveAdditionalEventListeners()
    {

        EventManager.StopListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }
}
