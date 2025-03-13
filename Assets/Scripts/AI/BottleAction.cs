using UnityEngine;

public class BottleAction : EnemyAction
{
    public override void Action(Enemy enemy)
    {
        if (CanAction(enemy) == false) return;
        Debug.Log("I choose");
        enemy.ChosenBottle.TakeEffect(enemy.Manipulator);
    }

    public override bool CanAction(Enemy enemy)
    {
        return ServiceLocator.Instance.Get<GameManager>().CheckTurn(enemy.YourTurn.Name);
    }

    public override void Epilogue(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }

}
