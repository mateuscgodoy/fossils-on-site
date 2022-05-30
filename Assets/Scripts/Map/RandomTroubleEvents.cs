using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTroubleEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject troublePanelGameObject, otherButtonsOverlay;
    [SerializeField]
    private TextMeshProUGUI troubleText, effectText;
    [SerializeField]
    private int maxNumberOfActiveTroubles = 2;

    private int troubleCounter = 0;

    public delegate void TroubleEvent(string key, TextMeshProUGUI effectText);
    public static TroubleEvent ApplyEventEffect;

    private void OnEnable()
    {
        EscavationSiteController.OnSiteTrouble += GenerateRandomEvent;
        PlayerAttributes.OnTroubleEnd += DecreaseTroubleCounter;
    }

    private void OnDisable()
    {
        EscavationSiteController.OnSiteTrouble -= GenerateRandomEvent;
        PlayerAttributes.OnTroubleEnd += DecreaseTroubleCounter;
    }

    private string[] moveEnergyOptions =
    {
        "You slipped and hurt your leg.",
        "Long hours of work under the sun have applied it's weight over you.",
        "An unexpected storm affected your work."
    };

    private string[] excavateEnergyOptions =
    {
        "You cut your hand leaning on a rough stone.",
        "You lost one of your tools.",
        "An unexpected storm affected your work.",
        "A crow took one of your tools to it's nest.",
        "The soil composition from this site turned out to be difficult to excavate."
    };

    private string[] studyEnergyOptions =
    {
        "Something you ingested didn't do you any good.",
        "Long hours of work under the sun have applied it's weight over you.",
        "The weather made you sick, you got a cold.",
        "The climate conditions are affecting your progress." // Might be a new case where the player can produce an action to mitigate penalty effect.
    };

    private string[] fossilAmountOptions =
    {
        "",
    };

    private string[] directEnergyOptions =
    {
        "You spent the rest of the day watching a wild fox roam around. Totally worth it.",
        "You went on the woods to look for fuel for your campfire and got lost.",
        "Animals prowling your camp at night kept you wake all night.",
        "Long hours of work under the sun have applied it's weight over you."
    };

    private string[] moneyPerDay =
    {
        "The expedition might require more resources than you expected.",
        "Some of you food spoiled, your expedition won't be able to last too long.",
        "While you were away, an wild animal took some of your food supplies."
    };

    private string[] dictKeys = { 
        "MoveEnergy",
        "StudyEnergy",
        "ExcavateEnergy",
        "FossilAmount",
        "DirectEnergy",
        "MoneyPerDay"
    };

    Dictionary<string, string[]> sortEvents = new Dictionary<string, string[]>();

    private void Awake()
    {
        sortEvents.Add("MoveEnergy", moveEnergyOptions);
        sortEvents.Add("StudyEnergy", studyEnergyOptions);
        sortEvents.Add("ExcavateEnergy", excavateEnergyOptions);
        sortEvents.Add("FossilAmount", fossilAmountOptions);
        sortEvents.Add("DirectEnergy", directEnergyOptions);
        sortEvents.Add("MoneyPerDay", moneyPerDay);
    }

    private void GenerateRandomEvent()
    {
        if (troubleCounter > maxNumberOfActiveTroubles)
            return;

        otherButtonsOverlay.SetActive(false);

        // Chose a random key from they key array.
        int randomKeyEvent = Random.Range(0, dictKeys.Length);
        string eventKey = dictKeys[randomKeyEvent];

        if (eventKey != "FossilAmount" && eventKey != "DirectEnergy")
            troubleCounter++;

        // Retrieve they string[] value of that key from the Dict
        string[] dictValue = sortEvents[eventKey];
     
        // Chose a random string from the array of possibilities
        randomKeyEvent = Random.Range(0, dictValue.Length);
        string eventText = dictValue[randomKeyEvent];

        troubleText.text = eventText;
        troublePanelGameObject.SetActive(true);
        
        ApplyEventEffect?.Invoke(eventKey, effectText);
    }

    private void DecreaseTroubleCounter()
    {
        troubleCounter--;
    }

    public void DismissTroublePanel()
    {
        otherButtonsOverlay.SetActive(true);
        troublePanelGameObject.SetActive(false);
    }
}
