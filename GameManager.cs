using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    // Resources.
    public int weaponPrice;
    public int expLevelUp;
    public int familiar_upgrade_cost_scaling;

    // References
    public Player player;
    public Weapon weapon;
    public Familiar familiar;
    public SummonDataManager summons;
    public SpellDataManager spells;
    public VillageDataManager villages;
    public EquipmentDataManager all_equipment;
    // UI Stuff
    public FloatingTextManager floatingTextManager;
    public FixedTextManager fixedTextManager;
    public HUD hud;
    public RectTransform healthBar;
    public Text healthText;
    public RectTransform manaBar;
    public Text manaText;

    // Logic.
    public int coins;
    public int mana_crystals;
    public int experience;
    public int current_day;
    public int danger_level;
    public int current_depth;
    public int current_max_depth;

    // Floating text.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Fixed Textbox.
    public void ShowFixedText(string speaker, string speakers_words)
    {
        fixedTextManager.ShowText(speaker, speakers_words);
    }

    // Determine prices.
    public int DeterminePrice(string thing)
    {
        int level = 1;
        int price = 0;
        int cost = 0;
        if (thing == "weapon")
        {
            level = weapon.weaponLevel + 1;
            price = weaponPrice;
        }
        else
        {
            // pass.
        }
        cost = price * level * level;
        return cost;
    }

    // Upgrade Weapon.
    public bool TryUpgradeWeapon()
    {
        int cost = DeterminePrice("weapon");
        if (coins >= cost)
        {
            coins -= cost;
            weapon.UpgradeWeapon();
            return true;
        }

        else
        {
            return false;
        }
    }

    public void EatMana()
    {
        Debug.Log("Eating Mana");
        if (mana_crystals > 0)
        {
            mana_crystals--;
            player.EatMana();
            OnManaChange();
            GrantExp(1);
        }
    }

    public void FeedFamiliarMana()
    {
        Debug.Log("Feeding Mana");
        if (mana_crystals > 0)
        {
            mana_crystals--;
            familiar.GainExp(1);
        }
    }

    public bool UpgradeFamiliarStats(string upgraded_stat)
    {
        int cost = UpgradeFamiliarCost(upgraded_stat);
        if (mana_crystals >= cost)
        {
            mana_crystals -= cost;
            if (upgraded_stat == "bonus_rotate_speed")
            {
                familiar.bonus_rotate_speed++;
                return true;
            }
            else if (upgraded_stat == "heal_threshold_increase")
            {
                familiar.heal_threshold_increase++;
                return true;
            }
            else if (upgraded_stat == "bonus_damage")
            {
                familiar.bonus_damage++;
                return true;
            }
            else if (upgraded_stat == "bonus_push_force")
            {
                familiar.bonus_push_force++;
                return true;
            }
            else if (upgraded_stat == "bonus_heal")
            {
                familiar.bonus_heal++;
                return true;
            }
            else
            {
                mana_crystals += cost;
                return false;
            }
        }
        return false;
    }
    public int UpgradeFamiliarCost(string upgraded_stat)
    {
        int cost = 0;
        if (upgraded_stat == "bonus_rotate_speed")
        {
            cost = familiar.bonus_rotate_speed * familiar.bonus_rotate_speed;
        }
        else if  (upgraded_stat == "heal_threshold_increase")
        {
            cost = familiar.heal_threshold_increase * familiar.heal_threshold_increase;
        }
        else if  (upgraded_stat == "bonus_damage")
        {
            cost = familiar.bonus_damage * familiar.bonus_damage;
        }
        else if  (upgraded_stat == "bonus_push_force")
        {
            cost = familiar.bonus_push_force * familiar.bonus_push_force;
        }
        else if  (upgraded_stat == "bonus_heal")
        {
            cost = familiar.bonus_heal * familiar.bonus_heal;
        }
        return cost;
    }
    public int GetExptoLevel()
    {
        int exp = 0;
        int level = player.playerLevel + 1;
        exp = level * level;
        exp += Random.Range(-level, level);

        return exp;
    }

    public void GrantExp(int exp)
    {
        experience += exp;
        ShowText("+" + exp + "exp", 20, Color.cyan, player.transform.position, Vector3.up*40, 1.0f);
        int exp_to_level = GetExptoLevel();
        if(experience >= exp_to_level)
        {
            experience -= exp_to_level;
            PlayerLevelUp();
            ShowText("Leveled Up!", 30, Color.green, player.transform.position, Vector3.up*33, 2.0f);
        }
    }

    public void GrantCoins(int money)
    {
        int gained_gold = money;
        gained_gold += Random.Range(0, current_depth*2);
        villages.collected_gold += gained_gold;
        ShowText("+ "+gained_gold+" coins", 20, Color.yellow, player.transform.position, Vector3.up*25, 1.0f);
    }

    public void GrantMana(int crystals)
    {
        int gained_mana = crystals;
        gained_mana += Random.Range(0, current_depth);
        villages.collected_mana += gained_mana;
        ShowText("+ "+gained_mana+" crystals", 25, Color.blue, player.transform.position, Vector3.up*25, 1.0f);
    }
    public void PlayerLevelUp()
    {
        player.LevelUp();
    }

    public void PlayerDefeated()
    {
        coins -= player.max_health + player.max_mana;
        player.health = 1;
        player.current_mana = 0;
        OnHealthChange();
        OnManaChange();
        if (coins < 0)
        {
            coins = 0;
        }
        experience -= player.max_health;
        if (experience < 0)
        {
            experience = 0;
        }
        danger_level -= player.playerLevel;
        if (danger_level < 0)
        {
            danger_level = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        NewWeek();
        SaveState();
        ShowFixedText("Spirit Guardian Blaty", "You were defeated but I dragged you back, a few weeks regenerating in your coffin and you're as good as new.");
    }

    public void OnHealthChange()
    {
        float ratio  = (float)player.health / (float)player.max_health;
        healthBar.localScale = new Vector3(ratio, 1, 1);
        healthText.text = player.health.ToString()+" / "+player.max_health.ToString();
    }

    public void OnManaChange()
    {
        float ratio = (float)player.current_mana / (float)player.max_mana;
        manaBar.localScale = new Vector3(ratio, 1, 1);
        manaText.text = player.current_mana.ToString()+" / "+player.max_mana.ToString();
    }

    // Saving and loading.
    public void SaveState()
    {
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        SaveDataWrapper save_data = new SaveDataWrapper();
        save_data.UpdateData();
        string save_json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText("Assets/Saves/save_data.json", save_json);
        FamiliarStatsWrapper familiar_stats = new FamiliarStatsWrapper();
        familiar_stats.UpdateData();
        string familiar_stats_json = JsonUtility.ToJson(familiar_stats, true);
        File.WriteAllText("Assets/Saves/familiar_stats.json", familiar_stats_json);
        Debug.Log("Saved");
        summons.SaveData();
        spells.SaveData();
        all_equipment.SaveData();
        villages.SaveData();
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            player.SetLevel(loaded_data.player_level);
            player.SetHealth(loaded_data.player_health);
            player.SetMana(loaded_data.player_mana);
            weapon.SetLevel(loaded_data.weapon_level);
            coins = loaded_data.coins;
            mana_crystals = loaded_data.mana_crystals;
            experience = loaded_data.experience;
            current_day = loaded_data.current_day;
            string familiar_stats = File.ReadAllText("Assets/Saves/familiar_stats.json");
            FamiliarStatsWrapper loaded_familiar_stats = JsonUtility.FromJson<FamiliarStatsWrapper>(familiar_stats);
            familiar.SetStats(loaded_familiar_stats);
        }
        else
        {
            Debug.Log("Load failed");
        }
        summons.LoadData();
        spells.LoadData();
        all_equipment.LoadData();
        villages.LoadData();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    public void NewDay()
    {
        current_day++;
    }

    public void NewWeek()
    {
        current_day += 7;
    }

    public void ResetDepth()
    {
        current_depth = 0;
        current_max_depth = 0;
    }

    public void AdjustDepth(int depth_change)
    {
        current_depth += depth_change;
        // If you backtrack, maybe you reached a new max depth.
        if (depth_change < 0)
        {
            int potential_max_depth = current_depth - depth_change;
            if (potential_max_depth > current_max_depth)
            {
                current_max_depth = potential_max_depth;
            }
        }
    }
}
