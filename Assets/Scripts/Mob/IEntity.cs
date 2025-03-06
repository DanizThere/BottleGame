using UnityEngine;

public interface IEntity
{
    public void SetMob(Mob mob);
    public void SetBackpack(Backpack backpack);
    public void HandleAgree();
    public void ResetAgree();

}
