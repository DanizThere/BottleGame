using UnityEngine;

public interface IUtilityAIMaker
{
    public EnemyAction DecideAction(EnemyAction[] actions);

}
