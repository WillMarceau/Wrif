using UnityEngine;

public class RadioController : MonoBehaviour
{

    public Transform player;

    public Rigidbody playerRigidBody;

    public GameObject miniGame;

    public GameObject pressE;

    public float interactionRange;

    private float distance;

    private bool near = false;

    private bool gameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        //check if the player is close enough to use the terminal
        distance = Vector3.Distance(transform.position, player.position);

        if(distance <= interactionRange)
        {
            near = true;
        }
        else
        {
            near = false;
            pressE.SetActive(false);

        }

        if(near)
        {
            if(Input.GetKeyDown(KeyCode.E) && !gameActive)
            {
                Debug.Log("Launching Game...");
                gameActive = true;
                pressE.SetActive(false);
                miniGame.SetActive(true);
                playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;


            }
            else if(Input.GetKeyDown(KeyCode.E) && gameActive)
            {
                Debug.Log("Closing Game...");
                gameActive = false;
                miniGame.SetActive(false);
                playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                pressE.SetActive(true);

            }
            else if(!gameActive && near){
                pressE.SetActive(true);

            }
        }
    }
}
