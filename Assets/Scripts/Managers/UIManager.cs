using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IService, IDispose
{
    private EventBus eventBus;
    private DeathMenu deathMenu;
    public ListManager listManager { get; private set; }
    public CharacterList characterList;
    private Player player;

    [SerializeField] private GameObject DeadMenu;
    [SerializeField] private GameObject DialogueUI;

    public void Init(Player player, ListManager list, EventBus eventBusPref)
    {
        deathMenu = DeadMenu.GetComponent<DeathMenu>();
        if (DeadMenu != null) DeadMenu.SetActive(false);
        if (DialogueUI != null) DialogueUI.SetActive(false);

        eventBus = eventBusPref;

        eventBus.Subscribe<DeathSignal>(Death,0);
        eventBus.Subscribe<DeathSignal>(deathMenu.ShowInfo,1);
        eventBus.Subscribe<DialogueSignal>(DialogueStart);
        eventBus.Subscribe<StopDialogueSignal>(DialogueEnd);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

        this.player = player;

        listManager = list;

        characterList.Init(this.player);
    }

    public void Death(DeathSignal signal)
    {
        DeadMenu.SetActive(true);
    }

    public void DialogueStart(DialogueSignal signal) 
    {
        DialogueUI.SetActive(true);
    }

    public void DialogueEnd(StopDialogueSignal signal)
    {
        DialogueUI.SetActive(false);
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DeathSignal>(Death);
        eventBus.Unsubscribe<DialogueSignal>(DialogueStart);
        eventBus.Unsubscribe<StopDialogueSignal>(DialogueEnd);
    }
}
