using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovingObstacle : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 12f;          
    public float destroyBehind = 25f;  

    Transform player;

    void OnEnable()
    {
        if (player == null && PlayerController.Instance != null)
            player = PlayerController.Instance.transform;
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        if (player && transform.position.z < player.position.z - destroyBehind)
            ObstaclePool.Instance.ReturnObstacle(gameObject);
    }
public void Init(float newSpeed)
{
    speed = newSpeed;
}

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        float roofY = transform.position.y + 1.0f;

        if (col.transform.position.y > roofY) return;   

        GameManager.Instance.LoseLife();
        ObstaclePool.Instance.ReturnObstacle(gameObject);
    }
}
