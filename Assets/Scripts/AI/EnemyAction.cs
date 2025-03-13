using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    public float Price;
    public Decision[] Decisions;

    public abstract bool CanAction(Enemy enemy);
    public abstract void Action(Enemy enemy);
    public abstract void Epilogue(Enemy enemy);
}
