using UnityEngine;

public class Patrol : MonoBehaviour
{
    // waypoints
    public Transform[] patrolPoints;
    // navMesh Agent
    //UnityEngine.AI.NavMeshAgent navMeshAgent;
    // current waypoint index
    // movement stuff
    public float speed;
    int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set first waypoint as destination
        Vector3 direction = patrolPoints[index].position - transform.position;
        direction.y = 0;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = rot;
    }

    // Update is called once per frame
    void Update()
    {
        // move towards target direction
        // can change target when player is detected, otherwise just move between waypoints
        Transform target = patrolPoints[index];
        Vector3 direction;

        // if waypoint reached, set next waypoint and turn towards it
        if (Vector3.Distance(transform.position, target.position) < 0.1f) {
            index = (index + 1) % patrolPoints.Length;
            target = patrolPoints[index];
            direction = target.position - transform.position;

            // rotate
            direction = (target.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = rot;

        }
        direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
