using UnityEngine;
using UnityEngine.UI;

public class ValueCell : ListCell
{
    [SerializeField] private Button buttonIncrease;
    [SerializeField] private Button buttonDecrease;
    private int value = 8;
    private TMPro.TMP_Text values;

    private ListManager listManager;
    [SerializeField] private string Characteristic;

    private void Awake()
    {
        values = GetComponentInChildren<TMPro.TMP_Text>();

        values.text = value.ToString();

        Description = desc;
        NameOfInteract = nameOf;
    }

    private void Start()
    {
        buttonIncrease.onClick.AddListener(IncreaseValues);
        buttonDecrease.onClick.AddListener(DecreaseValues);
        player = FindAnyObjectByType<Player>();

        listManager = ServiceLocator.Instance.Get<UIManager>().listManager;
        listManager.AddValueCell(this);
        HideButtons();
    }

    public override void SelectCell()
    {
        base.SelectCell();
        listManager.EnableButtonsInList(this);
    }

    public override void Interact()
    {
        SelectCell();
    }

    public void IncreaseValues()
    {
        if (listManager.points < 1 || value > 19) return;
        ++value;
        listManager.ChangePoint(-1);
        values.text = value.ToString();
    }

    public void DecreaseValues()
    {
        if (value < 2) return;
        listManager.ChangePoint(1);
        --value;
        values.text = value.ToString();
    }

    public void HideButtons()
    {
        buttonDecrease.gameObject.SetActive(false);
        buttonIncrease.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        buttonDecrease.gameObject.SetActive(true);
        buttonIncrease.gameObject.SetActive(true);
    }
}
