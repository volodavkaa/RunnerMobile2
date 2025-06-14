using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MessagePanel : MonoBehaviour
{
    public static MessagePanel Instance;

    [Header("Refs")]
    public RectTransform panelTransform;
    public TMP_Text      messageText;

    [Header("Timing")]
    public float slideDuration = 0.3f;
    public float visibleTime   = 2f;

    Vector2 hiddenPos, shownPos;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
{
    LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);
    float h = panelTransform.rect.height;

    float topInset = Screen.height - (Screen.safeArea.y + Screen.safeArea.height);
    hiddenPos = new Vector2(0,  topInset + h + 20);   
    shownPos  = new Vector2(0, -topInset - 20);       

    panelTransform.anchoredPosition = hiddenPos;
    gameObject.SetActive(false);
}

    public void ShowMessage(string msg)
    {
        gameObject.SetActive(true);           
        StopAllCoroutines();
        StartCoroutine(Animate(msg));
    }

    IEnumerator Animate(string msg)
    {
        messageText.text = msg;
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);

  
        yield return Slide(hiddenPos, shownPos);

        yield return new WaitForSecondsRealtime(visibleTime);

 
        yield return Slide(shownPos, hiddenPos);

        gameObject.SetActive(false);         
    }

    IEnumerator Slide(Vector2 from, Vector2 to)
    {
        float t = 0;
        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;
            panelTransform.anchoredPosition = Vector2.Lerp(from, to, t / slideDuration);
            yield return null;
        }
        panelTransform.anchoredPosition = to;
    }
}
