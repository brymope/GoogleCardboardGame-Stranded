using UnityEngine;
using UnityEngine.Playables;

public class EndSequenceController : MonoBehaviour
{
    public GameObject gameplayShip;
    public GameObject cutsceneShip;
    public PlayableDirector takeoffTimeline;

    //private bool sequencePlayed = false;

    public void PlayEndSequence()
    {
        cutsceneShip.GetComponent<CutsceneShipController>().PlayCutscene();

    }
}
