using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{
    public Transform spawnPoint;
    public DroneManager droneManager;

    public void Die()
    {
        // for now just reload the scene
        StartCoroutine(Dying());

    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.3f);
        //Destroy(gameObject);
        // move player to checkpoint
        transform.position = spawnPoint.transform.position;
        // respawn all drones
        droneManager.RespawnAll();

        //SceneManager.LoadScene("MainScene");
    }
}
