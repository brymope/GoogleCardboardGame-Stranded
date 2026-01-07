using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class TaskItem
{
    public string description;
    public bool completed;

    [Tooltip("Item IDs that must be collected to complete this task.")]
    public List<string> requiredItemIDs = new List<string>();

    [Tooltip("How many of each required item are needed.")]
    public int requiredCountPerItem = 1;
}

public class GameTaskManager : MonoBehaviour
{
    [Header("Tasks")]
    public List<TaskItem> tasks = new List<TaskItem>();

    [Header("UI")]
    public TMP_Text taskText;

    private int currentIndex = 0;
    public Inventory playerInventory;
    public bool loopWhenFinished = false;
    public ShipGlow shipGlow;
    public GameObject cutsceneShip;

    private void Start()
    {
        if (playerInventory == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerInventory = player.GetComponent<Inventory>();
        }

        if (playerInventory == null)
            Debug.LogWarning("GameTaskManager: No player inventory found!");

        currentIndex = FindNextIncompleteIndex(0);
        RefreshUI();
    }

    void RefreshUI()
    {
        if (currentIndex < 0 || currentIndex >= tasks.Count)
        {
            taskText.text = "All tasks completed!";
            Debug.Log("All tasks completed — starting end sequence.");
            cutsceneShip.GetComponent<CutsceneShipController>().PlayCutscene();

            return;
        }

        var task = tasks[currentIndex];

        int have = CountCollected(task);
        int need = task.requiredItemIDs.Count * task.requiredCountPerItem;

        if (need > 0)
            taskText.text = $"{task.description} ({have}/{need})";
        else
            taskText.text = task.description;
    }

    int FindNextIncompleteIndex(int startIndex)
    {
        for (int i = startIndex; i < tasks.Count; i++)
            if (!tasks[i].completed) return i;
        return -1;
    }

    int CountCollected(TaskItem task)
    {
        int total = 0;

        foreach (var id in task.requiredItemIDs)
        {
            int qty = playerInventory.GetQuantity(id);
            total += Mathf.Min(qty, task.requiredCountPerItem); // Clamp to required per item
        }

        return total;
    }


    public void RegisterItemPickup(string itemID)
    {
        var task = GetCurrentTask();
        if (task == null || task.completed) return;

        if (!task.requiredItemIDs.Contains(itemID)) return;

        int collected = CountCollected(task);
        int needed = task.requiredItemIDs.Count * task.requiredCountPerItem;

        RefreshUI();

        if (collected >= needed)
        {
            MarkCurrentComplete();
        }
    }

    public void MarkCurrentComplete()
    {
        var task = GetCurrentTask();
        if (task == null) return;

        task.completed = true;
        shipGlow?.SetGlow(true);

        AdvanceToNext();
    }

    void AdvanceToNext()
    {
        currentIndex = FindNextIncompleteIndex(currentIndex + 1);

        if (currentIndex == -1 && loopWhenFinished)
        {
            foreach (var t in tasks) t.completed = false;
            currentIndex = FindNextIncompleteIndex(0);
        }

        RefreshUI();
    }

    public TaskItem GetCurrentTask()
    {
        if (currentIndex < 0 || currentIndex >= tasks.Count)
            return null;
        return tasks[currentIndex];
    }
    public void OnPlayerGazedAtMonitor()
    {
        var task = GetCurrentTask();
        if (task == null)
        {
            Debug.Log("No current task.");
            return;
        }
        RefreshUI();
        // If task has required items, verify that the counts match
        if (task.requiredItemIDs.Count > 0)
        {
            int collected = CountCollected(task);
            int needed = task.requiredItemIDs.Count * task.requiredCountPerItem;

            if (collected < needed)
            {
                Debug.Log($"Task '{task.description}' not yet complete. Progress: {collected}/{needed}");
                return;
            }
        }

        // If requirement is zero OR requirements met
        Debug.Log($"Task '{task.description}' completed by monitor interaction.");
        MarkCurrentComplete();
    }


}
