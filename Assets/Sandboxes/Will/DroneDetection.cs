using UnityEngine;
using System.Collections;
public class DroneDetection : Detection
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public GameObject explosionFab;
    public float stopDistance;
    public float chaseSpeed;
    public Patrol patrolScript;
    public float countdown;
    public float explosionRange;
    private bool isChasing = false;
    public Light detectionLight;

    public Light spotLight;

    private bool movement = true;
    private bool hasExploded = false;
    public AudioClip detectedSfx;
    public AudioClip explosionSfx;
    private bool hasPlayedDetectedSfx = false;
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing && movement) {
            // calc distance
            float distance = Vector3.Distance(transform.position, player.position);

            // get closer if needed
            if (distance > stopDistance) 
            {
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y += 0.5f;
                transform.position += direction * chaseSpeed * Time.deltaTime;

                direction = (player.position - transform.position).normalized;
                direction.y = -0.3f;
                Quaternion rot = Quaternion.LookRotation(direction);
                transform.rotation = rot;
            }

            // just explode 
            else
            {
                movement = false;
                // stop timer
                StopCoroutine(ExplosionCountdown());

                // Start explosion
                StartCoroutine(ExplosionWait());

            }
        }
    }

    public override void PlayerDetected(Transform playerInput)
    {
        // on player detection enable chasing, disable patrol and pass the player input

        isChasing = true;
        patrolScript.enabled = false;
        player = playerInput.transform;
        detectionLight.color = Color.red;
        spotLight.color = Color.red;

        // play detected sound effect
        if (!hasExploded && !hasPlayedDetectedSfx && detectedSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(detectedSfx);
            hasPlayedDetectedSfx = true;
        }

        StartCoroutine(ExplosionCountdown());
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        GameObject explosion = Instantiate(explosionFab, transform.position, Quaternion.identity);

        // layer mask
        LayerMask mask = ~LayerMask.GetMask("Enemy");
        // cancel movement

        // start animation

        // calc distance
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= explosionRange)
        {
            // check line of sight
            Vector3 direction = player.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            // destroy if hit
            if (Physics.Raycast(ray, out hit, explosionRange, mask))
            {
                if (hit.collider.transform == player) 
                {
                    // reload scene for now
                    Death deathScript = player.GetComponent<Death>();
                    deathScript.Die();
                }
            }
        }
        // play explosion        
        // Make the drone invisible
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
        if (detectionLight != null)
        {
            detectionLight.enabled = false;
            // Also disable halo if present
            Behaviour halo = (Behaviour)detectionLight.GetComponent("Halo");
            if (halo != null)
                halo.enabled = false;
        }
        if (spotLight != null)
        {
            spotLight.enabled = false;
        }

         // Play explosion sound and destroy after sound finishes
        if (explosionSfx != null && audioSource != null)
        {
            Debug.Log("Playing explosion sound, length: " + explosionSfx.length);
            audioSource.volume = 3f;
            audioSource.spatialBlend = 0f;
            audioSource.PlayOneShot(explosionSfx);
            Destroy(gameObject, explosionSfx.length);
        }
        else
        {
            Debug.LogWarning("Explosion sound or AudioSource missing!");
            Destroy(gameObject);
        }


    }

    IEnumerator ExplosionCountdown() 
    {
        float elapsed = 0f;
        float blinkInterval = 1f;
        float blinkTimer = 0f;
        // countdown to explosion

        while (elapsed < countdown)
        {
            elapsed += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkInterval) 
            {
                detectionLight.enabled = !detectionLight.enabled;
                blinkTimer = 0f;
            }
            yield return null;
        }

        detectionLight.enabled = true;

        // wait a sec
        this.Explode();
    }

    IEnumerator ExplosionWait() 
    {
        // start animation

        // wait
        yield return new WaitForSeconds(1f);

        // explode
        this.Explode();
    }

   
    public override void PlayerLost()
    {
        hasPlayedDetectedSfx = false;
    }
}
