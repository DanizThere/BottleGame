using UnityEngine;

public class DamageSignal : ISignal
{
    public int HP;
    public DamageSignal(int hp)
    {
        HP = hp;
    }
}
