using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static PlayerController;

public class PlayerAttack : MonoBehaviour
{

    public float SwingDelay
    {
        get
        {
            float dex = PlayerStats.Instance.cachedCalculatedValues[Stat.Dexterity];
            dex = Mathf.Clamp(dex, 33.0f, 1000.0f);
            return swingDelay * 100.0f/dex;
        }
        set
        {
            swingDelay = value;
        }
    }

    public float swingDelay;
    public float swingDamage;

    public float pushPower;

    public LayerMask targetLayer;

    public bool canAttack = true;
    public float animationLength;
    public SpriteRenderer sr;

    private bool isInDialogue = false;
    public static PlayerAttack Instance;

    public void testStats()
    {
        swingDamage = PlayerStats.Instance.cachedCalculatedValues[Stat.Damage];
        //swingDelay = swingDelay - PlayerStats.Instance.cachedCalculatedValues[Stat.Dexterity];
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        
        }
    }
    private void Start()
    {
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
        targetLayer = LayerMask.GetMask("Enemy", "Destructible");
        animationLength = AnimationLength("AttackW");
        swingDamage = PlayerStats.Instance.cachedCalculatedValues[Stat.Damage];
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.DialogueStart, FreezePlayer);
        EventManager.StopListening(Event.DialogueFinish, UnfreezePlayer);
    }
    void FreezePlayer(IEventPacket packet)
    {
        isInDialogue = true;
    }

    void UnfreezePlayer(IEventPacket packet)
    {
        isInDialogue = false;
    }
    private void Update()
    {
        if (isInDialogue)
            return;

        if(PlayerController.Instance.movement == Vector2.zero)
        {
            if (Input.GetMouseButton(0) && canAttack && (PlayerController.Instance.currentState == CURRENT_STATE.RUNNING || PlayerController.Instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(Attack());
            }
        }
        else
        { 
            if (Input.GetMouseButton(0) && canAttack && (PlayerController.Instance.currentState == CURRENT_STATE.RUNNING || PlayerController.Instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(MoveAttack(PlayerController.Instance.lastPlayerDirection, pushPower));
            }
        }
        testStats();
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        PlayerController.Instance.animator.SetTrigger("isAttackingTrigger");
        PlayerController.Instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength + 0.1f);
        PlayerController.Instance.currentState = CURRENT_STATE.RUNNING;
        Invoke("ResetAttack", SwingDelay);
    }

    private IEnumerator MoveAttack(Vector2 force, float power)
    {
        canAttack = false;
        PlayerController.Instance.animator.SetTrigger("isAttackingTrigger");
        PlayerController.Instance.rb.AddForce(force * power);
        PlayerController.Instance.currentState = CURRENT_STATE.MOVE_ATTACK;
        Invoke("ResetAttack", SwingDelay + animationLength + 0.1f);
        yield return new WaitForSeconds(0.2f);
        PlayerController.Instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength - 0.1f);
        PlayerController.Instance.currentState = CURRENT_STATE.RUNNING;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private float AnimationLength(string clipName)
    {
        if (PlayerController.Instance.animator != null && PlayerController.Instance.animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in PlayerController.Instance.animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        Debug.LogError("Make sure the animation name is correct.");
        return 0;
    }
}
