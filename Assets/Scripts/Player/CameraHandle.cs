using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CameraHandle : MonoBehaviour
{
    public Transform Handler;
    private void FixedUpdate()
    {
        Handle(Handler);
    }

    public void Handle(Transform handle)
    {
        transform.position = handle.position;
    }

    public async void HandleRotate(Transform handle, float time)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, handle.rotation, time);
    }
}
