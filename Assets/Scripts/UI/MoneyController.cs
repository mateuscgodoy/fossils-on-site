using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    [SerializeField]
    private int playerMoney = 150, dailyPenalty = -5;

    private string moneyEventKey = "MoneyPerDay";
    private TextMeshProUGUI moneyText;

    public delegate void MoneyTroubleEvent(int days);
    public static MoneyTroubleEvent AddEventDuration;

    private void OnEnable()
    {
        ClockController.OnClockUpdate += DayChange;
        RandomTroubleEvents.ApplyEventEffect += MoneyEventLaunched;
    }

    private void OnDisable()
    {
        ClockController.OnClockUpdate -= DayChange;
        RandomTroubleEvents.ApplyEventEffect -= MoneyEventLaunched;
    }

    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
        moneyText.text = $"{playerMoney}";
    }

    private void DayChange()
    {
        ChangePlayerMoney(dailyPenalty);
    }

    private void ChangePlayerMoney(int amount)
    {
        if (playerMoney + amount <= 0)
        {
            // FUTURE GAME OVER SCENE FROM HERE.
            moneyText.text = "0";
            Debug.Log("Game Over.");
            return;
        }

        playerMoney += amount;
        moneyText.text = $"{playerMoney}";
    }

    private void MoneyEventLaunched(string key, TextMeshProUGUI effectText)
    {
        if(key != moneyEventKey)
        {
            return;
        }

        int increaseMoneyPerDay = Random.Range(5, 15);
        dailyPenalty += increaseMoneyPerDay;

        int effectDuration = Random.Range(2, 4);
        AddEventDuration?.Invoke(effectDuration);

        effectText.text = $"Now the expedition consumes {increaseMoneyPerDay} gold per day, for the next {effectDuration} days.";
    }
}
