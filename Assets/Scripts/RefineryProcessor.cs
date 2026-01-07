using System.Collections;
using UnityEngine;

public class RefineryProcessor : Interactive
{
    [Header("Refinery Settings")]
    public string requiredItemID = "soil_sample";
    public float processingDuration = 5f;
    public Renderer refineryRenderer;

    private Inventory playerInventory;
    private bool isProcessing = false;

    private Material refineryMat;
    private Color baseEmissionColor;
    public ParticleSystem BlueSmoke;

    [Header("Fuel Spawn")]
    [Tooltip("Prefab of collectable fuel container")]
    public GameObject fuelContainerPrefab;
    [Tooltip("Position where fuel should appear")]
    public Transform fuelSpawnPoint;

    void Start()
    {
        if (refineryRenderer == null)
            refineryRenderer = GetComponentInChildren<Renderer>();

        if (refineryRenderer != null)
        {
            refineryMat = refineryRenderer.material;
            baseEmissionColor = refineryMat.GetColor("_EmissionColor");
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerInventory = player.GetComponent<Inventory>();
    }

    public override void Interact()
    {
        Debug.Log("Interacted with Refinery");

        if (playerInventory == null)
        {
            Debug.LogWarning("RefineryProcessor: No Inventory found.");
            return;
        }

        if (!playerInventory.HasItemID(requiredItemID))
        {
            Debug.Log("Refinery: Soil Sample missing!");
            return;
        }

        if (!isProcessing)
        {
            StartCoroutine(ProcessSample());
        }
    }

    private IEnumerator ProcessSample()
    {
        Debug.Log("Refinery started processing...");
        isProcessing = true;

        float elapsed = 0f;

        // Glow effect loop
        while (elapsed < processingDuration)
        {
            float pulse = (Mathf.Sin(Time.time * 6f) + 1f) * 0.5f; // 0 to 1
            refineryMat.SetColor("_EmissionColor", baseEmissionColor * (1f + pulse * 2f));

            elapsed += Time.deltaTime;
            yield return null;
        }

        // return the color back to the original color
        refineryMat.SetColor("_EmissionColor", baseEmissionColor);
        // start neon green smoke
        if (BlueSmoke != null)
            BlueSmoke.Play();
        Debug.Log("Refinery processing complete!");

        // Optional: remove soil sample after processing
        playerInventory.RemoveItemID(requiredItemID);
        // Spawn fuel container
        if (fuelContainerPrefab != null && fuelSpawnPoint != null)
        {
            Instantiate(fuelContainerPrefab,
                        fuelSpawnPoint.position,
                        fuelSpawnPoint.rotation);
            Debug.Log("Refinery: Fuel container spawned!");
        }
        else
        {
            Debug.LogWarning("Refinery: Fuel spawn references missing!");
        }
        // Optional: mark task complete if needed
        // FindObjectOfType<GameTaskManager>()?.MarkCurrentComplete();

        isProcessing = false;
    }
}
