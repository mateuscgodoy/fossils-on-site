using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FossilHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fossilText, nextLevelText;
    [SerializeField]
    private GameObject nextLevelPanel;
    [SerializeField]
    private int fossilsFound = 0, amountOfFossilsRequired = 10, playerLevel = 1;

    private AudioSource fossilAudioSource;

    public delegate int OnLevelPass();
    public static OnLevelPass UpdatePlayerGold;

    private void OnEnable()
    {
        EscavationSiteController.OnFossilAcquire += IncreaseFossilCount;
        PlayerAttributes.OnFossilTrouble += HandleFossilTrouble;
    }

    private void OnDisable()
    {
        EscavationSiteController.OnFossilAcquire -= IncreaseFossilCount;
        PlayerAttributes.OnFossilTrouble -= HandleFossilTrouble;
    }

    private void Awake()
    {
        fossilAudioSource = GetComponent<AudioSource>();
        fossilText.text = $"{fossilsFound}/{amountOfFossilsRequired}";
    }

    private void Update()
    {
        if (fossilsFound == amountOfFossilsRequired)
        {
            if(playerLevel == 3)
            {
                SceneManager.LoadScene("VictoryScene");
                Debug.Log("GG!");
            }
            GenerateNextLevel();
        }
    }

    private void IncreaseFossilCount()
    {
        fossilAudioSource.Play();
        fossilsFound++;
        fossilText.text = $"{fossilsFound}/{amountOfFossilsRequired}";
    }

    private string HandleFossilTrouble()
    {
        if(fossilsFound == 0)
        {
            return "You mistook a stone for a fossil!";
        }

        fossilsFound--;
        fossilText.text = $"{fossilsFound}/{amountOfFossilsRequired}";

        return "You lost 1 of your fossils";
    }

    // NEXT LEVEL AMOUNT OF FOSSILS
    private void GenerateNextLevel()
    {
        nextLevelPanel.SetActive(true);     
        playerLevel++;
        amountOfFossilsRequired *= 2;
        fossilsFound = 0;
        var playerMoney = UpdatePlayerGold?.Invoke();
        nextLevelText.text = $"Your next expedition will require {amountOfFossilsRequired} fossils and now you have a total of {playerMoney} gold.";
        fossilText.text = $"{fossilsFound}/{amountOfFossilsRequired}";
    }

    public void DismissNextLevelPanel()
    {
        nextLevelPanel.SetActive(false);
    }
}
