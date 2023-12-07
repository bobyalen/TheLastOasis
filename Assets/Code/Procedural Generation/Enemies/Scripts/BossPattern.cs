using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public enum BossState
{
    DORMANT,
    IDLE,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    ATTACK4,
}

public class BossPattern : MonoBehaviour
{
    private GameObject player;
    public float speed;
    public float onCollisionDamage;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform roomTransform;
    

    BossState currentState = BossState.DORMANT;
    BossState lastAttackState = BossState.IDLE;
    bool isStateRunning = false;
    float currentAttackDamage = 0.0f;
    Coroutine currentCoroutine = null;

    int debugFrameCounter = 0;

    [Header("Idle State Parameters")]
    [SerializeField]
    private float idleTime;

    [Header("Charge State Parameters")]
    [SerializeField]
    float revTime;
    [SerializeField]
    float timeBetweenRevs;
    [SerializeField]
    float revSpeed;
    [SerializeField]
    float chargeSpeed;
    [SerializeField]
    float chargeStaggerTime;
    [SerializeField]
    bool hasCollidedWithWall;
    [SerializeField]
    GameObject chargeHitbox;
    [SerializeField]
    float chargeDamage;

    [Header("Cross State Parameters")]
    [SerializeField]
    float runToCentreSpeed;
    [SerializeField]
    BossCrossScript Cross;
    [SerializeField]
    float alphaBegin, alphaEnd;
    [SerializeField]
    float alphaFadeTime;
    [SerializeField]
    float crossTurningSpeed;
    [SerializeField]
    float crossStaggerTime;
    [SerializeField]
    float crossDamage;

    [Header("Spawn State Parameters")]
    [SerializeField]
    float spawnChargeTime;
    [SerializeField]
    float spawnTimeBetweenSpawns;
    [SerializeField]
    float spawnTotalTime;
    [SerializeField]
    float spawnStaggerTime;
    [SerializeField]
    BossSpawner spawnObject;
    [SerializeField]
    float spawnObjectChargeTime;
    [SerializeField]
    float spawnObjectStayAliveTime;
    [SerializeField]
    float spawnDamage;

    [Header("Transition Parameters")]
    [SerializeField]
    float transitionTime;

    private bool isInFinalState = false;
    private bool isTransitioningToFinalState = false;

    private void Start()
    {
        onCollisionDamage = 5;
        speed = 2f;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        roomTransform = GetComponentInParent<RoomScript>().transform;

        EventManager.StartListening(Event.AwakenBoss, AwakenBoss);
    }

    private void Update()
    {

        ProcessStateMachineChange();
        ProcessState();
        
    }

    void ProcessState()
    {
        if (isStateRunning)
            return;
        switch(currentState)
        {
            case BossState.IDLE:
                {
                    currentCoroutine = StartCoroutine(ProcessIdleState());
                    break;
                }
            case BossState.ATTACK1:
                {
                    currentAttackDamage = chargeDamage;
                    currentCoroutine = StartCoroutine(ProcessChargeState());
                    break;
                }
            case BossState.ATTACK2:
                {
                    currentAttackDamage = crossDamage;
                    currentCoroutine = StartCoroutine(ProcessCrossState());
                    break;
                }
            case BossState.ATTACK3:
                {
                    currentAttackDamage = spawnDamage;
                    currentCoroutine = StartCoroutine(ProcessSpawnState());
                    break;
                }
            case BossState.ATTACK4:
                {
                    StartCoroutine(ProcessDropState());
                    break;
                }
        }
    }

