using UnityEngine;

public class UITimer : MonoBehaviour, IDispose
{
    [SerializeField] private TMPro.TMP_Text clock;
    private EventBus eventBus;

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<DisplayTimeSignal>(UpdateTimer);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    public void UpdateTimer(DisplayTimeSignal signal)
    {
        clock.text = signal.Time.ToString();
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<DisplayTimeSignal>(UpdateTimer);
    }

}
