using UnityEngine;

public class AcidScript : MonoBehaviour
{

    public DroneManager droneManager;
    public void OnTriggerEnter(Collider other)
    {
        // if player comes into contact with the acid
        if (other.CompareTag("Player")) 
        {

            // respawn player
            Death deathScript = other.GetComponent<Death>();
            deathScript.Die();

            // respawn all drones
            droneManager.RespawnAll();
            
        }
    }    
}
