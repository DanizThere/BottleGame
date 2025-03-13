using System;
using UnityEngine;

public class CharacterList : MonoBehaviour, IInteractable
{
    [SerializeField] private TMPro.TMP_Text PersonName;
    [SerializeField] private TMPro.TMP_Text Savethrows;
    #region stats
    [Header("Charasteristics")]
    [SerializeField] private TMPro.TMP_Text characteristicStrength;
    [SerializeField] private TMPro.TMP_Text characteristicDexterity;
    [SerializeField] private TMPro.TMP_Text characteristicConstitution;
    [SerializeField] private TMPro.TMP_Text characteristicIntelligence;
    [SerializeField] private TMPro.TMP_Text characteristicWisdom;
    [SerializeField] private TMPro.TMP_Text characteristicCharisma;
    #endregion


    public void Init(DNDPerson person)
    {
        Debug.Log("Good luck");
    }

    public void Interact()
    {
    }
}
