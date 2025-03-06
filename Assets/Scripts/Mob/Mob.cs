using UnityEngine;

public class Mob : MonoBehaviour
{
    private Skin skin;
    private Head head;
    private Body body;
    private Sex sex;

    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform bodyTransform;

    public void SetSkin(Skin skin)
    {
        this.skin = skin;
    }

    public void SetHead(Head head)
    {
        this.head = head;
        this.head.transform.position = headTransform.position;
    }

    public void SetBody(Body body)
    {
        this.body = body;
        this.body.transform.position = bodyTransform.position;
    }

    public void SetSex(Sex sex)
    {
        this.sex = sex;
    }
}

public enum Sex
{
    MALE,
    FEMALE
}
