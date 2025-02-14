using UnityEngine;

public interface IEnemy
{
    public bool Check();

    public void SuccessCheck(IAction action);
}
