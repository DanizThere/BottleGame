

public class HealingBottle : CommonBottle
{
    public int healing = 2;
    public int iterations = 5;

    public void StandardEffect(DNDManipulator person)
    {
        person.TakeHeal(healing);
        person.SetIntervalAfterHeal();
        person.PlusLevelOfStress(healing * iterations);
    }

    public override void TakeEffect(DNDManipulator person)
    {
        base.TakeEffect(person);
        StandardEffect(person);  
    }

    public override void SetEffect(Player player)
    {
        base.TakeEffect(player);
        StandardEffect(player.Manipulator);
    }

    public override void SetEffect(Enemy enemy)
    {
        base.TakeEffect(enemy);
        StandardEffect(enemy.Manipulator);
    }
}
