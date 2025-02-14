using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Player : DNDPerson, IDispose
{
    private CameraMove cam;
    private CameraHandle cameraHandle;
    private Camera cams;
    private PlayerCommon PlayerCommon;
    private PlayerAction PlayerAction;
    public bool canUse;
    public IAction mainAction { get; private set; }
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
        PlayerAction = GetComponent<PlayerAction>();
        PlayerCommon = GetComponent<PlayerCommon>();
        cams = Camera.main;
        cam = cams.GetComponent<CameraMove>();
        cameraHandle = FindAnyObjectByType<CameraHandle>();

        canUse = true;

        PersonInit(1.1f, personClass, TypeOfPerson.PLAYER, 100, 1, 8, 8, 8, 8, 8, 8, "Без имени");

        PlayerCommon.Init(this, cam);
        mainAction = PlayerCommon;

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }

    public override void PersonInit(float stressMultipliyer, DNDClasses personClass, TypeOfPerson typeOfPerson, int maxLevelOfStress, int savethrowsFromDeath, int characteristicStrength, int characteristicDexterity, int characteristicConstitution, int characteristicIntelligence, int characteristicWisdom, int characteristicCharisma, string personName)
    {
        base.PersonInit(stressMultipliyer, personClass, typeOfPerson, maxLevelOfStress, savethrowsFromDeath, characteristicStrength, characteristicDexterity, characteristicConstitution, characteristicIntelligence, characteristicWisdom, characteristicCharisma, personName);
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        eventBus.Subscribe<ChangeActionSignal>(SetAction);
        eventBus.Subscribe<StartUseSignal>(SetUseTrue);
        eventBus.Subscribe<StopUseSignal>(SetUseFalse);
        eventBus.Subscribe<UnsubscibeSignal>(Dispose);

    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { FlowToFormerPos(); }
    }

    private void SetAction(ChangeActionSignal signal)
    {
        mainAction = handleAction == true ? PlayerCommon : PlayerAction;
        handleAction = !handleAction;   
    }

    public void SetUseTrue(StartUseSignal signal)
    {
        canUse = true;
    }

    public void SetUseFalse(StopUseSignal signal) { canUse = false; }

    public void Dispose(UnsubscibeSignal signal)
    {
        eventBus.Unsubscribe<ChangeActionSignal>(SetAction);
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

        orientation.rotation = Quaternion.identity;
        transform.position = destination;
    }

    private async Task DoFlow(Vector3 origPos, Vector3 destination, Quaternion rotation, CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            if(token.IsCancellationRequested)
            {
                transform.position = destination;
                t = 1;
                break;
            }
            cameraHandle.HandleRotate(orientation, t);
            transform.position = Vector3.MoveTowards(origPos, destination, t * 10);

            await Task.Delay(TimeSpan.FromSeconds(.001f));
        }
    }

    public async Task FlowToFormerPos()
    {
        if (transform.position == originalVector) { 
            CancelToken();
            orientation.rotation = Quaternion.identity;
        }

        Vector3 formerPos = transform.position;

        await DoFlow(formerPos, originalVector, originalQuaternion, CancellationToken);
    }

    private void CancelToken()
    {
        CancellationTokenSource.Cancel();

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }
}
