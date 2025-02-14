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

    private EventBus eventBus;

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
    }

    public void Init(DNDPerson person)
    {
        this.PersonName.text = person.personName;
        this.Savethrows.text = person.savethrowsFromDeath.ToString();  
    }

    public void Interact()
    {
        eventBus.Invoke(new ListSignal());
    }
}
