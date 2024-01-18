using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class UpgradeOption
{
     public string name;
    public Action<Player> applyUpgrade;
    public int level;

    private Player player;

    public int maxLevel;

     public string[] descriptions = new string[3];

   public UpgradeOption(string name, Action<Player> applyUpgrade, string[] levelDescriptions)
    {
        this.name = name;
        this.applyUpgrade = applyUpgrade;
        this.level = 0;
        this.maxLevel = 3;
        this.descriptions = levelDescriptions;
        this.player = GameObject.Find("Player").GetComponent<Player>();
    }
     public virtual void ApplyUpgrade(Player player)
    {
        applyUpgrade(player);
    }
 public string GetNextLevelDescription()
{
    if (level < descriptions.Length)
    {
        return descriptions[level];
    }
    else
    {
        // Return a default description if the level is not less than the length of the descriptions array
        return "Max level reached";
    }
}
}
