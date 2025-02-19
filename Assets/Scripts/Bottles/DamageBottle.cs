public class DamageBottle : CommonBottle
{
    public int damage = 2;
    public int iterations = 5;

    public void StandardEffect(DNDPerson person)
    {
        person.TakeDamage(damage);
        person.CheckHP();
        person.MinusLevelOfStress(damage * iterations);
    }

    public override void TakeEffect(DNDPerson person)
    {
        base.TakeEffect(person);
        StandardEffect(person);
    }

    public override void TakeEffect(Player player)
    {
        base.TakeEffect(player);
        StandardEffect(player);
        eventBus.Invoke(new DamageSignal(player.maxHit));
    }

    public override void TakeEffect(Enemy enemy)
    {
        base.TakeEffect(enemy);
        StandardEffect(enemy);
    }
}
