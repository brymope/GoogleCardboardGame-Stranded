using UnityEngine;

public class GameStartController : MonoBehaviour
{
    [Header("Canvas & Player Scripts")]
    public GameObject startCanvas;       // World-space start screen canvas
    public MonoBehaviour[] playerScripts; // Scripts to enable when game starts (Inventory, Pickup, Movement, etc.)

    void Start()
    {
        // Show start canvas
        if (startCanvas != null)
            startCanvas.SetActive(true);

        // Disable gameplay scripts at start
        if (playerScripts != null)
        {
            foreach (var script in playerScripts)
                script.enabled = false;
        }

        // Disable all teleport surface colliders at start
        TeleportSurface[] teleporters = GameObject.FindObjectsByType<TeleportSurface>(FindObjectsSortMode.None);
        foreach (var t in teleporters)
        {
            Collider col = t.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }

    // Called by the gaze button to start the game
    public void StartGame()
    {
        // Hide start canvas
        if (startCanvas != null)
            startCanvas.SetActive(false);

        // Enable all gameplay scripts
        if (playerScripts != null)
        {
            foreach (var script in playerScripts)
                script.enabled = true;
        }

        // Enable teleport surface colliders
        TeleportSurface[] teleporters = GameObject.FindObjectsByType<TeleportSurface>(FindObjectsSortMode.None);
        foreach (var t in teleporters)
        {
            Collider col = t.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;
        }
    }
}
