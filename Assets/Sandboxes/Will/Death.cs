using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{

    Animator ani;
    public CapsuleCollider capsule;
    public CapsuleCollider frictionCapsule;
    public BoxCollider box;
    public Transform spawnPoint;
    public DroneManager droneManager;
    public GameObject deathScreen;
    PlayerMovement movementScript;

    void Start() 
    {
        ani = GetComponent<Animator>();
        deathScreen.SetActive(false);
        movementScript = GetComponent<PlayerMovement>();

    }

    public void Die()
    {
        //ani.SetTrigger("IsDeath");
        // for now just reload the scene
        ani.SetBool("IsDeath", true);
        StartCoroutine(Dying());

    }

    private IEnumerator Dying()
    {
        if (movementScript != null)
        {
            movementScript.canMove = false;
        }

        //ani.SetTrigger("IsDeath");

        yield return null;

        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName("Death"))
        {
            yield return null;
            state = ani.GetCurrentAnimatorStateInfo(0);
        }
        capsule.enabled = false;
        frictionCapsule.enabled = false;
        box.enabled = true;
        //ani.SetTrigger("IsDeath");
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
        // move player to checkpoint
        deathScreen.SetActive(true);
        //ani.SetTrigger("IsRespawn");
        capsule.enabled = true;
        frictionCapsule.enabled = true;
        box.enabled = false;
        transform.position = spawnPoint.transform.position;
        ani.SetBool("IsDeath", false);
        // respawn all drones
        droneManager.RespawnAll();

        yield return new WaitForSeconds(1.5f);
        movementScript.canMove = true;
        deathScreen.SetActive(false);

        //SceneManager.LoadScene("MainScene");
    }
}
