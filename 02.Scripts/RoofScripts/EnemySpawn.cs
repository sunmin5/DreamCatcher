using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    RoofPlayer rp;

    [SerializeField] GameObject prefab;

    void Start()
    {
        rp = FindObjectOfType<RoofPlayer>();
    }

    void Update()
    {
        if (rp.isDead) Instantiate(prefab, transform);

        for (int i = 0; i <= rp.noteCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}