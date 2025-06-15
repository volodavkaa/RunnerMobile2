using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Торкнулись: " + other.name);

    if (other.CompareTag("Player"))
    {
        SoundManager.Instance?.PlayCoinSound();
        UIManager.Instance.AddCoin(1);
        MissionManager.Instance?.RegisterCoin(1, true); 
        CoinPool.Instance.Return(gameObject);
    }
}
}
