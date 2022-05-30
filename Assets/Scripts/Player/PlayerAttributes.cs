using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField]
    private int studyEnergyCost = 1, moveEnergyCost = 2, excavateEnergyCost = 3, playerMoney = 150, moneyPerDayRatio = 5;
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private int defaultStudyEnergyCost = 1, defaultMoveEnergyCost = 2, defaultExcavateEnergyCost = 3, defaultMoneyPerDayRatio = 5, defaultMoney = 150;

    public delegate void CommunicateTimer(string key, int daysToAdd);
    public static CommunicateTimer OnEventTrigger;

    public delegate int DecreaseTimer();
    public static DecreaseTimer OnEnergyTrouble;

    public delegate string HandleFossilTrouble();
    public static HandleFossilTrouble OnFossilTrouble;

    public delegate void HandleTroubleCounter();
    public static HandleTroubleCounter OnTroubleEnd;

    public int StudyEnergy { get { return studyEnergyCost; } private set { } }
    public int MoveEnergy { get { return moveEnergyCost; } private set { } }
    public int ExcavateEnergy { get { return excavateEnergyCost; } private set { } }

    private void OnEnable()
    {
        ClockController.OnClockUpdate += DayChange;
        ClockController.OnDurationEnd += ResetAttributeState;
        RandomTroubleEvents.ApplyEventEffect += ProcessTroubleEvent;
        FossilHandler.UpdatePlayerGold += GenerateNewLevelGold;
    }

    private void OnDisable()
    {
        ClockController.OnClockUpdate -= DayChange;
        ClockController.OnDurationEnd -= ResetAttributeState;
        RandomTroubleEvents.ApplyEventEffect -= ProcessTroubleEvent;
        FossilHandler.UpdatePlayerGold -= GenerateNewLevelGold;
    }

    private void Awake()
    {
        moneyText.text = $"{playerMoney}";
    }

    private void DayChange()
    {
        ChangePlayerMoney(moneyPerDayRatio);
    }

    private void ChangePlayerMoney(int amount)
    {
        if (playerMoney - amount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
            moneyText.text = "0";
            return;
        }

        playerMoney -= amount;
        moneyText.text = $"{playerMoney}";
    }
    private void ProcessTroubleEvent(string key, TextMeshProUGUI effectText)
    {
        int effectDaysDuration = Random.Range(2, 5);

        switch (key)
        {
            case "MoveEnergy":
                if (moveEnergyCost < 6)
                    moveEnergyCost++;

                OnEventTrigger?.Invoke(key, effectDaysDuration);

                effectText.text = $"Move the camp now costs {MoveEnergy} Energy, for the next {effectDaysDuration} days.";
                break;
            case "StudyEnergy":
                if (studyEnergyCost < 5)
                    studyEnergyCost++;

                OnEventTrigger?.Invoke(key, effectDaysDuration);

                effectText.text = $"Study the surroundings now costs {StudyEnergy} Energy, for the next {effectDaysDuration} days.";
                break;
            case "ExcavateEnergy":
                if (excavateEnergyCost < 7)
                    excavateEnergyCost++;

                OnEventTrigger?.Invoke(key, effectDaysDuration);

                effectText.text = $"Excavate the site now costs {ExcavateEnergy} Energy, for the next {effectDaysDuration} days.";
                break;
            case "FossilAmount":
                effectText.text = OnFossilTrouble?.Invoke();
                break;
            case "DirectEnergy":
                int energyLost = (int)OnEnergyTrouble?.Invoke();

                effectText.text = $"You lost {energyLost} points of energy.";
                break;
            case "MoneyPerDay":
                moneyPerDayRatio += Random.Range(2, 11);

                OnEventTrigger?.Invoke(key, effectDaysDuration);

                effectText.text = $"For the next {effectDaysDuration} days the expedition will consume {moneyPerDayRatio} gold coins.";
                break;
            default:
                break;
        }
    }

    private void ResetAttributeState(string key)
    {
        switch (key)
        {
            case "MoveEnergy":
                moveEnergyCost = defaultMoveEnergyCost;
                break;
            case "StudyEnergy":
                studyEnergyCost = defaultStudyEnergyCost;
                break;
            case "ExcavateEnergy":
                excavateEnergyCost = defaultExcavateEnergyCost;
                break;
            case "MoneyPerDay":
                moneyPerDayRatio = defaultMoneyPerDayRatio;
                break;
            default:
                break;
        }

        OnTroubleEnd?.Invoke();
    }

    private int GenerateNewLevelGold()
    {
        playerMoney += (int)(defaultMoney * 1.75f); // Almost a 2x, not indeed 2x to reward player with higher funds
        moneyText.text = $"{playerMoney}";
        return playerMoney;
    }
}
