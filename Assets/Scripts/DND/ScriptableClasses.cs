using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="New Class")]
public class ScriptableClasses : ScriptableObject
{
    public string className;
    public Dices diceOfHits;
    public List<DNDFeature> features;
}
