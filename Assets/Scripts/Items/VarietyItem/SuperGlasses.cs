using System.Threading.Tasks;
using UnityEngine;

public class SuperGlasses : ItemInteract
{
    public override Task Interact()
    {
        return base.Interact();
    }

    public override void Effect()
    {
        Camera.main.fieldOfView = 120;
    }

    public override void Release()
    {
        throw new System.NotImplementedException();
    }
}
