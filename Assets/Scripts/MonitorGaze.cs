using UnityEngine;

public class MonitorGazeCompleter : Interactive
{
    [Tooltip("TaskManager that controls the retro screen UI")]
    public GameTaskManager taskManager;

    [Header("Optional visual attachment")]
    public GameObject attachPointForBooster;

    void Start()
    {
        if (taskManager == null)
            taskManager = FindAnyObjectByType<GameTaskManager>();
    }

    public override void Interact()
    {
        if (taskManager == null)
        {
            Debug.LogWarning("MonitorGazeCompleter: No GameTaskManager found!");
            return;
        }

        var currentTask = taskManager.GetCurrentTask();
        if (currentTask == null)
        {
            Debug.Log("Monitor: No active task.");
            return;
        }

        // Optional: Booster visual (just cosmetic)
        if (attachPointForBooster != null &&
            currentTask.description.ToLower().Contains("booster"))
        {
            attachPointForBooster.SetActive(true);
        }

        // Delegate all validation / completion logic to GameTaskManager
        taskManager.OnPlayerGazedAtMonitor();
    }
}
