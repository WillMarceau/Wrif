using UnityEngine;
using System.Collections;

public class WarriorDetection : Detection
{
    public Transform player;

    public float turnSpeed;
    private float shootTimer;
    public Light detectionLight;
    private bool isAttacking;
    public float shootAngle;
    public float shootCooldown;
    public Light spotLight;
    public GameObject muzzleFlash;
    public Transform shootPoint;
    //new test code
    public AudioClip playerDetectedClip; 
    public AudioClip fireClip;          
    private AudioSource audioSource;
    private bool hasPlayedDetectedSound = false;
    EnemyObserver observerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        shootTimer = 0.5f;
        observerScript = GetComponentInChildren<EnemyObserver>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component NOT found on " + gameObject.name);
        }
        else
        {
            Debug.Log("AudioSource found on " + gameObject.name);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isAttacking && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime * 100f);

            float angle = Quaternion.Angle(transform.rotation, lookRotation);
            if (angle <= shootAngle)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0f)
                {
                    this.Fire();
                    shootTimer = shootCooldown;
                }
            }
        }
        
    }

    public override void PlayerDetected(Transform playerInput)
    {
        // activate attacking sequence
        isAttacking = true;
        player = playerInput.transform;
        detectionLight.color = Color.red;
        spotLight.color = Color.red;

        // Play player detected sound
        if (!hasPlayedDetectedSound && audioSource != null && playerDetectedClip != null)
        {
            Debug.Log("Player detected sound");
            audioSource.PlayOneShot(playerDetectedClip);
            hasPlayedDetectedSound = true;
        }
    }

    public override void PlayerLost()
    {
        // lost player
        isAttacking = false;
        player = null;
        detectionLight.color = Color.green;
        spotLight.color = Color.green;
        shootTimer = 0.5f;
        hasPlayedDetectedSound = false;

    }

    private void Fire()
    {
        // play animation
        GameObject flash = Instantiate(muzzleFlash, shootPoint.position, shootPoint.rotation);
        flash.transform.SetParent(shootPoint);

        // play fire sound BEFORE destroying the flash
        if (audioSource != null && fireClip != null)
        {
            Debug.Log("Playing fire sound");
            audioSource.PlayOneShot(fireClip);
        }

        Destroy(flash, 0.4f);

        // check line of sight
        LayerMask mask = ~LayerMask.GetMask("Enemy");
        Vector3 direction = player.position - transform.position + Vector3.up;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            // if hit player
            if (hit.transform == player)
            {
                // trigger respawn and death animations
                // apply damage

                Death deathScript = player.GetComponent<Death>();
                deathScript.Die();

                StartCoroutine(ResetAfterWait(1f));

            }
        }
    }

    private IEnumerator ResetAfterWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        player = null;
        detectionLight.color = Color.green;
        spotLight.color = Color.green;
        shootTimer = 0.5f;

        if (observerScript != null)
        {
            observerScript.inRange = false;
        }

    }


}
