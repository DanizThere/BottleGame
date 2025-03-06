public class DamageBottle : CommonBottle
{
    public int damage = 2;
    public int iterations = 5;

    public void StandardEffect(DNDManipulator person)
    {
        person.TakeDamage(damage);
        person.CheckHP();
        person.MinusLevelOfStress(damage * iterations);
    }

    public override void SetEffect(Player player)
    {
        base.TakeEffect(player);
        StandardEffect(player.dndManipulator);
        eventBus.Invoke(new DamageSignal());
    }

    public override void SetEffect(Enemy enemy)
    {
        base.TakeEffect(enemy);
        StandardEffect(enemy.manipulator);
    }
}
