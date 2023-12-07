using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject collectablePrefab;
    [SerializeField]
    GameObject consumablePrefab;
    [SerializeField]
    public CollectableData coinData;
    [SerializeField]
    public ConsumeableData healthPotion;

    public static ItemSpawnManager Instance;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Spawn(CollectableData data, Transform t, int amount)
    {
        var go = Instantiate(collectablePrefab, t.position, Quaternion.identity);

        //Set Sprite Info
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = data.Sprite;
        //Set Collectable info
        Collectable coll = go.GetComponent<Collectable>();
        coll.stackSize = amount;
        coll.SetCollectableData(data);
    }

    public GameObject SpawnItem(Item item, Transform t, int amount)
    {
        Vector3 position = Random.insideUnitCircle;
        var go = Instantiate(item.prefabToSpawn, t.position + position * 0.35f, Quaternion.identity);
        go.transform.parent = t.root.transform;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = item.Sprite;
        if(item is CollectableData)
        {
            SetCollectableData(item as CollectableData, go, amount);
        }
        else if(item is ConsumeableData)
        {
            SetConsumeableData(item as ConsumeableData, go);
        }
        return go;
    }

    private void SetCollectableData(CollectableData data, GameObject gameObject, int amount)
    {
        Collectable coll = gameObject.GetComponent<Collectable>();
        coll.stackSize = amount;
        coll.SetCollectableData(data);
    }

    private void SetConsumeableData(ConsumeableData data, GameObject gameObject)
    {
        Consumeable cons = gameObject.GetComponent<Consumeable>();
        cons.SetConsumeableData(data);
    }
}
