using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // enable movement
    public bool canMove = true;

    // animator

    // rigidbody
    Rigidbody m_Rigidbody;

    // audio

    // Movement
    private float horizontal;
    private float vertical;
    public float turnSpeed = 20f;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool jumpRequest;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw removes smoothing, allowing for instant stops
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && canMove)
        {
            jumpRequest  = true;
        }
    }

    void FixedUpdate() {
        // check for movement ablility

        if (!canMove) 
        {
            return;
        }

        // jump if requested
        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }

        m_Movement.Set(horizontal, 0f, vertical);

        // dont move unless input
        if (m_Movement.sqrMagnitude > 0.01f)
        {
            m_Movement.Normalize();

            // set walking animation

            // play audio

            // rotate to desired direction
            Vector3 desiredDirection = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredDirection);

            // just moving here for now
            m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * moveSpeed * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation (m_Rotation);
        }

        else {
            // cancel out horizontal movement
            Vector3 currentVelocity = m_Rigidbody.linearVelocity;
            currentVelocity.x = 0f;
            currentVelocity.z = 0f;
            m_Rigidbody.linearVelocity = currentVelocity;
           // m_Rigidbody.linearVelocity = Vector3.zero;
            //m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    // move with animation
    // just moving for now

    private void Jump()
    // applies jump force
    {
        m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool isGrounded()
    // checks if the player is on a surface
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }


}
