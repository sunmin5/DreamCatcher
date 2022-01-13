using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Animator anim;

    float attackDelay = 1.4f;
    float attackTime = 0;

    int enemyDamage = 1;

    public bool isAttack = false;

    RoofPlayer player;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<RoofPlayer>();
    }

    void Update()
    {
        
        if (isAttack)
        {
            if (Time.time >= attackTime)
            {
                anim.SetBool("Attack", true);
                if (!player.isDead) StartCoroutine(player.OnDamage(enemyDamage));
                attackTime = Time.time + attackDelay;
            }
        }
        else anim.SetBool("Attack", false);
    }
}