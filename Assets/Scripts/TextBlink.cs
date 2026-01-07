using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    public float fadeSpeed = 2f;   // how fast the blink fades

    private TextMeshProUGUI txt;

    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (txt == null) return;

        // Smooth fade between 0 → 1 → 0
        float alpha = (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f;
        txt.alpha = alpha;
    }
}
