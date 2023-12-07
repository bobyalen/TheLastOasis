using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.PlayerSettings;
public enum CURRENT_STATE
{
    RUNNING,
    IDLE,
    ATTACK,
    DASHING,
    SCENE_CHANGE,
    MOVE_ATTACK
}
public class PlayerController : MonoBehaviour
{
    // Getting state for future development once we have animations.

    public CURRENT_STATE currentState;

    [SerializeField] public Rigidbody2D rb;
    [SerializeField] float speed;

    [HideInInspector] public Vector2 movement;

    private SpriteRenderer sr;
    public Animator animator;
    private Color originalColor;

    public bool invulnerability;
    private float invulnerabilityHolder;
    private float blinkTimer;
    private bool canDash = true;

    private bool isInDialogue = false;

    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private float dashDistance, dashCooldown, dashLength;
    [SerializeField] public Vector2 lastPlayerDirection;

    public Transform doorWayPoint;
    [SerializeField]
    private GameObject Shield;

    public KeyCode lastKeyPressed;

    public static PlayerController Instance;
    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        invulnerabilityHolder = invulnerabilityDuration;
        dashElapsedCooldown = dashCooldown;
        originalColor = GetComponent<SpriteRenderer>().color;
        speed = PlayerStats.Instance.cachedCalculatedValues[Stat.Speed];
        EventManager.StartListening(Event.BossTeleport, BossTeleport);
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
        EventManager.StartListening(Event.PlayerDeath, FreezePlayer);
        EventManager.StartListening(Event.StatChanged, UpdateStats);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.BossTeleport, BossTeleport);
        EventManager.StopListening(Event.DialogueStart, FreezePlayer);
        EventManager.StopListening(Event.DialogueFinish, UnfreezePlayer);
        EventManager.StopListening(Event.PlayerDeath, FreezePlayer);
        EventManager.StopListening(Event.StatChanged, UpdateStats);
    }

    void FreezePlayer(IEventPacket packet)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        isInDialogue = true;
    }

    void UnfreezePlayer(IEventPacket packet)
    {
        isInDialogue = false;
    }
    private void Update()
    {
        Shield.SetActive(PlayerStats.Instance.cachedCalculatedValues[Stat.Divine_Shield] > 0);
        if (isInDialogue)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        //var key = GetLastKeyPressed();
        //if (key != KeyCode.None)
        //    lastKeyPressed = key;

        if (canDash && currentState == CURRENT_STATE.RUNNING)
        {
            if (Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero)
                Dash();
        }

        if (movement != Vector2.zero)
        {
            lastPlayerDirection = movement;

            if(currentState == CURRENT_STATE.IDLE)
                currentState = CURRENT_STATE.RUNNING;

            //if (currentState == CURRENT_STATE.RUNNING)
            //    animator.SetBool("isMoving", true);
        } 
        else
        {
            if(currentState == CURRENT_STATE.RUNNING)
                currentState = CURRENT_STATE.IDLE;
        }
        //else
        //{
        //    animator.SetBool("isMoving", false);
        //}
        ChangeColorOnDash();
        Invulnerability();
        testStats();
    }

    KeyCode GetLastKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return KeyCode.W;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return KeyCode.S;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return KeyCode.D;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return KeyCode.A;
        return KeyCode.None;

    }

    private Dictionary<KeyCode, Vector2> keyMapping = new Dictionary<KeyCode, Vector2>()
    {
        {KeyCode.W, Vector2.up },
        {KeyCode.S, Vector2.down},
        {KeyCode.D, Vector2.right},
        {KeyCode.A, Vector2.left }
    };

    void FixedUpdate()
    {
        if (isInDialogue)
            return;
        //Vector2 lastDir = Vector2.zero;
        //if (lastKeyPressed != KeyCode.None)
        //    lastDir = keyMapping[lastKeyPressed];

        switch (currentState)
        {
            case CURRENT_STATE.IDLE:
                rb.velocity = Vector2.zero;
                animator.SetBool("isMoving", false);
                break;
            case CURRENT_STATE.RUNNING:
                rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", lastPlayerDirection.x);
                animator.SetFloat("moveY", lastPlayerDirection.y);
                //rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
                //if (Mathf.Abs(lastPlayerDirection.y) == Mathf.Abs(lastPlayerDirection.x) && lastPlayerDirection.x != 0)
                //{
                //    animator.SetFloat("moveX", lastDir.x);
                //    animator.SetFloat("moveY", lastDir.y);
                //}
                //else
                //{
                //    animator.SetFloat("moveX", lastPlayerDirection.x);
                //    animator.SetFloat("moveY", lastPlayerDirection.y);
                //}
                break;
            case CURRENT_STATE.ATTACK:
                rb.velocity = Vector2.zero;
                break;
            //case CURRENT_STATE.COMBO:
            //    rb.velocity = Vector2.zero;
            //    animator.SetFloat("moveX", lastPlayerDirection.x);
            //    animator.SetFloat("moveY", lastPlayerDirection.y);
            //    break;
            case CURRENT_STATE.SCENE_CHANGE:
                Vector2 movePos = Vector2.MoveTowards(transform.position, doorWayPoint.position, 2 * Time.deltaTime);
                rb.MovePosition(movePos);
                break;

        }

    }

    public IEnumerator CinematicMove(Vector2 position, Vector2 finalFacing)
    {
        Vector2 direction = position - new Vector2(transform.position.x, transform.position.y);
        animator.SetFloat("moveX", Mathf.Sign(direction.x));
        animator.SetFloat("moveY", Mathf.Sign(direction.y));
        var step = 1.0f * Time.deltaTime;
        while((Vector2)transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, step);
            yield return null;
        }
        animator.SetFloat("moveX", finalFacing.x);
        animator.SetFloat("moveY", finalFacing.y);
    }

    public IEnumerator Spin(float time)
    {
        float initialDelay = 0.5f;
        float currDelay = initialDelay;
        Vector2[] directions = { Vector2.down, Vector2.left, Vector2.up, Vector2.right };
        int currDirection = 0;
        while(time > 0)
        {
            time -= Time.deltaTime;
            currDelay -= Time.deltaTime;
            if(currDelay < 0)
            {
                currDirection++;
                if(currDirection > 3)
                {
                    currDirection = 0;
                    initialDelay /= 1.5f;
                }
                animator.SetFloat("moveX", directions[currDirection].x);
                animator.SetFloat("moveY", directions[currDirection].y);
                currDelay = initialDelay;
            }
            yield return null;

        }
        animator.SetFloat("moveX", 1);
        animator.SetFloat("moveY", 0);
    }

    public void Vanish(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;
    }
    public void testStats()
    {
        speed = PlayerStats.Instance.cachedCalculatedValues[Stat.Speed];
    }

    void BossTeleport(IEventPacket packet)
    {
        BossTeleportPacket btp = packet as BossTeleportPacket;
        if(btp != null)
        {
            transform.position = btp.transform.position - Vector3.up * 4.0f;
        }
    }

    private void Dash()
    {
        currentState = CURRENT_STATE.DASHING;

        rb.velocity = Vector2.zero;

        Vector2 dashDirection = movement.normalized * dashDistance;

        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        rb.AddForce(dashDirection, ForceMode2D.Impulse);

        canDash = false;
        Invoke("ResetDash", dashCooldown);
        StartCoroutine(ElapseDashCooldown());
        Invoke("DisableIsDashing", dashLength);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void DisableIsDashing()
    {
        rb.velocity = Vector2.zero;
        currentState = CURRENT_STATE.RUNNING;
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        //Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
    float dashElapsedCooldown;

    private IEnumerator ElapseDashCooldown()
    {
        dashElapsedCooldown = 0;
        for(float i = 0; i < dashCooldown; i+= Time.deltaTime)
        {
            dashElapsedCooldown += Time.deltaTime;
            yield return null;
        }
        dashElapsedCooldown = 0.8f;
    }
    public float GetDashPercentage()
    {
        return dashElapsedCooldown / dashCooldown;
    }

    private void Invulnerability()
    {
        if (invulnerability)
        {
            blinkTimer -= Time.deltaTime;
            ChangePlayerAlpha();
            IgnoreCollider(true);

            if (blinkTimer <= 0.5f)
            {
                blinkTimer = 1;
                invulnerabilityDuration--;

                if (invulnerabilityDuration < 0)
                {
                    invulnerability = false;
                    invulnerabilityDuration = invulnerabilityHolder;
                    blinkTimer = 1;
                    ChangePlayerAlpha();
                    //IgnoreCollider(false);
                }
            }
        }
    }

    private void ChangePlayerAlpha()
    {
        Color playerColor = sr.color;
        playerColor.a = blinkTimer;
        gameObject.GetComponent<SpriteRenderer>().color = playerColor;
    }

    private void ChangeColorOnDash()
    {
        Color playerColor = Color.gray;
        if (currentState == CURRENT_STATE.DASHING)
        {
            playerColor.a = 0.8f;
            sr.color = playerColor;
        }
        else
            sr.color = originalColor;
    }

    private void IgnoreCollider(bool ignore)
    {
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, ignore);
    }

    public Transform FindWayPoint()
    {
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("DoorWaypoint");
        Transform tempTrans = null;
        float tempDist = 3;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            float distance = Vector2.Distance(PlayerController.Instance.transform.position, wayPoints[i].transform.position);
            if (distance < tempDist)
            {
                tempTrans = wayPoints[i].transform;
                tempDist = distance;
            }
        }
        return tempTrans;
    }

    private void UpdateStats(IEventPacket packet)
    {
        StatChangedPacket scp = packet as StatChangedPacket;
        if(scp != null)
        {
            if (scp.stat == Stat.Speed)
                speed = PlayerStats.Instance.cachedCalculatedValues[Stat.Speed];
        }
    }
}