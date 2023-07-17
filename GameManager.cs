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
    public string saved_data;
    public StoryDataManager story;
    public SummonDataManager summons;
    public SpellDataManager spells;
    public VillageDataManager villages;
    public EquipmentDataManager all_equipment;
    // UI Stuff.
    public FloatingTextManager floatingTextManager;
    public FixedTextManager fixedTextManager;
    public InteractableTextManager interactableTextManager;
    public EventChoices current_event = null;
    public HUD hud;
    public RectTransform healthBar;
    public Text healthText;
    public RectTransform manaBar;
    public Text manaText;
    public RectTransform stamBar;
    public Text stamText;
    public HUDControls controls;

    // Resources/Logic.
    public int experience;
    public int current_day;
    protected int blood_deadline;
    public int danger_level;
    public int current_depth;
    public int current_max_depth;

    // Useful Generic Functions
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

    public List<string> InverstListOrder(List<string> list_to_reverse, List<string> newly_reversed_list)
    {
        newly_reversed_list.Clear();
        int list_length = list_to_reverse.Count;
        for (int i = list_length; i > 0; i--)
        {
            newly_reversed_list.Add(list_to_reverse[i-1]);
        }
        return newly_reversed_list;
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

    // Interactable Text.
    public void SetEvent(EventChoices eventChoices)
    {
        current_event = eventChoices;
        ShowInteractableText(current_event.words, current_event.speaker_name, current_event.choice_1_text, current_event.choice_2_text, current_event.choice_3_text);
    }

    public void ReceiveChoice(int choice)
    {
        current_event.ReceiveChoice(choice);
    }

    public void ShowInteractableText(string words, string name = "", string choice_1="", string choice_2="", string choice_3="")
    {
        interactableTextManager.ShowTexts(words, name, choice_1, choice_2, choice_3);
    }

    // Player actions.
    public void PickWeaponType(int index)
    {
        weapon.SetType(index);
    }

    public bool UpgradeWeapon(int type)
    {
        if (type >= 0)
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

    public void ClaimTile(int tile_num)
    {
        if (villages.collected_settlers > 0 && villages.collected_mana > 0)
        {
            villages.collected_settlers--;
            villages.collected_mana--;
            villages.tiles.ClaimTile(tile_num);
        }
    }

    public void ExploreTile(int tile_num)
    {
        villages.tiles.ExploreTile(tile_num);
        NewWeek();
    }

    // Player resources.
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

    // 0:Blood,1:Settlers,2:Mana,3:Gold,4:Food,5:Mats
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
        string new_text = "";
        if (amount > 0)
        {
            new_text += "+ "+amount;
        }
        else if (amount < 0)
        {
            new_text += amount;
        }
        switch (type)
        {
            case 0:
                villages.collected_blood += amount;
                ShowText(new_text+" blood crystals", 25, Color.red, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 1:
                villages.collected_settlers += amount;
                ShowText(new_text+" settlers", 25, Color.green, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 2:
                villages.collected_mana += amount;
                ShowText(new_text+" mana crystals", 25, Color.blue, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 3:
                villages.collected_gold += amount;
                ShowText(new_text+" coins", 20, Color.yellow, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 4:
                villages.collected_food += amount;
                ShowText(new_text+" food", 20, Color.green, player.transform.position, Vector3.up*25, 1.0f);
                break;
            case 5:
                villages.collected_materials += amount;
                ShowText(new_text+" materials", 20, Color.grey, player.transform.position, Vector3.up*25, 1.0f);
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
        bag.DropItems();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        NewWeek();
        SaveState();
        ShowInteractableText("You were defeated but I dragged you back, a few weeks regenerating in your coffin and you're as good as new.", "Spirit Guardian Blaty");
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
    [ContextMenu("New Game")]
    public void NewGame()
    {
        Directory.Delete("Assets/Saves", true);
        SaveState();
    }

    public void SaveData()
    {
        saved_data = "";
        saved_data += player.playerLevel.ToString()+"#";
        saved_data += player.health.ToString()+"#";
        saved_data += player.current_mana.ToString()+"#";
        saved_data += player.current_stamina.ToString()+"#";
        saved_data += familiar.level.ToString()+"#";
        saved_data += familiar.exp.ToString()+"#";
        saved_data += familiar.current_blood.ToString()+"#";
        saved_data += weapon.weapon_type.ToString()+"#";
        saved_data += weapon.weapon_levels+"#";
        saved_data += experience.ToString()+"#";
        saved_data += current_day.ToString()+"#";
    }

    public void SaveState()
    {
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        weapon.UpdateLevels();
        SaveData();
        File.WriteAllText("Assets/Saves/save_data", saved_data);
        story.SaveData();
        summons.SaveData();
        spells.SaveData();
        all_equipment.SaveData();
        villages.SaveData();
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (File.Exists("Assets/Saves/save_data"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data");
            string[] loaded_data = save_data.Split("#");
            player.SetLevel(int.Parse(loaded_data[0]));
            player.SetHealth(int.Parse(loaded_data[1]));
            player.SetMana(int.Parse(loaded_data[2]));
            player.SetStam(int.Parse(loaded_data[3]));
            familiar.SetLevel(int.Parse(loaded_data[4]));
            familiar.SetExp(int.Parse(loaded_data[5]));
            familiar.SetCBlood(int.Parse(loaded_data[6]));
            weapon.SetLevels(loaded_data[8]);
            weapon.SetType(int.Parse(loaded_data[7]));
            experience = int.Parse(loaded_data[9]);
            current_day = int.Parse(loaded_data[10]);
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
        blood_deadline = story.ReturnDeadlineDate();
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
        villages.tiles.PassWorldTime();
        story.CheckTime();
    }

    public void NewWeek()
    {
        for (int i = 0; i < 7; i++)
        {
            NewDay();
        }
    }

    public void ReturnHome()
    {
        current_depth = 0;
        current_max_depth = 0;
        bag.StoreItems();
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
