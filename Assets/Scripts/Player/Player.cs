using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Player : DNDPerson, IPlayerInput, IAction, IDispose
{
    private CameraMove cam;
    private CameraHandle cameraHandle;
    private Camera cams;
    private GameManager gameManager;

    public bool canUse;
    private bool handleAction;
    private Quaternion originalQuaternion;
    private Vector3 originalVector;
    [SerializeField] private Transform orientation;

    private CancellationTokenSource CancellationTokenSource;
    private CancellationToken CancellationToken;
    private void Awake()
    {
        originalQuaternion = Quaternion.Euler(0,0,0);
        originalVector = transform.position;

        personClass = GetComponent<DNDClasses>();
        cams = Camera.main;
        cam = cams.GetComponent<CameraMove>();
        cameraHandle = FindAnyObjectByType<CameraHandle>();

        PersonInit(1.1f, personClass, TypeOfPerson.PLAYER, 100, 1, 8, 8, 8, 8, 8, 8, "Без имени");

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        gameManager = ServiceLocator.Instance.Get<GameManager>();

        eventBus.Subscribe<StartUseSignal>(SetUseTrue);
        eventBus.Subscribe<StopUseSignal>(SetUseFalse);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

    }

    public void SetUseTrue(StartUseSignal signal)
    {
        canUse = true;
    }

    public void SetUseFalse(StopUseSignal signal) { canUse = false; }

    public void Dispose(UnsubscibeSignal signal)
    {
    }

    public override void CheckHP()
    {
        if(hits <= 0)
        {
            savethrowsFromDeath--;
            eventBus.Invoke(new WhenDeadSignal(this, savethrowsFromDeath));
        }
    }

    public override bool CanBelief()
    {
        return true;
    }

    public async Task Flow(Vector3 destination,Quaternion rotation)
    {
        if (transform.position == destination)
        {
            Debug.Log(destination);
            CancelToken();
        }

        Vector3 formerPos = transform.position;

        await DoFlow(formerPos, destination, rotation, CancellationToken);

        orientation.rotation = rotation;
        transform.position = destination;
    }

    private async Task DoFlow(Vector3 origPos, Vector3 destination, Quaternion rotation, CancellationToken token)
    {
        token.Register(() =>
        {
            return;
        });
        orientation.rotation = rotation;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            if(token.IsCancellationRequested)
            {
                transform.position = destination;
                break;
            }
            cameraHandle.HandleRotate(orientation, t * 10);
            transform.position = Vector3.MoveTowards(origPos, destination, t * 10);

            await Task.Delay(TimeSpan.FromSeconds(.001f));
        }
    }

    public async Task FlowToFormerPos()
    {
        if (transform.position == originalVector) { 
            CancelToken();
            orientation.rotation = originalQuaternion;
        }

        Vector3 formerPos = transform.position;

        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            cameraHandle.HandleRotate(transform, t * 10);
            transform.position = Vector3.MoveTowards(transform.position, originalVector, t * 10);

            await Task.Delay(TimeSpan.FromSeconds(.001f));
        }
    }

    private void CancelToken()
    {
        CancellationTokenSource.Cancel();

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }

    public void Act()
    {
        if (!canUse) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.MouseOnWorldScreen(), out hit))
        {
            var bottle = hit.collider.GetComponent<CommonBottle>();
            var characterList = hit.collider.GetComponent<CharacterList>();
            var cell = hit.collider.GetComponent<ListCell>();
            if (bottle != null)
            {
                bottle.TakeEffect(this);
            }
            if (characterList != null)
            {
                Use(characterList);
            }
            if (cell != null)
            {
                Use(cell);
            }
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    public void Use(IInteractable interactable)
    {
        interactable.Interact();
    }

    public void Cancel()
    {
        if (gameManager.GameState == GameState.On_UI)
        {
            ServiceLocator.Instance.Get<GameManager>().SetGameState(GameState.On_Game);
            FlowToFormerPos();
            ServiceLocator.Instance.Get<UIManager>().listManager.DisableAllButtonsInList();
        }

    }

    public void TryBelief(DNDPerson person, CommonBottle bottle)
    {
        if (person.CanBelief())
        {
            if (person.characteristics["Charisma"][1] > UnityEngine.Random.Range(0, (int)Dices.D20))
            {
                bottle.TakeEffect(person);
                person.PlusLevelOfStress(30);
            }
            return;
        }
    }
}
