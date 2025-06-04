using UnityEngine;

public class ColliderRespawnChange : MonoBehaviour
{
    public Transform newSpawn;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Death deathScript = other.GetComponent<Death>();
            if (deathScript != null)
            {
                deathScript.spawnPoint = newSpawn;
            }
        }
    }
}