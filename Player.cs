using UnityEngine;
using TMPro; // TextMeshPro namespace
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;


public class Player : MonoBehaviour
{
    public float hitPoints;
    public GameObject projectilePrefab;
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();
    public float shootRate = 1.0f;
    public GameObject slimeArmPrefab; // Assign this in the Unity Editor
    public GameObject shotgunProjectilePrefab; // Assign in Unity Editor
    public int slimeArms = 0;
    public int shotgunShots = 20;
    public float shotgunInterval = 2.0f; // Time between shotgun blasts
    private float shotgunTimer = 0;
    private bool hasShotgunUpgrade = false;
    private bool hasShieldUpgrade = false;
    public float shieldHealth = 50f;    // Total health of the shield
    public float shieldRespawnTime = 5f; // Time to respawn the shield after it breaks
    public GameObject shieldAura;
    private float currentShieldHealth;
    private bool shieldActive = false;
    private float shieldTimer = 0f; 
    public GameObject bigShotProjectilePrefab;  // Assign in Unity Editor
    public float bigShotInterval = 5.0f; // Time between big shot fires
    private bool hasBigShotUpgrade = false;
    private float bigShotTimer = 0;
    private float bigShotSpeed = 0.25f; // Base speed of Big Shot
    private float bigShotSize = 0.4f; // Base size of Big Shot
    private int bigShotCount = 0;
    private float shootTimer = 0;
    private CircleCollider2D playerCollider;
    private Camera mainCamera;
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;
    public delegate void OnLevelUp();
    public event OnLevelUp onLevelUp;
    public TextMeshProUGUI hitPointsText;
    public List<UpgradeOption> upgradeOptions = new List<UpgradeOption>();

    void Start()
    {
        mainCamera = Camera.main;
        playerCollider = GetComponent<CircleCollider2D>();

        // Initialize the dictionary
    

    // Populate the dictionary with the upgrades and their initial levels
    // Replace "Upgrade1", "Upgrade2", etc. with the actual names of your upgrades
    upgradeOptions.Add(new UpgradeOption("Slime Shield", EnableSlimeShield, new string[] { "Basic Slime Shield", "More health.", "Even more health." }));
    upgradeOptions.Add(new UpgradeOption("Shotgun", EnableShotgunUpgrade, new string[] { "Shotgun.", "More bullets.", "Even more bullets." }));
    upgradeOptions.Add(new UpgradeOption("Big Shot", EnableBigShotUpgrade, new string[] { "Big shot.", "More big shots.", "Even more big shots." }));
    upgradeOptions.Add(new UpgradeOption("Slime Arm", EnableSlimeArm, new string[] { "Slime arm.", "One more.", "More arms." }));
    // Add more upgrades as needed
    }
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootRate)
        {
            ShootProjectile();
            shootTimer = 0;
        }
        if (hasShotgunUpgrade)
    {
        shotgunTimer += Time.deltaTime;
        if (shotgunTimer >= shotgunInterval)
        {
            ShootShotgun();
            shotgunTimer = 0;
        }
    }
          if (!shieldActive & hasShieldUpgrade)
    {
        shieldTimer += Time.deltaTime;
        if (shieldTimer >= shieldRespawnTime)
        {
            shieldActive = true;
            currentShieldHealth = shieldHealth;
            shieldAura.SetActive(true);
        }
    }
        if (hasBigShotUpgrade)
        {
            bigShotTimer += Time.deltaTime;
            if (bigShotTimer >= bigShotInterval)
            {
                ShootBigShot();
                bigShotTimer = 0f;
            }
        }
    }
    private void ConfigureShotgun(int shotCount)
    {
        shotgunShots = shotCount;
    }

        public void EnableShotgunUpgrade(Player player)
    {
        hasShotgunUpgrade = true;
        Debug.Log("has shotgun");
        switch (GetUpgradeLevel("Shotgun"))
        {
            case 1:
                ConfigureShotgun(shotgunShots);
                Debug.Log("shotgun lvl 1");
                break;
            case 2:
                ConfigureShotgun(shotgunShots*2); 
                Debug.Log("shotgun lvl 2");
                break;
            case 3:
                ConfigureShotgun(shotgunShots*3); 
                Debug.Log("shotgun lvl 3");

                break;
            default:
                break;
        }
    }
    void ShootShotgun()
    {
    for (int i = 0; i < shotgunShots; i++)
        {
            Vector2 shootDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
            Instantiate(shotgunProjectilePrefab, transform.position, rotation);
        }
    }
 
    public void EnableBigShotUpgrade(Player player)
{
    Debug.Log("EnableBigShotUpgrade called"); // This will print to the console when the method is called

    hasBigShotUpgrade = true;

    int bigShotLevel = GetUpgradeLevel("Big Shot");
    Debug.Log("BigShot level: " + bigShotLevel); // This will print the level of the BigShot upgrade

    // Rest of the method...
    switch (bigShotLevel)
    {
        case 1:
            ConfigureBigShot(bigShotLevel, bigShotSpeed, 1f);
            break;
        case 2:
            ConfigureBigShot(bigShotLevel, 2f, 1.5f); // Slightly faster and bigger
            break;
        case 3:
            ConfigureBigShot(bigShotLevel, 3f, 2f); // Even faster and bigger
            break;
        default:
            break;
    }
}
private void ConfigureBigShot(int shotCount, float speed, float size)
{


  // Assign this in the inspector

    bigShotProjectilePrefab.GetComponent<BigShotProjectile>().speed = speed;
    bigShotProjectilePrefab.GetComponent<BigShotProjectile>().size = size;
    bigShotCount = shotCount;
}

