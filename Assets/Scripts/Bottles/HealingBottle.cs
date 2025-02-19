

public class HealingBottle : CommonBottle
{
    public int healing = 2;
    public int iterations = 5;

    public void StandardEffect(DNDPerson person)
    {
        person.TakeHeal(healing);
        person.SetIntervalAfterHeal();
        person.PlusLevelOfStress(healing * iterations);
    }

    public override void TakeEffect(DNDPerson person)
    {
        base.TakeEffect(person);
        StandardEffect(person);  
    }

    public override void TakeEffect(Enemy enemy)
    {
        base.TakeEffect(enemy);
        StandardEffect(enemy);
    }

    public override void TakeEffect(Player player)
    {
        base.TakeEffect(player);
        StandardEffect(player);
        eventBus.Invoke(new HealSignal(player.maxHit, player.typeOfPerson));
    }
}
