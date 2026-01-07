using UnityEngine;

public class StartGameGazeButton : Interactive
{
    [Tooltip("Controller that manages enabling gameplay scripts")]
    public GameStartController gameStartController;

    [Tooltip("Optional visual feedback while gazing")]
    public GameObject gazeHighlight;

    private bool isGameStarted = false;

    void Start()
    {
        if (gameStartController == null)
            gameStartController = FindAnyObjectByType<GameStartController>();

        if (gazeHighlight != null)
            gazeHighlight.SetActive(false);
    }

    // Called by the Interactive system (handles dwell/twist automatically)
    public override void Interact()
    {
        if (isGameStarted) return;

        if (gameStartController == null)
        {
            Debug.LogWarning("StartGameGazeButton: No GameStartController found!");
            return;
        }

        // Optional visual feedback
        if (gazeHighlight != null)
            gazeHighlight.SetActive(true);

        gameStartController.StartGame();
        isGameStarted = true;

        // Optional: call base.Interact() if needed for internal Interactive logic
        // base.Interact();
    }
}
