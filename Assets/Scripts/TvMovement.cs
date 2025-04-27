using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // enable movement
    public bool canMove = true;

    // animator
    Animator ani;

    // rigidbody
    Rigidbody m_Rigidbody;

    // audio

    // Movement
    private float horizontal;
    public float sprintIncrease = 2f;
    private float vertical;
    public float turnSpeed = 20f;
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    private bool jumpRequest;
    private bool extraJump;
    private bool resetRequest;
    private bool sprintRequest;
    public Transform cameraTransform;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        extraJump = true;
        jumpRequest = false;
        sprintRequest = false;
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

        if (Input.GetKey(KeyCode.LeftShift)) {
            sprintRequest = true;
        }
        else {
            sprintRequest = false;
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
            // double Jump
            if (resetRequest)
            {
                resetRequest = false;
                Vector3 currentVert = m_Rigidbody.linearVelocity;
                currentVert.y = 0f;
                m_Rigidbody.linearVelocity = currentVert;
                Jump();
                ani.SetTrigger("IsDouble");
            }
            else {
                Jump();
                ani.SetTrigger("IsJumping");
            }

            Jump();
            jumpRequest = false;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        m_Movement = (cameraForward * vertical + cameraRight * horizontal);


        // dont move unless input
        if (m_Movement.sqrMagnitude > 0.01f)
        {
            Debug.Log("true");
            ani.SetBool("IsRunning", true);
            m_Movement.Normalize();

            // set walking animation

            // play audio

            // rotate to desired direction
            Vector3 desiredDirection = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredDirection);

            // just moving here for now
            if (sprintRequest) {
                m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * (sprintIncrease * moveSpeed) * Time.fixedDeltaTime);
                ani.SetBool("IsSprinting", true);

            }
            else {
                m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * moveSpeed * Time.fixedDeltaTime);
                ani.SetBool("IsSprinting", false);
            }
            //m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * moveSpeed * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation (m_Rotation);
        }

        else {
            // cancel out horizontal movement
            Vector3 currentVelocity = m_Rigidbody.linearVelocity;
            currentVelocity.x = 0f;
            currentVelocity.z = 0f;
            m_Rigidbody.linearVelocity = currentVelocity;
            ani.SetBool("IsRunning", false);
            Debug.Log("false");
           // m_Rigidbody.linearVelocity = Vector3.zero;
            //m_Rigidbody.angularVelocity = Vector3.zero;

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                float turnAmount = horizontal * turnSpeed * Time.fixedDeltaTime;
                Quaternion turnOffset = Quaternion.Euler(0, turnAmount, 0);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnOffset);
            }
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
