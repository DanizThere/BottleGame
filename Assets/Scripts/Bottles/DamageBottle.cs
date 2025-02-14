using System;
using System.Threading.Tasks;

public class DamageBottle : CommonBottle
{
    public int damage = 2;
    public int iterations = 5;

    public override async void TakeEffect(DNDPerson person)
    {
        base.TakeEffect(person);
        for (int i = 0; i < iterations; i++)
        {
            person.TakeDamage(damage);
            eventBus.Invoke(new DamageSignal(person.maxHit, person.typeOfPerson));
            await Task.Delay(TimeSpan.FromSeconds(1f));
        }
        person.CheckHP();
        person.MinusLevelOfStress(damage * iterations);
    }
}
