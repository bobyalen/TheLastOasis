using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();

    public float spawnDelay;
    private float currentDelay;

    public bool isRandom;
    private float xOffset = 0.1f;
    public int numberOfItems = 10;
    void Start()
    {
    }


    void Update()
    {
        UpdateSpawner();
    }

    void SpawnRandomItem()
    {
        int index = isRandom ? Random.Range(0, itemsToSpawn.Count) : 0; //If random is true, then set random range between 0 and number of items, otherwise set to 0
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 setRight = new Vector3(transform.position.x, 0, 0);
        Vector3 setUp = new Vector3(0, transform.position.y, 0);
        if (itemsToSpawn.Count > 0)
        {
            for (var i = 0; i < numberOfItems; i++)
            {
                Instantiate(itemsToSpawn[index], (position + (setRight * xOffset) + setUp * i), Quaternion.identity);

            }
        }
    }
    void UpdateSpawner()
    {
        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
        }
        else
        {
            //To do: Create a new list when time is reset, delete old list
            SpawnRandomItem();
            currentDelay = spawnDelay;
        }
    }
}
