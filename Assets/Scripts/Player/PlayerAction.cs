using UnityEngine;

public class PlayerAction : MonoBehaviour, IAction
{
    public void Act()
    {
        Debug.Log("�����");
    }

    public void TryBelief(DNDPerson person, CommonBottle bottle)
    {
    }

    public void Use(IInteractable interactable)
    {
    }
}
