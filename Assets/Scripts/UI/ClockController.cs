using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ClockController : MonoBehaviour
{
    [SerializeField]
    private int playerEnergy = 8;

    private int defaultPlayerEnergy;
    private List<int> listOfEffectDayDurations = new List<int>();
    private Dictionary<string, int> troubleEventsDuration = new Dictionary<string, int>();
    private AudioSource clockAS;

    public delegate void ClockManager();
    public static ClockManager OnClockUpdate;

    public delegate void EffectDurationManager(string key);
    public static EffectDurationManager OnDurationEnd;

    public delegate void UIDisplayManager(Dictionary<string, int> troubleEffectsDict);
    public static UIDisplayManager OnNewTroubleAddition;

    public int PlayerEnergy { get { return playerEnergy; } private set { } }

    private void OnEnable()
    {
        EscavationSiteController.OnEscavateAction += UpdateClock;
        StudySiteController.OnSiteStudy += UpdateClock;
        PlayerMovementOption.AfterPlayerMove += UpdateClock;
        PlayerAttributes.OnEventTrigger += AddEffectDuration;
        PlayerAttributes.OnEnergyTrouble += DecreaseRandomEnergyAmount;
    }

    private void OnDisable()
    {
        EscavationSiteController.OnEscavateAction -= UpdateClock;
        StudySiteController.OnSiteStudy -= UpdateClock;
        PlayerMovementOption.AfterPlayerMove -= UpdateClock;
        PlayerAttributes.OnEventTrigger -= AddEffectDuration;
        PlayerAttributes.OnEnergyTrouble -= DecreaseRandomEnergyAmount;
    }

    private void Awake()
    {
        clockAS = GetComponent<AudioSource>();
        defaultPlayerEnergy = playerEnergy;
    }

    private void Update()
    {
        if (playerEnergy <= 0)
        {
            clockAS.Play();
            GenerateNewDay();
        }
    }

    private void GenerateNewDay()
    {
        OnClockUpdate?.Invoke();
        UpdateClock(defaultPlayerEnergy);
        UpdateDayInterval();
        OnNewTroubleAddition?.Invoke(troubleEventsDuration);
    }

    private bool UpdateClock(int amount)
    {
        if (playerEnergy + amount >= 0)
        {
            playerEnergy += amount;
            return true;
        }
        return false;
    }

    private void UpdateDayInterval()
    {
        foreach (var item in troubleEventsDuration)
        {
            Debug.Log($"Key:{item.Key}, Value: {item.Value}");
        }

        var dictKeys = troubleEventsDuration.Keys;
        List<string> listOfKeys = new();

        foreach (var item in dictKeys)
        {
            listOfKeys.Add(item);
        }

        for (int i = 0; i < listOfKeys.Count; i++)
        {
            troubleEventsDuration[listOfKeys[i]]--;

            if (troubleEventsDuration[listOfKeys[i]] == 0)
            {
                troubleEventsDuration.Remove(listOfKeys[i]);
                OnDurationEnd?.Invoke(listOfKeys[i]);
            }
        }
    }

    private void AddEffectDuration(string key, int daysDuration)
    {
        if (troubleEventsDuration.ContainsKey(key) == true)
        {
            troubleEventsDuration.Remove(key);
        }
        
        troubleEventsDuration.Add(key, daysDuration);
        OnNewTroubleAddition?.Invoke(troubleEventsDuration);
    }

    private int DecreaseRandomEnergyAmount()
    {
        int randomAmount = playerEnergy > 1 ? Random.Range(2, 8 - playerEnergy) : 1;
        UpdateClock(-randomAmount);
        return randomAmount;
    }

    public void SkipDayAction()
    {
        playerEnergy = 0;
        return;
    }
}
