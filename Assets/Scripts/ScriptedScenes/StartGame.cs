using UnityEngine;

public class StartGame : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private PlayerMove playerMove;
    private EventBus eventBus;
    [SerializeField] private Transform[] dots;
    private void Start()
    {
        dialogueManager = ServiceLocator.Instance.Get<DialogueManager>();
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        playerMove = FindAnyObjectByType<PlayerMove>();
        Cutscene();
    }

    private async Awaitable Cutscene()
    {
        await Awaitable.WaitForSecondsAsync(1f);
        eventBus.Invoke(new DialogueSignal("1_security_start_1"));

    }
}
