using UnityEngine;
using System.Collections;

public class ShipGlow : MonoBehaviour
{
    public Renderer shipRenderer;
    public Color glowColor = Color.cyan;
    public float emissionIntensity = 3f;
    public float fadeDuration = 10f; // seconds to fade back

    private Material originalMaterial;
    private Material glowMaterial;
    private Coroutine fadeCoroutine;

    void Start()
    {
        // Assign Renderer automatically if not set
        if (shipRenderer == null)
            shipRenderer = GetComponent<Renderer>();

        if (shipRenderer == null)
            Debug.LogError("ShipGlow: No Renderer found!");

        // Store original material
        originalMaterial = shipRenderer.material;

        // Create glow material
        glowMaterial = new Material(originalMaterial);
        glowMaterial.EnableKeyword("_EMISSION");
        glowMaterial.SetColor("_EmissionColor", glowColor * emissionIntensity);

        // Start with original material
        shipRenderer.material = originalMaterial;
    }

    public void SetGlow(bool enable)
    {
        // Stop any ongoing fade
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        if (enable)
        {
            // Switch to glow material
            shipRenderer.material = glowMaterial;
            // Start fade back after delay
            fadeCoroutine = StartCoroutine(FadeBackToOriginal());
        }
        else
        {
            shipRenderer.material = originalMaterial;
        }
    }

    private IEnumerator FadeBackToOriginal()
    {
        float elapsed = 0f;

        Color startColor = glowMaterial.GetColor("_EmissionColor");
        Color endColor = Color.black; // emission black before switching back

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            Color c = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            glowMaterial.SetColor("_EmissionColor", c);
            yield return null;
        }

        // After fading, switch back to original material
        shipRenderer.material = originalMaterial;
        fadeCoroutine = null;
    }
}
