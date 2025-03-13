using UnityEngine;

public class HealthDecision : Decision
{
    public override float Decide(Enemy enemy)
    {
        return curve.Evaluate(enemy.Hits);
    }
}
