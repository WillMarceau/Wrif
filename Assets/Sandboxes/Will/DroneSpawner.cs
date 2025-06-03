using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    // player
    public Transform player;
    // dronePhab
    public GameObject dronePrefab;
    //Patrol Path
    public Transform[] patrolWaypoints;

    private GameObject currentDrone;

    public void Spawn()
    {

        // if current drone is not destroyed destroy it
        if (currentDrone != null)
        {
            Destroy(currentDrone);
        }

        if (patrolWaypoints.Length == 0)
        {
            return;
        }

        // Spawn new drone at the first waypoint
        Transform firstWaypoint = patrolWaypoints[0];
        currentDrone = Instantiate(dronePrefab, firstWaypoint.position, firstWaypoint.rotation);

        // Assign Waypoints and player
        Patrol patrolScript = currentDrone.GetComponent<Patrol>();
        if (patrolScript != null)
        {
            patrolScript.PatrolPoints = patrolWaypoints; 
        }
        DroneDetection detectionScript = currentDrone.GetComponent<DroneDetection>();
        if (detectionScript != null)
        {
            detectionScript.Player = player;
        }

        EnemyObserver observerScript = currentDrone.GetComponentInChildren<EnemyObserver>();
        if (observerScript != null)
        {
            observerScript.Player = player;
        }
    }

}
