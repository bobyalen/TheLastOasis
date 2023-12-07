using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BlindnessScript : MonoBehaviour
{

    public int blindnessLevel = 0;
    [SerializeField]
    public GameObject N;
    public GameObject E;
    public GameObject S;
    public GameObject W;

    private float yOffset;
    private float xOffset;
    // Start is called before the first frame update
    void Start()
    {
        blindnessLevel = (int)PlayerStats.Instance.cachedCalculatedValues[Stat.Blindness];
        yOffset = Mathf.Abs(N.transform.localPosition.y);
        xOffset = Mathf.Abs(E.transform.localPosition.x);
        SetBlindness();
        EventManager.StartListening(Event.StatChanged, OnStatChanged);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.StatChanged, OnStatChanged);
    }
    private void OnStatChanged(IEventPacket packet)
    {
        StatChangedPacket scp = packet as StatChangedPacket;
        if(scp != null)
        {
            if(scp.stat == Stat.Blindness)
            {
                blindnessLevel = (int)PlayerStats.Instance.cachedCalculatedValues[Stat.Blindness];
                SetBlindness();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBlindness()
    {
        if(blindnessLevel == 0)
        {
            N.SetActive(false);
            E.SetActive(false);
            S.SetActive(false);
            W.SetActive(false);
        }
        else
        {
            N.SetActive(true);
            E.SetActive(true);
            S.SetActive(true);
            W.SetActive(true);
            N.transform.localPosition = new Vector3(0, yOffset + (3 - blindnessLevel) * 0.5f);
            E.transform.localPosition = new Vector3(xOffset + (3 - blindnessLevel) * 0.5f, 0);
            S.transform.localPosition = new Vector3(0, -(yOffset + (3 - blindnessLevel) * 0.5f));
            W.transform.localPosition = new Vector3(-(xOffset + (3 - blindnessLevel) * 0.5f), 0);
        }
    }
}
