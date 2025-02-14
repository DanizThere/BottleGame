using UnityEngine;

public interface IAction
{
    public void Act();

    public void TryBelief(DNDPerson person, CommonBottle bottle);

    public void Use(IInteractable interactable);
}
