using System;

public class Turn
{
    private Guid name = Guid.NewGuid();
    public Guid Name
    {
        get => name;
        set => name = value;
    }
}