using UnityEngine;

public class Patrol : MonoBehaviour
{
    // waypoints
    public Transform[] patrolPoints;
    // navMesh Agent
    NavMeshAgent navMeshAgent;
    // current waypoint index
    int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set first waypoint as destination
        navMeshAgent.SetDestination(patrolPoints[0].position)
    }

    // Update is called once per frame
    void Update()
    {
        // if the patrol point is reached, move to next waypoint
        
        
    }
}