    IEnumerator ProcessIdleState()
    {
        isStateRunning = true;
        Vector2 direction1 = Random.insideUnitCircle;
        Vector2 direction2 = Random.insideUnitCircle;

        
        for(float i = 0; i < idleTime; i += Time.deltaTime)
        {
            if (i < idleTime / 4)
            {
                rb.velocity = speed * direction1;
            }
            else if (i < 2 * idleTime / 4)
            {
                rb.velocity = Vector2.zero;
            }
            else if(i < 3 * idleTime / 4)
            {
                rb.velocity = -speed * direction2;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            yield return null;
        }
        isStateRunning = false;
    }

    IEnumerator ProcessChargeState()
    {
        Debug.Log("CHARGE!");
        isStateRunning = true;
        //the bull revs up twice, then after the third, it charges
        Vector2 direction = Vector2.zero;
        for (int i = 0; i < 3; i++)
        {
            direction = player.transform.position - transform.position;
            for (float j = 0; j < revTime; j += Time.deltaTime)
            {
                rb.velocity = -direction.normalized * revSpeed;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            if(i != 2)
            for(float j = 0; j < timeBetweenRevs; j += Time.deltaTime)
            {
                yield return null;
            }

        }
        chargeHitbox.SetActive(true);
        int no_repeats = isInFinalState ? 3 : 1;
        for(int i = 0; i < no_repeats; i++)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            while (!hasCollidedWithWall)
            {
                rb.velocity = direction.normalized * chargeSpeed;
                yield return null;
            }
            hasCollidedWithWall = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            rb.velocity = Vector2.zero;
            if (i != no_repeats - 1)
                yield return new WaitForSeconds(revTime);
            direction = player.transform.position - transform.position;
        }
        chargeHitbox.SetActive(false);
        for (float i = 0; i < chargeStaggerTime; i += Time.deltaTime)
        {
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.None;
        isStateRunning = false;
    }

    IEnumerator ProcessCrossState()
    {
        isStateRunning = true;

        Debug.Log("CROSS!");
        //first move towards the centre
        int iter = 0;
        while(transform.position != roomTransform.position && iter < 1000)
        {
            iter++;
            transform.position = Vector3.MoveTowards(transform.position, roomTransform.position, runToCentreSpeed * Time.deltaTime);
            yield return null;
        }

        Cross.SetCrossAlphas(alphaBegin, IsAngry());
        Cross.SetColliders(false, IsAngry());
        Cross.gameObject.SetActive(true);
        Cross.SetCrossState(isInFinalState);
        for(float i = 0; i < alphaFadeTime; i += Time.deltaTime)
        {
            Cross.SetCrossAlphas(Mathf.Lerp(alphaBegin, alphaEnd, i / alphaFadeTime), IsAngry());
            yield return null;
        }
        Cross.SetCrossAlphas(1.0f, IsAngry());
        Cross.SetColliders(true, IsAngry());
        float totalRotation = 0;
        while(totalRotation <= 180)
        {
            Cross.transform.Rotate(Vector3.forward * crossTurningSpeed * (IsAngry() ? 1.33f : 1.0f) * Time.deltaTime);
            totalRotation += crossTurningSpeed * Time.deltaTime;
            yield return null;
        }
        Cross.gameObject.SetActive(false);
        Cross.SetColliders(false, IsAngry());
        for(float i = 0; i < crossStaggerTime; i += Time.deltaTime)
        {
            yield return null;
        }
        isStateRunning = false;
    }

    IEnumerator ProcessSpawnState()
    {
        isStateRunning = true;
        Debug.Log("SPAWN!");
        //stops moving and starts charging
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        Color initial = sr.color;
        Color target = new Color(0.33f, 0, 0.2f, 1.0f);
        for (float i = 0; i < spawnChargeTime; i += Time.deltaTime)
        {
            sr.color = Color.Lerp(initial, target, i / spawnChargeTime);
            yield return null;
        }
        //starts dropping the thingies
        float totalTimeElapsed = 0.0f;
        float timeSinceLastDrop = 0.0f;
        var go = Instantiate(spawnObject, player.transform.position, Quaternion.identity);
        go.SetParams(spawnObjectChargeTime, spawnObjectStayAliveTime);
        if (IsAngry())
            go.transform.localScale *= 1.33f;
        go.transform.parent = this.transform;
        while (totalTimeElapsed < spawnTotalTime)
        {
            totalTimeElapsed += Time.deltaTime;
            timeSinceLastDrop += Time.deltaTime;
            if (timeSinceLastDrop > spawnTimeBetweenSpawns)
            {
                go = Instantiate(spawnObject, player.transform.position, Quaternion.identity);
                go.SetParams(spawnObjectChargeTime, spawnObjectStayAliveTime);
                if (IsAngry())
                    go.transform.localScale *= 1.33f;
                go.transform.parent = this.transform;
                timeSinceLastDrop = 0.0f;
            }
            yield return null;
        }
        for (float i = 0; i < spawnStaggerTime; i += Time.deltaTime)
        {
            sr.color = Color.Lerp(target, initial, i/spawnStaggerTime);
            yield return null;
        }
        isStateRunning = false;
    }

    IEnumerator ProcessDropState()
    {
        isStateRunning = true;
        for (float i = 0; i < 1.5f; i += Time.deltaTime)
        {
            yield return null;
        }
        isStateRunning = false;
    }

    void ProcessStateMachineChange()
    {
        if (isStateRunning)
            return;
        switch(currentState)
        {
            case BossState.IDLE:
                {
                    var nextState = (BossState)Random.Range(2, 5);
                    while(nextState == lastAttackState)
                    {
                        nextState = (BossState)Random.Range(2, 5);
                    }
                    currentState = nextState;
                    break;
                }
            case BossState.ATTACK1:
            case BossState.ATTACK2:
            case BossState.ATTACK3:
            case BossState.ATTACK4:
                {
                    lastAttackState = currentState;
                    currentState = BossState.IDLE;
                    break;
                }
        }
    }


    public IEnumerator Awaken()
    {
        for(float i = 0; i < 1.5f; i += Time.deltaTime)
        {
            yield return null;
        }
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            if(!hasCollidedWithWall)
                hasCollidedWithWall = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            EventManager.TriggerEvent(Event.EnemyHitPlayer, new EnemyHitPacket()
            {
                healthDeplete = currentAttackDamage,
                playerInvulnerability = PlayerController.Instance.invulnerability,
                playerDivineShield = PlayerStats.Instance.cachedCalculatedValues[Stat.Divine_Shield] > 0
            });
        }
    }

    public void AwakenBoss(IEventPacket packet)
    {
        currentState = BossState.IDLE;
    }

    public void BecomeAngry()
    {
        isInFinalState = true;
        StopCoroutine(currentCoroutine);
        ResetToDefault();
        StartCoroutine(TransitionToAnger());
    }
    private void ResetToDefault()
    {

        sr.color = new Color(0.36f, 0.19f, 0.04f);
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.None;
        Cross.ResetCross();
        Cross.gameObject.SetActive(false);
    }
    public bool IsAngry()
    {
        return isInFinalState;
    }

    public bool isTransitioning()
    {
        return isTransitioningToFinalState;
    }

    private IEnumerator TransitionToAnger()
    {
        isTransitioningToFinalState = true;
        Color initial = sr.color;
        Color final = Color.red;
        for (float i = 0; i < transitionTime; i += Time.deltaTime)
        {
            sr.color = Color.Lerp(initial, final, i / transitionTime);
            yield return null;
        }
        isTransitioningToFinalState = false;
        isStateRunning = false;
    }
    
}
