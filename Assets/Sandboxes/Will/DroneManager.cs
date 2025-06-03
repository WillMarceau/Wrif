using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public DroneSpawner[] droneSpawners;

    public void RespawnAll()
    {
        foreach (DroneSpawner spawner in droneSpawners)
        {
            spawner.Spawn();
        }
    }
}
