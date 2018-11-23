using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUser : MonoBehaviour {

    //controller for pathfinding
    NavMeshAgent agent;
    public Vector3 target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //set the target
        if (target != null)
        {
            agent.SetDestination(target);
        }
    }

    /// <summary>
    /// Set the target position for the pathfinding
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}