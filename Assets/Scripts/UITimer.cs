using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text clock;

    private void Start()
    {
        //eventBus.Subscribe<DisplayTimeSignal>(UpdateTimer);
        //eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }
}
