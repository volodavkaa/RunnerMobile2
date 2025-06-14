using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
    public float destroyDistance = 25f;      

    private Transform player;
    private Collider  myCol;

    
    void Awake()
    {
        myCol  = GetComponent<Collider>();           
        player = PlayerController.Instance != null
               ? PlayerController.Instance.transform
               : null;
    }

    void OnEnable()                                 
    {
        if (player == null && PlayerController.Instance != null)
            player = PlayerController.Instance.transform;
    }

    void Update()
    {
        if (player == null) return;

        if (transform.position.z < player.position.z - destroyDistance)
            ObstaclePool.Instance.ReturnObstacle(gameObject);
    }

    void OnCollisionEnter(Collision collision)
{
    if (!collision.gameObject.CompareTag("Player"))
        return;
    if (CompareTag("LowObstacle"))
{
    Debug.Log("LowObstacle hit | isSliding: " + SlideManager.Instance.IsSliding()
);

    if (SlideManager.Instance.IsSliding()
)
    {
        Debug.Log("SLIDE SUCCESS → перешкода ігнорується");
        return;
    }
}

    Vector3 playerPos = collision.transform.position;
    Vector3 obstacleTop = transform.position + Vector3.up * 1.0f; 


    if (playerPos.y > obstacleTop.y)
    {
        return;
    }

    GameManager.Instance.LoseLife();
    ObstaclePool.Instance.ReturnObstacle(gameObject);
}
}
