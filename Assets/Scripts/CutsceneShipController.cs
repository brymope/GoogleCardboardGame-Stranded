using UnityEngine;

public class CutsceneShipController : MonoBehaviour
{
    public Transform startPoint;       // Empty GameObject placed on the ground
    public Transform endPoint;         // Empty GameObject high in the sky
    public float duration = 8f;        // seconds

    private float timer = 0f;
    private bool isPlaying = false;

    public GameObject gameplayShip;    // The ship the player was controlling

    public void PlayCutscene()
    {
        if (isPlaying) return;
        isPlaying = true;

        // Ensure the cutscene ship is visible and positioned at start
        gameObject.SetActive(true);
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;

        // Disable the gameplay ship
        if (gameplayShip != null)
            gameplayShip.SetActive(false);

        timer = 0f;
    }

    private void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        // Smoothly move and rotate
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
        transform.rotation = Quaternion.Slerp(startPoint.rotation, endPoint.rotation, t);

        // print position for debugging
        Debug.Log($"Cutscene Ship Position: {transform.position}");


        // Optionally destroy or disable after finishing
        if (t >= 1f)
        {
            isPlaying = false;
            // Optionally, you could destroy this object or enable effects here
        }
    }
}
