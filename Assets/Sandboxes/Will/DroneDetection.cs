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

    private bool movement = true;
    /*
    void Start()
    {
        
    }
    */

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

                /*Vector3 away = (transform.position - player.position).normalized;

                away.y = 0.6f;


                transform.position += away * 5 * Time.deltaTime;
                */

                //this.Explode();
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

    IEnumerator ExplosionWait() 
    {
        // start animation

        // wait
        yield return new WaitForSeconds(1f);

        // explode
        this.Explode();
    }

    // trigger explosion when coming into contact with player
    //private void OnTriggerEnter(Collider other)
    //{
        //if (other.CompareTag("Player"))
        //{
            //this.Explode();
        //}
    //}

    public override void PlayerLost()
    {
        
    }
}