void ShootBigShot()
{
    Debug.Log(bigShotCount);
    for (int i = 0; i < bigShotCount; i++)
    {
        Vector2 shootDirection = UnityEngine.Random.insideUnitCircle.normalized;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
        Instantiate(bigShotProjectilePrefab, transform.position, rotation);
    }
}

 private void ConfigureSlimeShield(float health)
    {
        shieldHealth = health;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamageFromEnemy(other);
        }
    }
    void TakeDamageFromEnemy(Collider2D enemyCollider)
    {
        // Assuming the enemy script has a method to get the damage value
        float damage = enemyCollider.GetComponent<Enemy>().GetDamageValue();
        TakeDamage(damage * Time.deltaTime); // Apply damage over time
    }

     private void ConfigureSlimeArm(int numberOfArms)
    {
        slimeArms = numberOfArms;
    }
    public void EnableSlimeArm(Player player)
    {
          switch (GetUpgradeLevel("SlimeArm"))
        {
            case 1:
                createSlimeArm(1);
                break;
            case 2:
                createSlimeArm(2);
                break;
            case 3:
                createSlimeArm(3);
                break;
            default:
                break;
        }
    }

    public void createSlimeArm(int slimeArms)
    {
         // Destroy existing slime arms
        foreach (Transform child in transform)
        {
            if (child.CompareTag("SlimeArm")) // Assuming you've set a tag for slime arms
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < slimeArms; i++){
            GameObject slimeArm = Instantiate(slimeArmPrefab, transform.position, Quaternion.identity);
            slimeArm.transform.SetParent(this.transform, false);
            float playerRadius = GetComponent<CircleCollider2D>().radius;
            float armLength = slimeArm.GetComponent<SpriteRenderer>().bounds.size.y / 2; // Adjust based on your setup
            slimeArm.transform.localPosition = new Vector3(0, playerRadius + armLength, 0);
            slimeArm.transform.localRotation = Quaternion.identity;
        }
    }
    public void EnableSlimeShield(Player player)
    {
        hasShieldUpgrade = true;
        switch (GetUpgradeLevel("Slime Shield"))
        {
            case 1:
                ConfigureSlimeShield(shieldHealth);
                break;
            case 2:
                ConfigureSlimeShield(shieldHealth * 1.5f); 
                break;
            case 3:
                ConfigureSlimeShield(shieldHealth * 2.0f); 
                break;
            default:
                break;
        }
        shieldActive = true;
        currentShieldHealth = shieldHealth;
        shieldAura.SetActive(true);
    }
    void ShootProjectile()
    {
        Vector2 shootDirection = Vector2.up; // This ensures the projectile shoots upwards
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootDirection);
        Vector3 spawnPosition = transform.position + new Vector3(shootDirection.x, shootDirection.y, 0) * 1.0f; // Adjust the multiplier if needed for spawn position
        Instantiate(projectilePrefab, spawnPosition, rotation);
    }
   public void TakeDamage(float damage)
    {
        if (shieldActive)
        {
            currentShieldHealth -= damage;
        if (currentShieldHealth <= 0)
        {
            shieldActive = false;
            shieldTimer = 0f;
            shieldAura.SetActive(false);
        }
        }
        else
        {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
        }
}
    public void LevelUp()
    {
        level++;
        Debug.Log("levelup");
        experience -= experienceToNextLevel;
        experienceToNextLevel = CalculateNextLevelExperience(experienceToNextLevel);
        Debug.Log($"Level Up: Level {level}, Experience {experience}, Next Level {experienceToNextLevel}");
        if (onLevelUp != null)
        {
            onLevelUp.Invoke();
            Debug.Log("onLevelUp event invoked.");
        }
        Time.timeScale = 0;
    }
