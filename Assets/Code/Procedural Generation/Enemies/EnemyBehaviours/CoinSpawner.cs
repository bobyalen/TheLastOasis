using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    float delay = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
