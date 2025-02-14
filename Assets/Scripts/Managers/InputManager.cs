using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IDispose
{
    private InputSystem_Actions action;
    public Vector2 CameraInput;
    private IAction act;
    private EventBus eventBus;
    private void Awake()
    {
        action = new InputSystem_Actions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        act = FindAnyObjectByType<Player>().mainAction;
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<ChangeActionSignal>(SetAction<ChangeActionSignal>, 2);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
        Debug.Log(act.ToString());
    }

    private void OnEnable()
    {
        action.Enable();

        action.Player.Look.canceled += i => CameraInput = Vector2.zero;

        action.Player.Look.performed += i => CameraInput = i.ReadValue<Vector2>();
        action.Player.Attack.performed += InteractPerformed;
    }

    private void OnDisable()
    {
        action.Disable();

        action.Player.Look.performed -= i => CameraInput = i.ReadValue<Vector2>();
        action.Player.Attack.performed -= InteractPerformed;
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<ChangeActionSignal>(SetAction<ChangeActionSignal>);
    }


    private void InteractPerformed(InputAction.CallbackContext context)
    {
        act.Act();
    }

    private void SetAction<T>(T signal) where T : ISignal
    {
        act = FindAnyObjectByType<Player>().mainAction;
    }
}
