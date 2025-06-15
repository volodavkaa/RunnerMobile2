using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    
    [Header("Рух")]
    public float forwardSpeed = 5f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 10f;

   
    [Header("Стрибок")]
    public float jumpForce = 7f;
    public float groundCheckDist = 0.2f;
    public LayerMask groundLayer;
[Header("Auto-Speed-Up")]
public float speedStepMeters  = 200f;   
public float speedMultiplier  = 1.03f;  
public float maxForwardSpeed  = 15f;
private float nextSpeedMilestone = 50f;
    private int currentLane = 1;          
    private Vector3 targetPosition;

    private bool isGrounded = true;
    private bool isSliding  = false;

    private Vector2 touchStart;           
    private bool   isSwiping = false;

    private float   distanceTraveled = 0f;
    private Vector3 lastPosition;
    private int     totalMeters          = 0;
    private int     lastRewardedMilestone = 0;

    private Rigidbody rb;
    private Animator  anim;

    
    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        targetPosition = transform.position;
        lastPosition   = transform.position;
        anim           = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (!GameManager.Instance.IsGameStarted()) return;

       
      
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) Jump();
        if (Input.GetKeyDown(KeyCode.LeftArrow)  && currentLane > 0) currentLane--;
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2) currentLane++;
        if (Input.GetKeyDown(KeyCode.DownArrow)) SlideManager.Instance.TrySlide();


                
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                isSwiping  = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                Vector2 delta = touch.position - touchStart;
                isSwiping = false;

                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
{
    if (delta.y >  50 && isGrounded)
        Jump();
    else if (delta.y < -50 && isGrounded)
    SlideManager.Instance.TrySlide();

}

                else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    if (delta.x >  50 && currentLane < 2) currentLane++;
                    if (delta.x < -50 && currentLane > 0) currentLane--;
                }
            }
        }

        
        float targetX    = (currentLane - 1) * laneDistance;
        targetPosition   = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

        
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition      = transform.position;

        if (distanceTraveled >= 1f)                
    {
        int metersToAdd = Mathf.FloorToInt(distanceTraveled);
        distanceTraveled -= metersToAdd;

        totalMeters += metersToAdd;
        UIManager.Instance.UpdateMeters(totalMeters);

        
        MissionManager.Instance?.RegisterDistance(metersToAdd, false);

        
        if (totalMeters >= nextSpeedMilestone)
        {
            nextSpeedMilestone += speedStepMeters;

            float newSpeed = Mathf.Min(forwardSpeed * speedMultiplier,
                                    maxForwardSpeed);

            if (newSpeed > forwardSpeed + 0.01f)
            {
                float k = newSpeed / forwardSpeed;
                forwardSpeed     = newSpeed;
                laneChangeSpeed *= k;

                MessagePanel.Instance.ShowMessage($"Speed ↑  {forwardSpeed:0.#}");
            }
        }

        
        if (totalMeters >= lastRewardedMilestone + 500)
        {
            lastRewardedMilestone += 500;
            UIManager.Instance.AddCoin(50);
            MessagePanel.Instance.ShowMessage("Нагорода: +50 монет!");
        }
    }

        
        if (anim != null) anim.SetBool("isRunning", true);
    }

    
    void FixedUpdate()
    {
        CheckGround();

        
        Vector3 velocity = rb.velocity;
        velocity.z = forwardSpeed;
        rb.velocity = velocity;

        
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 newPos = rb.position;
        newPos.x = Mathf.Lerp(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    
    void Jump()
    {
        if (!isGrounded || isSliding) return;
        if (!JumpEnergyManager.Instance.UseEnergy()) return;   

        
        Vector3 v = rb.velocity; v.y = 0f; rb.velocity = v;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        if (anim != null) anim.SetTrigger("Jump");
        MissionManager.Instance?.RegisterJump();

    }
    public bool IsGrounded()
{
    return isGrounded;
}

    

    void CheckGround()
{
    Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
    RaycastHit hit;

    bool hitGround = Physics.Raycast(ray, out hit, groundCheckDist, groundLayer);

    if (!hitGround)
    {
        if (Physics.Raycast(ray, out hit, groundCheckDist))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                hitGround = true;
            }
        }
    }

    isGrounded = hitGround;
    if (anim != null)
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("verticalVelocity", rb.velocity.y);
        bool isMovingOnGround = isGrounded && rb.velocity.y <= 0.1f;
        anim.SetBool("isRunning", isMovingOnGround);
    }
}


}
