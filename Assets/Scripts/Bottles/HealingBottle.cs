using System;
using System.Threading.Tasks;

public class HealingBottle : CommonBottle
{
    public int healing = 2;
    public int iterations = 5;

    public override async void TakeEffect(DNDPerson person)
    {
        base.TakeEffect(person);
        eventBus.Invoke(new HealSignal(person.maxHit, person.typeOfPerson));
        for (int i = 0; i < iterations; i++)
        {
            person.TakeHeal(healing);
            person.SetIntervalAfterHeal();
            await Task.Delay(TimeSpan.FromSeconds(1f));
        }
        person.PlusLevelOfStress(healing * iterations);
    }
}
