using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCrossScript : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sr_VerticalCross;
    [SerializeField]
    SpriteRenderer sr_HorizontalCross;
    [SerializeField]
    SpriteRenderer sr_BossCircle;
    [SerializeField]
    SpriteRenderer sr_Diagonal_1;
    [SerializeField]
    SpriteRenderer sr_Diagonal_2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCrossAlphas(float alpha, bool isLastState = false)
    {
        Color c = sr_VerticalCross.color;
        c.a = alpha;
        sr_VerticalCross.color = c;
        sr_HorizontalCross.color = c;
        if(isLastState)
        {
            sr_Diagonal_1.color = sr_Diagonal_2.color = c;
        }
        c.a = Mathf.Clamp(0.2f, 0.8f, alpha);
        sr_BossCircle.color = c;
    }
    
    public void SetColliders(bool active, bool isLastState = false)
    {
        sr_VerticalCross.gameObject.GetComponent<Collider2D>().enabled = active;
        sr_HorizontalCross.gameObject.GetComponent<Collider2D>().enabled = active;
        sr_BossCircle.gameObject.GetComponent<Collider2D>().enabled = active;
        if(isLastState)
        {
            sr_Diagonal_1.gameObject.GetComponent<Collider2D>().enabled = active;
            sr_Diagonal_2.gameObject.GetComponent<Collider2D>().enabled = active;
        }
    }

    public void ResetCross()
    {
        transform.rotation = Quaternion.identity;
        SetColliders(false);
    }

    public void SetCrossState(bool active)
    {
        sr_Diagonal_1.gameObject.SetActive(active);
        sr_Diagonal_2.gameObject.SetActive(active);
    }
}
