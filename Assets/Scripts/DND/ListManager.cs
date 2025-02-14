using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text pointsText;
    private List<ValueCell> listCells = new List<ValueCell>();

    public int points = 25;

    private void FixedUpdate()
    {
        if (pointsText != null)
        {
            pointsText.text = points.ToString();
        }
    }

    public void ChangePoint(int newPoint)
    {
        points += newPoint;
    }

    public void EnableButtonsInList(ValueCell cell)
    {
        foreach (var cells in listCells)
        {
            if (cell == cells)
            {
                cell.ShowButtons();
            }
        }
    }

    public void DisableAllButtonsInList()
    {
        if (listCells == null) return;
        foreach (var cells in listCells)
        {
            cells.HideButtons();
        }
    }

    public void AddValueCell(ValueCell cell)
    {
        listCells.Add(cell);
    }
}
