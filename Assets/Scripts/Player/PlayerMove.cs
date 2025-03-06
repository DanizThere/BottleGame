using System.Threading;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CancellationTokenSource CancellationTokenSource;
    private CancellationToken CancellationToken;
    private Quaternion originalQuaternion;
    private Vector3 originalVector;
    [SerializeField] private float duration, magnitude;
    [SerializeField] private Transform orientation;
    private CameraHandle cameraHandle;

    private void Awake()
    {
        cameraHandle = FindAnyObjectByType<CameraHandle>();
        originalQuaternion = Quaternion.Euler(0, 0, 0);
        originalVector = transform.position;
        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }

    public async Awaitable ShakeEffect(float duration, float magnitude)
    {
        Vector3 origPos = originalVector;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = Vector3.Slerp(transform.position, new Vector3(origPos.x + x, origPos.y + y, origPos.z), elapsed);
            elapsed += Time.deltaTime * 1.5f;
            await Awaitable.WaitForSecondsAsync(.001f);
        }

        transform.position = origPos;
    }

    public async Awaitable Flow(Vector3 destination, Quaternion rotation)
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

    private async Awaitable DoFlow(Vector3 origPos, Vector3 destination, Quaternion rotation, CancellationToken token)
    {
        token.Register(() =>
        {
            return;
        });
        orientation.rotation = rotation;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            if (token.IsCancellationRequested)
            {
                transform.position = destination;
                break;
            }
            cameraHandle.HandleRotate(orientation, t * 10);
            transform.position = Vector3.MoveTowards(origPos, destination, t * 10);

            await Awaitable.WaitForSecondsAsync(.001f);
        }
    }

    public async Awaitable FlowToFormerPos()
    {
        if (transform.position == originalVector)
        {
            CancelToken();
            orientation.rotation = originalQuaternion;
        }

        Vector3 formerPos = transform.position;

        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            cameraHandle.HandleRotate(transform, t * 10);
            transform.position = Vector3.MoveTowards(transform.position, originalVector, t * 10);

            await Awaitable.WaitForSecondsAsync(.001f);
        }
    }

    private void CancelToken()
    {
        CancellationTokenSource.Cancel();

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
    }
}
