using UnityEngine;

[CreateAssetMenu(fileName = "Create new sentence", menuName = "Dialogue/Sentence")]
public class Sentence : ScriptableObject
{
    public string id;
    public AudioClip voice;
    public string author;
    public string text;
}
