using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sr;
    Collider2D collider;
    float chargeTime;
    float stayAliveTime;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();  
    }

    public void SetParams(float chargeTime, float stayAliveTime)
    {
        this.chargeTime = chargeTime;
        this.stayAliveTime = stayAliveTime;
        StartCoroutine(DoSpawn());
    }

    public IEnumerator DoSpawn()
    {
        Color c = sr.color;
        float startAlpha = 0.3f;
        float endAlpha = 0.6f;
        for(float i = 0; i < chargeTime; i += Time.deltaTime)
        {
            c.a = Mathf.Lerp(startAlpha, endAlpha, i / chargeTime);
            sr.color = c;
            yield return null;
        }
        c.a = 1.0f;
        collider.enabled = true;
        sr.color = c;
        for(float i = 0; i < stayAliveTime; i += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
