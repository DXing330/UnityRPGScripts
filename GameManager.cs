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
        OnHealthChange();
    }

    // Resources.
    public List<Sprite> weaponSprites;
    public int weaponPrice;
    public int familiarPrice;
    public int expLevelUp;

    // References
    public Player player;
    public Weapon weapon;
    public Familiar familiar;
    public FloatingTextManager floatingTextManager;
    public RectTransform healthBar;
    public Text healthText;

    // Logic.
    public int coins;
    public int mana_crystals;
    public int experience;
    public int stat_points;

    // Floating text.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
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

    // Upgrade Familiar.
    public bool TryUpgradeFamiliar()
    {
        return false;
    }

    public bool UpgradePlayerStats(string upgraded_stat)
    {
        if (stat_points > 0)
        {
            if (upgraded_stat == "bonus_health")
            {
                player.bonus_health++;
            }
            else if (upgraded_stat == "damage_multiplier")
            {
                player.damage_multiplier++;
            }
            else if (upgraded_stat == "damage_reduction")
            {
                player.damage_reduction++;
            }
            else if (upgraded_stat == "luck")
            {
                player.luck++;
            }
            else
            {
                stat_points++;
            }
            stat_points--;
            return true;
        }
        return false;
    }

    public bool UpgradeFamiliarStats(string upgraded_stat)
    {
        if (upgraded_stat == "bonus_rotate_speed" && mana_crystals > familiar.bonus_rotate_speed)
        {
            familiar.bonus_rotate_speed++;
            mana_crystals -= familiar.bonus_rotate_speed;
            return true;
        }
        else if (upgraded_stat == "heal_threshold_increase" && mana_crystals > familiar.heal_threshold_increase)
        {
            familiar.heal_threshold_increase++;
            mana_crystals -= familiar.heal_threshold_increase;
            return true;
        }
        else if (upgraded_stat == "bonus_damage" && mana_crystals > familiar.bonus_damage)
        {
            familiar.bonus_damage++;
            mana_crystals -= familiar.bonus_damage;
            return true;
        }
        else if (upgraded_stat == "bonus_push_force" && mana_crystals > familiar.bonus_push_force)
        {
            familiar.bonus_push_force++;
            mana_crystals -= familiar.bonus_push_force;
            return true;
        }
        else if (upgraded_stat == "bonus_heal" && mana_crystals > familiar.bonus_heal)
        {
            familiar.bonus_heal++;
            mana_crystals -= familiar.bonus_heal;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetExptoLevel()
    {
        int level = player.playerLevel + 1;
        int exp = expLevelUp * level * level;

        return exp;
    }

    public float GetLuckBonus()
    {
        float luck_bonus = 1.0f;
        if (player.luck <= 100)
        {
            float random_luck = Random.Range(0, player.luck);
            float random_luck_percentage = random_luck/100;
            luck_bonus += random_luck_percentage;
        }
        else
        {
            float luck_percentage = player.luck/(100 + player.luck);
            luck_bonus += luck_percentage;
        }
        return luck_bonus;
    }

    public void GrantExp(int exp)
    {
        float luck_bonus = GetLuckBonus();
        int added_exp = Mathf.RoundToInt(exp * (luck_bonus));
        experience +=added_exp;
        ShowText("+" + added_exp + "exp", 20, Color.cyan, player.transform.position, Vector3.up*40, 1.0f);
        if(experience >= GetExptoLevel())
        {
            experience -= GetExptoLevel();
            PlayerLevelUp();
            ShowText("Leveled Up!", 30, Color.green, player.transform.position, Vector3.up*33, 2.0f);
        }
    }

    public void GrantCoins(int money)
    {
        float luck_bonus = GetLuckBonus();
        int added_coins = Mathf.RoundToInt(money * luck_bonus);
        coins += added_coins;
        ShowText("+ "+added_coins+" coins", 20, Color.yellow, player.transform.position, Vector3.up*25, 1.0f);
    }

    public void GrantMana(int crystals)
    {
        float luck_bonus = GetLuckBonus();
        int added_mana = Mathf.RoundToInt(crystals * luck_bonus);
        mana_crystals += added_mana;
    }
    public void PlayerLevelUp()
    {
        player.LevelUp();
        stat_points += player.playerLevel;
    }

    public void PlayerDefeated()
    {
        coins -= player.max_health;
        player.health = player.max_health;
        OnHealthChange();
        if (coins < 0)
        {
            coins = 0;
        }
        experience -= player.playerLevel;
        if (experience < 0)
        {
            experience = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        SaveState();
        ShowText("You were defeated but I brought you back.", 15, Color.red, familiar.transform.position, Vector3.zero, 2.0f);
    }

    public void OnHealthChange()
    {
        float ratio  = (float)player.health / (float)player.max_health;
        healthBar.localScale = new Vector3(ratio, 1, 1);
        healthText.text = player.health.ToString()+" / "+player.max_health.ToString();
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
        PlayerStatsWrapper player_stats = new PlayerStatsWrapper();
        player_stats.UpdateData();
        string player_stats_json = JsonUtility.ToJson(player_stats, true);
        File.WriteAllText("Assets/Saves/player_stats.json", player_stats_json);
        FamiliarStatsWrapper familiar_stats = new FamiliarStatsWrapper();
        familiar_stats.UpdateData();
        string familiar_stats_json = JsonUtility.ToJson(familiar_stats, true);
        File.WriteAllText("Assets/Saves/familiar_stats.json", familiar_stats_json);
        Debug.Log("Saved");
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string player_stats = File.ReadAllText("Assets/Saves/player_stats.json");
            PlayerStatsWrapper loaded_player_stats = JsonUtility.FromJson<PlayerStatsWrapper>(player_stats);
            player.SetStats(loaded_player_stats);
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            player.SetLevel(loaded_data.player_level);
            player.SetHealth(loaded_data.player_health);
            weapon.SetLevel(loaded_data.weapon_level);
            coins = loaded_data.coins;
            mana_crystals = loaded_data.mana_crystals;
            experience = loaded_data.experience;
            stat_points = loaded_data.stat_points;
            string familiar_stats = File.ReadAllText("Assets/Saves/familiar_stats.json");
            FamiliarStatsWrapper loaded_familiar_stats = JsonUtility.FromJson<FamiliarStatsWrapper>(familiar_stats);
            familiar.SetStats(loaded_familiar_stats);
        }
        else
        {
            Debug.Log("Load failed");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
