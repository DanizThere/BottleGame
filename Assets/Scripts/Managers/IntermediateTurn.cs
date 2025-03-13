using System.Collections;
using UnityEngine;

public class IntermediateTurn : Turn
{
    public async Awaitable CheckBottles()
    {
        bool exists = ServiceLocator.Instance.Get<BottlesManager>().CheckBottles();

        if (!exists)
        {
            await Awaitable.WaitForSecondsAsync(1);

            Debug.Log("I refill");
        }

    }
}
