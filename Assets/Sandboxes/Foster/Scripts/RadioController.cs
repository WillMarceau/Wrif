using UnityEngine;

public class RadioController : MonoBehaviour
{

    public Transform player;

    public Rigidbody playerRigidBody;

    public GameObject miniGame;

    public GameObject openWalls;

    public GameObject pressE;

    public float interactionRange;

    private float distance;

    private bool near = false;

    private bool gameActive = false;


    // Called by pressing the Download Key Button, activates message saying the exits are open

    public void KeyGetPress(){
        openWalls.SetActive(true);
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
            //if they aren't close make sure that the messages are off
            near = false;
            pressE.SetActive(false);
            //this message is only active right after you complete the minigame and are still close

            openWalls.SetActive(false);



        }

        if(near)
        {
            //if the player presses e run the minigame
            if(Input.GetKeyDown(KeyCode.E) && !gameActive)
            {
                Debug.Log("Launching Game...");
                gameActive = true;
                pressE.SetActive(false);
                miniGame.SetActive(true);
                //make sure the player doesn't fall off- minigame uses time so can't freeze time
                playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;


            }
            else if(Input.GetKeyDown(KeyCode.E) && gameActive)
            {
                //close the game by pressing e again
                Debug.Log("Closing Game...");
                gameActive = false;
                miniGame.SetActive(false);
                playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                pressE.SetActive(true);

            }
            else if(!gameActive && near){
                Debug.Log("Player Near");
                pressE.SetActive(true);

            }
        }
    }
}
