
public class FullHealthBottle : CommonBottle
{
    public void StandardEffect(DNDManipulator person)
    {
        person.SetHits(person.person.maxHit);
        person.PlusLevelOfStress(person.person.maxHit * 2);
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
