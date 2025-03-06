using UnityEngine;

public class MobBuilder
{
    private Mob mob;
    private Skin skin;
    private Head head;
    private Body body;
    private Sex sex;

    public MobBuilder WithRootPrefab(Mob mob)
    {
        this.mob = mob;

        return this;
    }

    public MobBuilder Skin(Skin skin)
    {
        this.skin = skin;

        return this;
    }

    public MobBuilder Head(Head head)
    {
        this.head = head;

        return this;
    }

    public MobBuilder Body(Body body)
    {
        this.body = body;

        return this;
    }

    public MobBuilder Sex(Sex sex = global::Sex.MALE)
    {
        this.sex = sex;

        return this;
    }

    public Mob Build(Transform container = null)
    {
        var mob = Object.Instantiate(this.mob, container);
        var createdSkin = Object.Instantiate(skin);
        var createdHead = Object.Instantiate(head);
        var createdBody = Object.Instantiate(body);

        mob.SetBody(createdBody);
        mob.SetHead(createdHead);
        mob.SetSkin(createdSkin);
        mob.SetSex(sex);

        return mob;
    }

    public MobBuilder Reset()
    {
        mob = null;
        head = null;
        body = null;
        skin = null;
        return this;
    }
}
