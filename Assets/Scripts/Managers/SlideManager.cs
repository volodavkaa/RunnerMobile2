using UnityEngine;
using System.Collections;
using System.Linq;

public class SlideManager : MonoBehaviour
{
    public static SlideManager Instance;

    private bool isSliding = false;
    private Animator anim;
    private Rigidbody rb;
    private Collider playerCol;

    public float slideDuration = 0.9f;
    public float ignoreCollisionTime = 0.7f;

    void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody>();
        playerCol = GetComponent<Collider>();
    }

    public void TrySlide()
    {
        if (!PlayerController.Instance.IsGrounded() || isSliding) return;
        StartCoroutine(SlideRoutine());
    }

    public bool IsSliding()
    {
        return isSliding;
    }

    private IEnumerator SlideRoutine()
    {
        isSliding = true;

        if (anim != null) anim.SetTrigger("Slide");

        Vector3 v = rb.velocity;
        v.z = PlayerController.Instance.forwardSpeed;
        rb.velocity = v;

        StartCoroutine(TemporarilyIgnoreLowObstacles());

        yield return new WaitForSeconds(slideDuration);

        isSliding = false;
    }

    private IEnumerator TemporarilyIgnoreLowObstacles()
    {
        Collider[] lowObstacles = FindObjectsOfType<Obstacle>()
            .Where(o => o.CompareTag("LowObstacle"))
            .Select(o => o.GetComponent<Collider>())
            .Where(c => c != null)
            .ToArray();

        foreach (var col in lowObstacles)
            Physics.IgnoreCollision(playerCol, col, true);

        yield return new WaitForSeconds(ignoreCollisionTime);

        foreach (var col in lowObstacles)
            Physics.IgnoreCollision(playerCol, col, false);
    }
}
