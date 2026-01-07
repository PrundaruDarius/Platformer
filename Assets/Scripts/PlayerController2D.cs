using UnityEngine;

public sealed class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] SpriteRenderer sr;

    [Header("Jump")]
    [SerializeField] float jumpImpulse = 12f;
    [SerializeField] int maxAirJumps = 1;
    [SerializeField] float coyoteTime = 0.12f;
    [SerializeField] float jumpBuffer = 0.12f;

    [Header("Fast Fall (S)")]
    [SerializeField] float fastFallMultiplier = 2.2f;
    [SerializeField] float maxFallSpeed = 20f;

    [Header("Extra Gravity (optional feel)")]
    [SerializeField] bool useExtraGravity = true;
    [SerializeField] float extraGravity = 20f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;
    [SerializeField] LayerMask groundMask;

    [Header("Stick to Moving Platform")]
    [SerializeField] bool enablePlatformStick = true;
    [SerializeField] float stickDeadZone = 0.05f; 

    Rigidbody2D rb;

    float moveInput;
    float coyoteTimer;
    float bufferTimer;
    int airJumpsLeft;

    PlatformMover currentPlatform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        airJumpsLeft = maxAirJumps;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput < -0.01f) sr.flipX = true;
        else if (moveInput > 0.01f) sr.flipX = false;

        if (Input.GetKeyDown(KeyCode.W))
            bufferTimer = jumpBuffer;

        if (bufferTimer > 0f)
            bufferTimer -= Time.deltaTime;

        bool grounded = IsGrounded();

        if (grounded)
        {
            coyoteTimer = coyoteTime;
            airJumpsLeft = maxAirJumps;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (bufferTimer > 0f)
        {
            if (grounded || coyoteTimer > 0f)
            {
                DoJump();
                bufferTimer = 0f;
                coyoteTimer = 0f;
            }
            else if (airJumpsLeft > 0)
            {
                DoJump();
                airJumpsLeft--;
                bufferTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 v = rb.linearVelocity;

        
        v.x = moveInput * moveSpeed;

        
        if (enablePlatformStick && currentPlatform != null && Mathf.Abs(moveInput) <= stickDeadZone)
        {
            
            float scroll = (GameManager.Instance != null) ? GameManager.Instance.CurrentSpeed : 0f;
            v.x = -scroll;
        }

        
        bool grounded = IsGrounded();
        if (!grounded)
        {
            if (useExtraGravity && v.y < 0f)
                v.y -= extraGravity * Time.fixedDeltaTime;

            if (Input.GetKey(KeyCode.S) && v.y < 0f)
                v.y *= fastFallMultiplier;

            if (v.y < -maxFallSpeed)
                v.y = -maxFallSpeed;
        }

        rb.linearVelocity = v;
    }

    void DoJump()
    {
        Vector2 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;

        rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);

        currentPlatform = null;
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask) != null;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!enablePlatformStick) return;

        PlatformMover pm = col.collider.GetComponent<PlatformMover>();
        if (pm == null) return;

        
        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.GetContact(i).normal.y > 0.5f)
            {
                currentPlatform = pm;
                return;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (!enablePlatformStick) return;

        if (col.collider.GetComponent<PlatformMover>() != null)
            currentPlatform = null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    public bool IsGroundedPublic() => IsGrounded();
}
