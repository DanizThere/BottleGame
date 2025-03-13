using UnityEngine;

public class View : UIScreen
{

    public override void OnDisable()
    {
        Menu.SetActive(false);
    }

    public override void OnEnable()
    {
        Menu.SetActive(true);
    }

}
