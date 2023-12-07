using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeepingTreeBehaviour : BaseAttackBehaviour
{
    [SerializeField] private AudioClip clip;
    [Header("Attack variables")]
    [SerializeField]
    AttackBase attackHitbox;
    public float windUpTime;
    public float strikeTime;
    public float timer;
    private float maxTimer;

    public bool isAttacking = false;

    protected override void DoAct()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if(!isAttacking)
            DoAttack();
    }

    protected override void DoAttack()
    {
        isAttacking = true;
        StartCoroutine(AttackFunctions.Smash(this, windUpTime, strikeTime));
    }

    protected override void DoBeginAttack()
    {
        attackHitbox.IsAttacking = true;
        attackHitbox.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void DoStopAttack()
    {
        attackHitbox.GetComponent<SpriteRenderer>().enabled = false;
        timer = maxTimer;
        isAttacking = false;
        attackHitbox.IsAttacking = false;
        attackHitbox.hasAttacked = false;
    }

    protected override void DoSetAnimatorVariables()
    {

    }




    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        maxTimer = timer;
    }
    protected override void RemoveAdditionalEventListeners()
    {
    }
    protected override void AddAdditionalEventListeners()
    {
    }
    protected override void OnHitAction()
    {
        AudioManager.instance.PlayTreeSound(clip);
    }
}
