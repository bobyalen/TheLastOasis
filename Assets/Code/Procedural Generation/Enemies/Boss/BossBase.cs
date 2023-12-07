using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBase : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxHealth = 2000;
    public float currentHealth;
    public string bossName;
    BossPattern _bossPattern;
    void Start()
    {
        currentHealth = maxHealth;
        _bossPattern = GetComponent<BossPattern>();
        UIManager.Instance.SetBossName(bossName);
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
        StartCoroutine(LoadEndScreen());
    }

    IEnumerator LoadEndScreen()
    {
        yield return new WaitForSeconds(1.5f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("EndScreen");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
    private void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        float damage = php.damage;
        if(_bossPattern.isTransitioning())
        {
            damage = (int)(10.0f * php.damage / 100.0f);
            if (damage == 0)
                damage = 1;
        }
        if (php.enemy == this.gameObject)
        {
            currentHealth -= damage;
        }
        EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
        {
            damage = (int)damage,
            position = this.gameObject.transform.position,
            textColor = Color.yellow

        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.X))
        {
            if(currentHealth > maxHealth / 2.0f)
            {
                currentHealth = maxHealth / 2.0f;
            }
        }
        if(currentHealth < 0)
        {
            Destroy(this.gameObject);
        }
        if(currentHealth <= maxHealth / 2.0f && !_bossPattern.IsAngry())
        {
            _bossPattern.BecomeAngry();
        }
        
        UIManager.Instance.SetBossHealth(currentHealth / maxHealth);
    }
}
