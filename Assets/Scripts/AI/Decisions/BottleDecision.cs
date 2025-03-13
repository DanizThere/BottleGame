using UnityEngine;

public class BottleDecision : Decision
{
    public override float Decide(Enemy enemy)
    {
        return curve.Evaluate(enemy.BottlePrice);
    }
}
