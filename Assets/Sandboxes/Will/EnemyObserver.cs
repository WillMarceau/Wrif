using UnityEngine;

public class EnemyObserver : MonoBehaviour
{
    public Transform player;

    bool inRange;
    private Detection detectionBehavior;

    public Detection detectionBehaviorScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detectionBehavior = detectionBehaviorScript as Detection;
    }

    void OnTriggerEnter (Collider other) {
        // set player in range if in collider
        if (other.transform == player) {
            inRange = true;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.transform == player) {
            inRange = false;
            detectionBehavior?.PlayerLost();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange) 
        {
            LayerMask mask = ~LayerMask.GetMask("Enemy");
            // check line of sight 
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            // if in sight
            if (Physics.Raycast(ray, out raycastHit, 100f, mask)) 
            {
                if (raycastHit.collider.transform == player) 
                {
                    // use detection behavior
                    detectionBehavior?.PlayerDetected(player);
                }
            }
        }
        
    }
}
