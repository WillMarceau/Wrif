using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    // player
    public Transform player;
    // dronePhab
    public GameObject dronePrefab;
    //Patrol Path
    public Transform[] patrolWaypoints;

    //private GameObject currentDrone;

    public GameObject existingDrone;

    public void Spawn()
    {

        // if current drone is not destroyed destroy it
        if (existingDrone != null)
        {
            Destroy(existingDrone);
        }

        if (patrolWaypoints.Length == 0)
        {
            return;
        }

        // Spawn new drone at the first waypoint
        Transform firstWaypoint = patrolWaypoints[0];
        existingDrone = Instantiate(dronePrefab, firstWaypoint.position, firstWaypoint.rotation);

        // Assign Waypoints and player
        Patrol patrolScript = existingDrone.GetComponent<Patrol>();
        if (patrolScript != null)
        {
            patrolScript.patrolPoints = patrolWaypoints; 
        }
        DroneDetection detectionScript = existingDrone.GetComponent<DroneDetection>();
        if (detectionScript != null)
        {
            detectionScript.player = player;
        }

        EnemyObserver observerScript = existingDrone.GetComponentInChildren<EnemyObserver>();
        if (observerScript != null)
        {
            observerScript.player = player;
        }
    }

}
