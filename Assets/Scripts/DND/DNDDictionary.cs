using System.Collections.Generic;
using UnityEngine;

public class DNDDictionary : MonoBehaviour, IService
{
    public Dictionary<string, object> spells = new Dictionary<string, object>();
    public Dictionary<int, int[]> levelAndPoints = new Dictionary<int, int[]>();

    private void Awake()
    {
        SetPoints();
    }

    public void AddSpells<T>(string name, T value) where T : class
    {
        if(!spells.ContainsKey(name))
        spells.Add(name, value);
    }

    public void RemoveSpells<T>(string name) where T : class
    {
        if (spells.ContainsKey(name))
        {
            spells.Remove(name);
        }
    }

    public void SetPoints()
    {
        for(int i = 0; i < 20; i++)
        {
            int pointsNew = 300 * Mathf.RoundToInt(1.25f * Mathf.Pow(1.5f, i));
            int pointsPres = 300 * Mathf.RoundToInt(1.25f * Mathf.Pow(i, i - 1));

            int lvl = i+1;
            levelAndPoints.Add(lvl, new int[] {pointsPres, pointsNew});
        }
    }
}

public enum Dices
{
    D4 = 4,
    D6 = 6, 
    D8 = 8,
    D10 = 10,
    D12 = 12,
    D20 = 20
}
