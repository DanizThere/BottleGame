
public class FullHealthBottle : CommonBottle
{
    public void StandardEffect(DNDPerson person)
    {
        person.SetHits(person.maxHit);
        person.PlusLevelOfStress(person.maxHit * 2);
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
