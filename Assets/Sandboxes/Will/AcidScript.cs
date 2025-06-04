using UnityEngine;

public class AcidScript : MonoBehaviour
{
    private bool triggered = false;

    public DroneManager droneManager;
    public void OnTriggerEnter(Collider other)
    {
        // if player comes into contact with the acid
        if (!triggered && other.CompareTag("Player")) 
        {
            triggered = true;
            Debug.Log($"droneManager is {(droneManager == null ? "NULL" : "SET")}");

            // respawn player
            Death deathScript = other.GetComponent<Death>();
            deathScript.Die();

            // respawn all drones
            droneManager.RespawnAll();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggered = false;
        }
    }
}