private void IncreaseUpgradeLevel(string upgradeName)
{
    if (upgradeLevels.ContainsKey(upgradeName))
    {
        int currentLevel = upgradeLevels[upgradeName];
        int maxLevel = GetMaxUpgradeLevel(upgradeName);

        if (currentLevel < maxLevel)
        {
            upgradeLevels[upgradeName] = currentLevel + 1;
        }
    }
}
    int GetMaxUpgradeLevel(string upgradeName)
    {
    // Implement logic to get the max level of the upgrade based on your game's design
    // For now, let's assume it's 10 for all upgrades
        return 3;
    }
    public int GetUpgradeLevel(string upgradeName)
{
    // Retrieve the current level of the specified upgrade
    // Assuming this is stored in a list or similar structure
    UpgradeOption upgradeOption = upgradeOptions.Find(u => u.name == upgradeName);
    if (upgradeOption != null)
    {
        return upgradeOption.level;
    }
    return 0; // Default level if the upgrade hasn't been acquired
}
public void ApplyUpgrade(UpgradeOption upgradeOption)
{
    Debug.Log($"Applying upgrade: {upgradeOption?.name}");
    if (upgradeOption == null)
    {
        Debug.Log("Upgrade not applied because upgradeOption is null.");
        return;
    }
    if (upgradeOption.level >= upgradeOption.maxLevel)
    {
        Debug.Log($"Upgrade not applied because the upgrade level is not less than the max level.");
        return;
    }
    Debug.Log($"Before applying upgrade: {upgradeOption.level}");
    // Increase the level before applying the upgrade
    upgradeOption.level++;
    // Now apply the upgrade
    upgradeOption.ApplyUpgrade(this);
    Debug.Log($"After applying upgrade: {upgradeOption.level}");
}

   public List<UpgradeOption> GetUpgradeOptions()
{
    return upgradeOptions;
}
     public void GainExperience(int amount)
        {
            experience += amount;
            Debug.Log(experience);
            if (experience >= experienceToNextLevel)
            {
                LevelUp();
    }
        }
    int CalculateNextLevelExperience(int currentExp)
    {
        // Example calculation, adjust as needed
        return Mathf.FloorToInt(currentExp * 1.5f);
    }

   private void Die()
    {
        // Load the Game Over scene
        SceneManager.LoadScene("GameOverScene"); // Replace "GameOverScene" with your actual scene name
    }
}