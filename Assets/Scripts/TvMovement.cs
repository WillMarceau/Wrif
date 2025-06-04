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
    public AudioClip jumpSound;
    public AudioClip doubleJumpSound;
    private AudioSource audioSource;

    // Movement
    private float horizontal;
    public float sprintIncrease = 2f;
    private float vertical;
    // used for camera based movement (third person follow)
    /*public float turnSpeed = 20f;*/
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    private bool jumpRequest;
    private bool extraJump;
    private bool resetRequest;
    private bool sprintRequest;
    private bool airborn;
    private bool slideRequest;
    public Transform tp1;
    public Transform tp2;
    public Transform tp3;

    
    public Transform cameraTransform;
    // camera based movement (third person follow)
    /*
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    */

    // coliders
    public CapsuleCollider normalCollider;
    public CapsuleCollider normalFriction;
    public BoxCollider slidingCollider;
    public CapsuleCollider slidingFriction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        extraJump = true;
        jumpRequest = false;
        sprintRequest = false;
        airborn = false;
        slideRequest = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && tp1 != null)
        {
            Teleport(tp1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && tp2 != null)
        {
            Teleport(tp2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && tp3 != null)
        {
            Teleport(tp3);
        }
        // GetAxisRaw removes smoothing, allowing for instant stops
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // not very efficent I dont think
        if (isGrounded()) {
            extraJump = true;
            airborn = false;
        }
        else {
            airborn = true;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            sprintRequest = true;
        }
        else {
            sprintRequest = false;
        }

        // what happens if you slide and jump at the same time
        if (Input.GetKeyDown(KeyCode.C) && isGrounded() && (inputDirection.magnitude >= 0.1f)) {
            slideRequest = true;
        }

        else if (Input.GetButtonDown("Jump") && canMove && isGrounded() && !slideRequest)
        {
            jumpRequest  = true;

            //play jump sound effect
            audioSource.PlayOneShot(jumpSound);
        }
        else if (Input.GetButtonDown("Jump") && canMove && !isGrounded() && extraJump && !slideRequest) 
        {
            // reset vertical velocity
            resetRequest = true;
            jumpRequest = true;
            extraJump = false;

            //play double jump sound effect
            audioSource.PlayOneShot(doubleJumpSound);

        }
    }
    
    void FixedUpdate()
    {
        if (!canMove) return;
        
        if (jumpRequest)
        {
            if (resetRequest)
            {
                resetRequest = false;
                Vector3 currentVel = m_Rigidbody.linearVelocity;
                currentVel.y = 0f;
                m_Rigidbody.linearVelocity = currentVel;
                Jump();
                ani.SetTrigger("IsDouble");
            }
            else
            {
                Jump();
                ani.SetTrigger("IsJumping");
            }

            jumpRequest = false;
        }

        /*
        if (slideRequest)
        {
            ani.SetBool("IsSliding", true);
            Slide();
        }
        */

        if (airborn && !slideRequest)
        {
            ani.SetBool("IsAirborn", true);
        }
        else
        {
            ani.SetBool("IsAirborn", false);
        }


        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            if (slideRequest)
        {
            ani.SetBool("IsSliding", true);
            Slide();
        }

            // Get camera directions
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten the camera vectors
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate movement direction
            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;
            moveDirection.Normalize();

            // Move
            float currentSpeed = sprintRequest ? moveSpeed * sprintIncrease : moveSpeed;
            m_Rigidbody.MovePosition(m_Rigidbody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);

            // Rotate player toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);

            // Animation
            ani.SetBool("IsRunning", true);
            ani.SetBool("IsSprinting", sprintRequest);
        }
        else
        {
            // Stop animation
            ani.SetBool("IsRunning", false);
            ani.SetBool("IsSprinting", false);
        }
    }

    void Teleport(Transform location)
    {
        m_Rigidbody.position = location.position;
        m_Rigidbody.linearVelocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }


    // SCRIPT FOR MOVEMENT BASED ON THIRD PERSON FOLLOW CAMERA
    /*
   

                /*
                // dont move unless input
                // this is for movement forward
                if (new Vector2(0f, vertical).sqrMagnitude > 0.01f)
                {

                    // dont rotate unless actually moving
                    ani.SetBool("IsRunning", true);
                    m_Movement.Normalize();

                    // play audio

                    // dont rotate unless actually moving

                    // rotate to desired direction
                    // dont rotate unless horizontal input
                    if (Mathf.Abs(horizontal) > 0.01f) {
                    Vector3 desiredDirection = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
                    m_Rotation = Quaternion.LookRotation(desiredDirection);
                    m_Rigidbody.MoveRotation(m_Rotation);
                    }

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
                    //m_Rigidbody.MoveRotation(m_Rotation);
                }

                // turn in palce if only horizontal input
                else if (Mathf.Abs(horizontal) > 0.01f) {
                    // cancel out momentum
                    Vector3 currentVelocity = m_Rigidbody.linearVelocity;
                    currentVelocity.x = 0f;
                    currentVelocity.z = 0f;
                    m_Rigidbody.linearVelocity = currentVelocity;

                    // turn
                    float turnAmount = horizontal * (turnSpeed* 40f) * Time.fixedDeltaTime;
                    Quaternion turn = Quaternion.Euler(0, turnAmount, 0);
                    m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turn);

                }

                else {
                    // cancel out horizontal movement
                    Vector3 currentVelocity = m_Rigidbody.linearVelocity;
                    currentVelocity.x = 0f;
                    currentVelocity.z = 0f;
                    m_Rigidbody.linearVelocity = currentVelocity;
                    ani.SetBool("IsRunning", false);
                    //m_Rigidbody.angularVelocity = Vector3.zero;
                    //m_Rigidbody.rotation = Quaternion.identity;

                    if (Mathf.Abs(horizontal) > 0.1f)
                    {
                        float turnAmount = horizontal * turnSpeed * Time.fixedDeltaTime;
                        Quaternion turnOffset = Quaternion.Euler(0, turnAmount, 0);
                        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnOffset);
                    }
                }

                //m_Rigidbody.linearVelocity = Vector3.zero;
                //Debug.Log(m_Rigidbody.rotation);
                m_Rigidbody.angularVelocity = Vector3.zero;
            }

        
    }
    */
    // END OF SCRIPT BASED ON THIRD PERSON FOLLOW

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
        return Physics.Raycast(transform.position, Vector3.down, 0.3f);
    }

    private void Slide()
    // alters collision to fit slide animation
    {
        normalCollider.enabled = false;
        normalFriction.enabled = false;

        slidingCollider.enabled = true;
        //slidingFriction.enabled = true;

    }

    public void EndSlide()
    {
        ani.SetBool("IsSliding", false);

        // reset colliders
        normalCollider.enabled = true;
        normalFriction.enabled = true;

        slidingCollider.enabled = false;
        //slidingFriction.enabled = false;

        slideRequest = false;
    }
}
