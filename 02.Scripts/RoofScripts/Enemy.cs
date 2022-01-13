using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum State { PATROL, CHASE, ATTACK, DIE }
    public State state = State.PATROL;

    bool isDead = false;

    EnemyAttack enemyAttack;
    EnemyPatrol enemyPatrol;
    RoofPlayer player;

    Vector3 targetPos;

    Animator anim;

    NavMeshAgent pathFinder;

    float enemySpeed = 1;
    float attackDistance = 1.2f;
    float detectedDistance = 5.5f;

    [SerializeField] ParticleSystem disruptionParticle;
    [SerializeField] ParticleSystem explosionParticle;

    void Start()
    {
        player = FindObjectOfType<RoofPlayer>();
        pathFinder = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyPatrol = GetComponent<EnemyPatrol>();

        StartCoroutine(CheckState());
        StartCoroutine(EnemyState());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            targetPos = player.transform.position;
            float distance = Vector3.Distance(targetPos, transform.position);

            if (player.setDreamCatcher) state = State.DIE;

            if (state == State.DIE) yield break;

            else if (distance <= detectedDistance)
            {
                if (!player.safe) state = State.CHASE;
                else state = State.PATROL;

                if (distance <= attackDistance) state = State.ATTACK;
                else pathFinder.isStopped = false;
            }
            else state = State.PATROL;

            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator EnemyState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);
            switch (state)
            {
                case State.PATROL:
                    enemyAttack.isAttack = false;
                    enemyPatrol.Patrol = true;
                    break;
                case State.CHASE:
                    enemyAttack.isAttack = false;
                    EnemyMove();
                    break;
                case State.ATTACK:
                    enemyAttack.isAttack = true;
                    pathFinder.isStopped = true;
                    enemyPatrol.Patrol = false;
                    break;
                case State.DIE:
                    pathFinder.isStopped = true;
                    enemyPatrol.Patrol = false;
                    EnemyStop();
                    Invoke("EnemyDie", 3);
                    break;
            }
        }
    }

    void EnemyMove()
    {
        anim.SetBool("IsMove", true);
        pathFinder.speed = enemySpeed;
        pathFinder.destination = targetPos;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "SlowArea")
        {
            if (state != State.PATROL) pathFinder.speed = enemySpeed / 2;
        }
    }

    void EnemyStop()
    {
        anim.SetTrigger("Idle");
        disruptionParticle=Instantiate(disruptionParticle);
        disruptionParticle.transform.position = transform.position + Vector3.up;
        pathFinder.speed = 0;
        disruptionParticle.Play();
        isDead = true;
    }

    void EnemyDie()
    {
        disruptionParticle.Stop();
        explosionParticle = Instantiate(explosionParticle);
        explosionParticle.transform.position = transform.position + Vector3.up;
        explosionParticle.Play();
        anim.SetTrigger("Dead");
    }
}