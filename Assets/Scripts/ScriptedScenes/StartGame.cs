using UnityEngine;

public class StartGame : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private PlayerMove playerMove;
    [SerializeField] private Transform[] dots;
    private void Start()
    {
        dialogueManager = ServiceLocator.Instance.Get<DialogueManager>();
        playerMove = FindAnyObjectByType<PlayerMove>();
        Cutscene();
    }

    private async Awaitable Cutscene()
    {
        await Awaitable.WaitForSecondsAsync(1f);
    }

    //замена eventbus на событие старта
}
