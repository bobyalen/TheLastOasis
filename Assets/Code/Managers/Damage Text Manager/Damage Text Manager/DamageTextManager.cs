using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;
    // Start is called before the first frame update
    private List<GameObject> pooledObjects;
    [SerializeField]
    private int amountToPool;

    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(textPrefab);
            tmp.SetActive(false);
            tmp.transform.parent = this.transform;
            pooledObjects.Add(tmp);
        }
    }

    private GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }

    void Start()
    {
        EventManager.StartListening(Event.DamageDealt, SpawnText);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.DamageDealt, SpawnText);
    }

    void SpawnText(IEventPacket packet)
    {
        DamageDealtPacket ddp = packet as DamageDealtPacket;
        if(ddp != null)
        {
            StartCoroutine(SpawnText(ddp.position, ddp.textColor, ddp.damage));
        }
    }

    IEnumerator SpawnText(Vector2 startPos, Color c, int dmgNumber)
    {
        GameObject go = GetPooledObject();
        go.SetActive(true);
        TextMeshPro tmp = go.GetComponent<TextMeshPro>();
        tmp.color = c;
        tmp.text = dmgNumber.ToString();
        go.transform.position = startPos;
        for(float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            go.transform.position = Vector2.Lerp(startPos, startPos + Vector2.up * 0.5f, t / 0.5f);
            if(t >= 0.25f)
            {
                Color col = tmp.color;
                col.a = Mathf.Lerp(0.0f, 1.0f, (0.5f - t) / 0.25f);
                tmp.color = col;
            }
            yield return null;
        }

        go.SetActive(false);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.yellow,
                damage = 5,
                position = PlayerController.Instance.transform.position + (Vector3)Random.insideUnitCircle
            });
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.red,
                damage = 5,
                position = PlayerController.Instance.transform.position + (Vector3)Random.insideUnitCircle
            });
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.green,
                damage = 5,
                position = PlayerController.Instance.transform.position + (Vector3)Random.insideUnitCircle
            });
        }
    }
}
