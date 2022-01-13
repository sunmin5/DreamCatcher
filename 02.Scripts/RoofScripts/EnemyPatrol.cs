using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    float patrolSpeed = 0.6f;
    int randomPoint;

    public GameObject[] points;

    NavMeshAgent pathFinder;

    Animator anim;

    bool patrolling;
    public bool Patrol
    {
        get { return patrolling; }
        set
        {
            patrolling = value;
            if (patrolling)
            {
                pathFinder.speed = patrolSpeed;
                PatrolPoint();
            }
        }
    }

    void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        patrolling = true;
    }

    void PatrolPoint()
    {
        pathFinder.SetDestination(points[randomPoint].transform.position);
        pathFinder.isStopped = false;
        anim.SetBool("IsMove", false);
    }

    void Update()
    {
        if(pathFinder.remainingDistance <= 0.5f) randomPoint = Random.Range(0, points.Length);
    }
}