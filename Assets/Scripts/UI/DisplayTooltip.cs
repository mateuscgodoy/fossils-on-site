using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject tooltipPanel;
    [SerializeField]
    private TextMeshProUGUI tooltipText;

    internal void SetUIElement(string key, int daysDuration)
    {
        switch (key)
        {
            case "MoveEnergy":
                tooltipText.text = $"Movement penalty. Duration: {daysDuration} days.";
                break;
            case "StudyEnergy":
                tooltipText.text = $"Site study penalty. Duration: {daysDuration} days.";
                break;
            case "ExcavateEnergy":
                tooltipText.text = $"Excavation penalty. Duration: {daysDuration} days.";
                break;
            case "MoneyPerDay":
                tooltipText.text = $"Money per day penalty. Duration: {daysDuration} days.";
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}
