using UnityEngine;

public class KeyboardTestCompleter : MonoBehaviour
{
    public GameTaskManager taskManager;
    public KeyCode completeKey = KeyCode.Space;

    void Update()
    {
        if (taskManager == null) return;
        if (Input.GetKeyDown(completeKey))
        {
            taskManager.MarkCurrentComplete();
        }
    }
}
