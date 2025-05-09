using UnityEngine;
using System.Collections;
public class DroneDetection : Detection
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public float stopDistance;
    public float chaseSpeed;
    public Patrol patrolScript;
    public float countdown;
    public float explosionRange;
    private bool isChasing = false;
    public Light detectionLight;
    /*
    void Start()
    {
        
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (isChasing) {
            // calc distance
            float distance = Vector3.Distance(transform.position, player.position);

            // get closer if needed
            if (distance > stopDistance) 
            {
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y += 0.7f;
                transform.position += direction * chaseSpeed * Time.deltaTime;

                direction = (player.position - transform.position).normalized;
                direction.y = -0.3f;
                Quaternion rot = Quaternion.LookRotation(direction);
                transform.rotation = rot;
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

        StartCoroutine(ExplosionCountdown());
    }

    void Explode()
    {
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
            if (Physics.Raycast(ray, out hit, explosionRange))
            {
                if (hit.collider.transform == player) 
                {
                    Destroy(player.gameObject);
                }
            }
        }
        Destroy(gameObject);

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
        //yield return new WaitForSeconds(countdown);
        detectionLight.enabled = true;
        this.Explode();
    }
}
