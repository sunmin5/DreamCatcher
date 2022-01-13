using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject item;

    Transform playerTr;

    [SerializeField] float playerMaxDistance;

    float itemSpawnTime;
    float lastSpawnTime;
    float itemSpawnMin = 2;
    float itemSpawnMax = 5;

    void Start()
    {
        playerTr = FindObjectOfType<ParkPlayer>().transform;
        itemSpawnTime = Random.Range(itemSpawnMin, itemSpawnMax);
    }
    
    void Update()
    {
        if(Time.time >= itemSpawnTime + lastSpawnTime)
        {
            lastSpawnTime = Time.time;
            itemSpawnTime = Random.Range(itemSpawnMin, itemSpawnMax);

            Vector3 spawnPos = NavMeshHitPositon();

            GameObject potion = Instantiate(item, spawnPos, Quaternion.identity, gameObject.transform);

            Destroy(potion, 10);
        }

        if(TextObject.instance.failText.enabled == true)
        {
            gameObject.GetComponent<ItemSpawner>().enabled = false;
        }
    }

    Vector3 NavMeshHitPositon()
    {
        Vector3 itemSpawnPos = playerTr.position + Random.insideUnitSphere * playerMaxDistance;

        float x = Mathf.Clamp(itemSpawnPos.x, -12, 15);
        itemSpawnPos.x = x;

        itemSpawnPos.y = playerTr.position.y;

        float z = Mathf.Clamp(itemSpawnPos.z, -24, -0.5f);
        itemSpawnPos.z = z;

        NavMeshHit hit;
        NavMesh.SamplePosition(itemSpawnPos, out hit, playerMaxDistance, NavMesh.AllAreas);
        return hit.position;
    }
}