using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform container;

    private List<GameObject> hearts = new List<GameObject>();

    public void InitHearts(int count)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
        hearts.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject heart = Instantiate(heartPrefab, container);
            hearts.Add(heart);
        }
    }

    public void LoseHeart()
    {
        if (hearts.Count == 0) return;

        GameObject last = hearts[hearts.Count - 1];
        hearts.RemoveAt(hearts.Count - 1);
        Destroy(last);
    }
}
