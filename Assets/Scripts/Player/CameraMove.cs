using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour, IMoveable, IDispose
{
    private InputManager inputManager;
    private EventBus eventBus;
    private Player player;
    public float minRotY = -45f, maxRotY = 45f;
    public float minRotX = -90f, maxRotX = 90f;
    [SerializeField] private Transform orientation; 
    [SerializeField] private float sens = 1f;
    private bool canMove;
    private Camera cam;

    private float rotX, rotY;
    private void Start()
    {
        cam = Camera.main;
        inputManager = FindAnyObjectByType<InputManager>();

        player = FindAnyObjectByType<Player>();

        eventBus = ServiceLocator.Instance.Get<EventBus>();
        eventBus.Subscribe<OnUISignal>(OnUI);
        eventBus.Subscribe<OffUISignal>(OffUI);
        eventBus.Subscribe<StopPlaySignal>(SetMoveFalse);
        eventBus.Subscribe<StopPlaySignal>(SetOriginalCamera);
        eventBus.Subscribe<StartPlaySignal>(SetMoveTrue);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);
    }

    private void LateUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (!canMove) return;

        float x = inputManager.CameraInput.x * Time.fixedDeltaTime * sens;
        float y = inputManager.CameraInput.y * Time.fixedDeltaTime * sens;

        rotY += x;
        rotX -= y;
        rotY = Mathf.Clamp(rotY, minRotY, maxRotY);
        rotX = Mathf.Clamp(rotX, minRotX, maxRotX);

        transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.rotation = Quaternion.Euler(0, rotY, 0);
    }

    public Ray MouseOnWorldScreen()
    {
        Vector3 mousePos = new Vector3(inputManager.CameraInput.x, inputManager.CameraInput.y, 0);
        mousePos.z = cam.nearClipPlane;

        return new Ray(transform.position, transform.forward * 25);
    }

    public Vector3 MouseOnViewScreen()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        return cam.ScreenToViewportPoint(mousePos);
    }

    private void SetOriginalCamera(StopPlaySignal signal)
    {
        rotX = 0;
        rotY = 0;
        transform.rotation = Quaternion.identity;
    }

    public void SetMoveTrue(StartPlaySignal signal)
    {
        canMove = true;
    }

    public void SetMoveFalse(StopPlaySignal signal)
    {
        canMove = false;
    }

    public void OnUI(OnUISignal signal)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OffUI(OffUISignal signal)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<StopPlaySignal>(SetMoveFalse);
    }
}
