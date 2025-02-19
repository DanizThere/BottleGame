using UnityEngine;

public class PermamentDeathBottle : CommonBottle
{
    public void StandardEffect(DNDPerson person)
    {
        person.SetHits(0);
        person.CheckHP();
        person.MinusLevelOfStress(50);
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
        eventBus.Invoke(new DamageSignal(player.maxHit));
    }
}
