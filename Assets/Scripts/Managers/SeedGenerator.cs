using UnityEngine;

public class SeedGenerator
{
    private int Seed;

    public void Generate()
    {
        Seed = GenerateKey();
        HandleSeed(Seed);
    }

    public SeedGenerator()
    {

    }

    public SeedGenerator(int seed)
    {
        Seed = seed;
        HandleSeed(Seed);
    }

    public void HandleSeed(int seed)
    {
        Random.InitState(seed);
    }

    public int ShowSeed()
    {
        return Seed;
    }

    public int GenerateKey()
    {
        string keys = null;
        for (int i = 0; i < 7; i++)
        {
            keys += Random.Range(0, 10);
        }
        return keys.GetHashCode();
    }
}
