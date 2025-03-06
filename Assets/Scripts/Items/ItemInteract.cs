using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ItemInteract : MonoBehaviour
{
    public async virtual Task Interact()
    {
        await Animation();
    }

    public virtual async void Effect()
    {

    }

    public virtual async Task Animation()
    {
        Effect();
    }

    public abstract void Release();
}
