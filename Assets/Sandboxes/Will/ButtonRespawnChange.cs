using UnityEngine;

public class ChangeRespawn : MonoBehaviour
{
    public GameObject player;

    public Transform newSpawn;

    public void ChangeSpawn()
    {
        Death deathScript = player.GetComponent<Death>();
        if (deathScript != null)
        {
            deathScript.spawnPoint = newSpawn;
        }
    }
}

