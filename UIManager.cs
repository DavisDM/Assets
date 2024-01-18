using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI hitPointsText;
    public Slider experienceSlider;
    public GameObject levelUpOptionsUI;
    public Button[] upgradeButtons;
    public TextMeshProUGUI nextLevelDescriptionText; // Add this field

    void Start()
    {
        UpdateExperienceUI();
        ShowHitPoints();
    }

    void Update()
    {
        UpdateExperienceUI();
        ShowHitPoints();
    }

    void UpdateExperienceUI()
    {
        if (player.experienceToNextLevel > 0)
        {
            experienceSlider.maxValue = player.experienceToNextLevel;
            experienceSlider.value = player.experience;
        }
    }

    void OnEnable()
    {
        player.onLevelUp += ShowRandomUpgrades;
    }

    void OnDisable()
    {
        player.onLevelUp -= ShowRandomUpgrades;
    }
   void ShowRandomUpgrades()
    {
       List<UpgradeOption> availableUpgrades = player.upgradeOptions
        .Where(upgrade => player.GetUpgradeLevel(upgrade.name) < upgrade.maxLevel)
        .ToList();

         // Shuffle and pick 3 upgrades
        var chosenUpgrades = availableUpgrades.OrderBy(x => UnityEngine.Random.value).Take(3).ToList();

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < chosenUpgrades.Count)
            {
                var upgradeOption = chosenUpgrades[i];
                var button = upgradeButtons[i];
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = $"{upgradeOption.name}";
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SelectUpgrade(upgradeOption));     
                // Add a TextMeshProUGUI component for displaying the description under the button
                var descriptionText = button.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
                descriptionText.text = upgradeOption.GetNextLevelDescription();
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
    }
        levelUpOptionsUI.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    void SelectUpgrade(UpgradeOption upgradeOption)
{
    if (upgradeOption != null && player != null)
    {   
        Debug.Log("Selecting upgrade: " + upgradeOption.name);
        player.ApplyUpgrade(upgradeOption); // Apply the selected upgrade to the player
        levelUpOptionsUI.SetActive(false); // Close the upgrade screen
        Time.timeScale = 1; // Unpause the game (if it was paused)
    }
    else
    {
        Debug.LogError("Player or upgradeOption is null!");
    }
}


    // Rest of your UIManager code

    void ShowHitPoints()
    {
        if (player != null && hitPointsText != null)
        {
            hitPointsText.text = "Hit Points: " + player.hitPoints;
        }
    }
}
