using UnityEngine;

public class EnemyDecide : IUtilityAIMaker
{
    private readonly Enemy enemy;

    public EnemyDecide(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public EnemyAction DecideAction(EnemyAction[] actions)
    {

        foreach (EnemyAction action in actions)
        {
            float price = 0;

            foreach (Decision decisions in action.Decisions)
            {
                price += decisions.Decide(enemy);
            }
            price /= action.Decisions.Length;

            action.Price = price;
        }

        float bestPrice = 0;

        EnemyAction bestAction = null;
        foreach (EnemyAction action in actions)
        {
            if (action.Price < bestPrice)
            {
                bestPrice = action.Price;
                bestAction = action;
            }
        }

        return bestAction;
    }
}
