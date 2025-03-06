using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IDispose
{
    private InputSystem_Actions action;
    public Vector2 CameraInput;
    private IPlayerInput act;
    private EventBus eventBus;
    private void Awake()
    {
        action = new InputSystem_Actions();
    }

    private void Start()
    {
        act = FindAnyObjectByType<Player>().GetComponent<IPlayerInput>();
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    private void OnEnable()
    {
        action.Enable();

        action.Player.Look.canceled += i => CameraInput = Vector2.zero;
        action.UI.Cancel.performed += CancelPerformed;

        action.Player.Look.performed += i => CameraInput = i.ReadValue<Vector2>();
        action.Player.Attack.performed += InteractPerformed;
    }

    private void OnDisable()
    {
        action.Disable();

        action.Player.Look.performed -= i => CameraInput = i.ReadValue<Vector2>();
        action.UI.Cancel.performed -= CancelPerformed;
        action.Player.Attack.performed -= InteractPerformed;
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        
    }


    private void InteractPerformed(InputAction.CallbackContext context)
    {
        act.Act();
    }

    private void CancelPerformed(InputAction.CallbackContext context)
    {
        act.Cancel();
    }
}
