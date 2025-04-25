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
    public float jumpForce = 2f;
    private bool jumpRequest;
    private bool extraJump;
    private bool resetRequest;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    public Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        extraJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw removes smoothing, allowing for instant stops
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // not very efficent I dont think
        if (isGrounded()) {
            extraJump = true;
        }

        if (Input.GetButtonDown("Jump") && canMove && isGrounded())
        {
            jumpRequest  = true;
        }
        else if (Input.GetButtonDown("Jump") && canMove && !isGrounded() && extraJump)
        {
            // reset vertical velocity
            resetRequest = true;
            jumpRequest = true;
            extraJump = false;
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
            if (resetRequest)
            {
                resetRequest = false;
                Vector3 currentVert = m_Rigidbody.linearVelocity;
                 currentVert.y = 0f;
                m_Rigidbody.linearVelocity = currentVert;
            }

            Jump();
            jumpRequest = false;
        }

        // TEST CODE BELOW: ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // get camera directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // flatten to horizontal plane
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // combine input with camera direction
        Vector3 moveDirection = camForward * input.y + camRight * input.x;
        moveDirection.Normalize(); // keep consistent speed even diagonally

        // apply movement
        Vector3 velocity = moveDirection * moveSpeed;
            // preserve vertical physics (eg. gravity)
        m_Rigidbody.linearVelocity = new Vector3(velocity.x, m_Rigidbody.linearVelocity.y, velocity.z);

        // TEST CODE ABOVE ~~~~~~~~~~~~~~~~~~~~~

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
