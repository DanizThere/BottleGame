using UnityEngine;

public interface IPlayerInput
{
    public void Act();
    public void Cancel();
    public void Use(IInteractable interactable);
}
