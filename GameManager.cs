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


    // Player Stuff.
    public Player player;
    public Bag bag;
    public Weapon weapon;
    public Familiar familiar;
    // Data Managers.
    public StoryDataManager story;
    public SummonDataManager summons;
    public SpellDataManager spells;
    public VillageDataManager villages;
    public EquipmentDataManager all_equipment;
    // UI Stuff.
    public FloatingTextManager floatingTextManager;
    public FixedTextManager fixedTextManager;
    public HUD hud;
    public RectTransform healthBar;
    public Text healthText;
    public RectTransform manaBar;
    public Text manaText;
    public RectTransform stamBar;
    public Text stamText;

    // Resources/Logic.
    public int experience;
    public int current_day;
    public int danger_level;
    public int current_depth;
    public int current_max_depth;

    public string ConvertListToString(List<string> string_list)
    {
        string returned = "";
        for (int i = 0; i < string_list.Count; i++)
        {
            returned += string_list[i];
            if (i < string_list.Count-1)
            {
                returned += "|";
            }
        }
        return returned;
    }

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

    public void PickWeaponType(int index)
    {
        weapon.SetType(index);
    }

    public bool UpgradeWeapon(int type)
    {
        int current_weapon_level = int.Parse(weapon.weapon_levels_list[type]);
        int price = current_weapon_level * current_weapon_level;
        if (villages.collected_materials >= price && villages.collected_gold >= price)
        {
            weapon.weapon_levels_list[type] = (current_weapon_level+1).ToString();
            villages.collected_materials -= price;
            villages.collected_gold -= price;
            return true;
        }
        return false;
    }

    public void EatMana()
    {
        Debug.Log("Eating Mana");
        if (villages.collected_mana > 0)
        {
            villages.collected_mana--;
            player.EatMana();
            OnManaChange();
            GrantExp(1);
        }
    }

    public void DrinkBlood()
    {
        Debug.Log("Drinking Blood");
        if (villages.collected_blood > 0)
        {
            villages.collected_blood--;
            player.DrinkBlood();
            OnHealthChange();
            GrantExp(1);
        }
    }

    public void Sleep()
    {
        player.RecoverStamina();
        NewDay();
    }

    public void FeedFamiliarMana()
    {
        Debug.Log("Feeding Mana");
        if (villages.collected_mana > 0)
        {
            villages.collected_mana--;
            familiar.GainExp(1);
        }
    }

    public void FeedFamiliarBlood()
    {
        Debug.Log("Feeding Blood");
        if (villages.collected_blood > 0)
        {
            villages.collected_blood--;
            familiar.DrinkBlood();
        }
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

    // 0:Blood,1:EXP,2:Mana,3:Gold,4:Food,5:Mats
    public void CollectResource(int type, int amount)
    {
        switch (type)
        {
            case 0:
                bag.blud += amount;
                ShowText("+ "+amount+" blood crystals", 25, Color.red, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 1:
                GrantExp(amount);
                break;
            case 2:
                bag.mana += amount;
                ShowText("+ "+amount+" mana crystals", 25, Color.blue, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 3:
                bag.gold += amount;
                ShowText("+ "+amount+" coins", 20, Color.yellow, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 4:
                bag.food += amount;
                ShowText("+ "+amount+" food", 20, Color.green, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 5:
                bag.mats += amount;
                ShowText("+ "+amount+" materials", 20, Color.grey, player.transform.position, Vector3.up*25, 1.0f);
                break;
        }
    }

    public void GainResource(int type, int amount)
    {
        switch (type)
        {
            case 0:
                villages.collected_blood += amount;
                ShowText("+ "+amount+" blood crystals", 25, Color.red, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 1:
                GrantExp(amount);
                break;
            case 2:
                villages.collected_mana += amount;
                ShowText("+ "+amount+" mana crystals", 25, Color.blue, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 3:
                villages.collected_gold += amount;
                ShowText("+ "+amount+" coins", 20, Color.yellow, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 4:
                villages.collected_food += amount;
                ShowText("+ "+amount+" food", 20, Color.green, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 5:
                villages.collected_materials += amount;
                ShowText("+ "+amount+" materials", 20, Color.grey, player.transform.position, Vector3.up*25, 1.0f);
                break;
        }
    }

    public void PlayerLevelUp()
    {
        player.LevelUp();
    }

    public void PlayerDefeated()
    {
        villages.collected_gold -= player.max_health + player.max_mana;
        player.health = 1;
        player.current_mana = 0;
        OnHealthChange();
        OnManaChange();
        if (villages.collected_gold < 0)
        {
            villages.collected_gold = 0;
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

    public void OnStamChange()
    {
        float ratio = (float)player.current_stamina / (float)player.max_stamina;
        stamBar.localScale = new Vector3(ratio, 1, 1);
        stamText.text = (ratio * 100.0f).ToString() + "%";
    }

    public float StaminaRatio()
    {
        return (float)player.current_stamina / (float)player.max_stamina;
    }

    // Saving and loading.
    public void SaveState()
    {
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        weapon.UpdateLevels();
        SaveDataWrapper save_data = new SaveDataWrapper();
        save_data.UpdateData();
        string save_json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText("Assets/Saves/save_data.json", save_json);
        FamiliarStatsWrapper familiar_stats = new FamiliarStatsWrapper();
        familiar_stats.UpdateData();
        string familiar_stats_json = JsonUtility.ToJson(familiar_stats, true);
        File.WriteAllText("Assets/Saves/familiar_stats.json", familiar_stats_json);
        Debug.Log("Saved");
        story.SaveData();
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
            player.SetHealth(loaded_data.player_hlth);
            player.SetStam(loaded_data.player_stam);
            player.SetMana(loaded_data.player_mana);
            weapon.SetLevels(loaded_data.weapon_levels);
            weapon.SetType(loaded_data.weapon_type);
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
        story.LoadData();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Randomize spawnpoints when entering a new scene.
        int spawn_zone = Random.Range(0, 3);
        switch (spawn_zone)
        {
            case 0:
                player.transform.position = GameObject.Find("SpawnPoint").transform.position;
                break;
            case 1:
                if (GameObject.Find("SpawnPoint_1") != null)
                {
                    player.transform.position = GameObject.Find("SpawnPoint_1").transform.position;
                }
                else
                {
                    player.transform.position = GameObject.Find("SpawnPoint").transform.position;
                }
                break;
            case 2:
                if (GameObject.Find("SpawnPoint_2") != null)
                {
                    player.transform.position = GameObject.Find("SpawnPoint_2").transform.position;
                }
                else
                {
                    player.transform.position = GameObject.Find("SpawnPoint").transform.position;
                }
                break;
        }
    }

    public void NewDay()
    {
        current_day++;
        villages.tiles.PassTime();
    }

    public void NewWeek()
    {
        current_day += 7;
        villages.tiles.PassTime();
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
