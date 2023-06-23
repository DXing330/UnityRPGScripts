public class Equipment
{
    protected int different_stats = 10;
    // Determines what list the equipment is sorted into.
    public int type_id = 0;
    // Determines whether the player gets bonus stats from the equipment.
    public int equipped = 0;
    // Determines how many bonus stats the equipment provides.
    public int rarity = 0;
    // Determines which special stat the the equipment provides.
    public int special_effect_id = 0;
    public int special_effect_strength = 0;
    // Basic stats, all equipment reduces damage.
    public int physical_resist = 0;
    public int fire_resist = 0;
    public int poison_resist = 0;
    public int magic_resist = 0;
    public int divine_resist = 0;

    public Equipment(int s_effect_id = 0, int s_effect_strength = 0, int phys_resist = 0, int f_resist = 0, int pois_resist = 0, int magi_resist = 0, int divi_resist = 0)
    {
        special_effect_id = s_effect_id;
        special_effect_strength = s_effect_strength;
        physical_resist = phys_resist;
        fire_resist = f_resist;
        poison_resist = pois_resist;
        magic_resist = magi_resist;
        divine_resist = divi_resist;
    }

    public virtual string ConvertSelftoString()
    {
        string stat_string = "";
        stat_string += type_id.ToString()+"|";
        stat_string += rarity.ToString()+"|";
        stat_string += equipped.ToString()+"|";
        stat_string += special_effect_id.ToString()+"|";
        stat_string += special_effect_strength.ToString()+"|";
        stat_string += physical_resist.ToString()+"|";
        stat_string += fire_resist.ToString()+"|";
        stat_string += poison_resist.ToString()+"|";
        stat_string += magic_resist.ToString()+"|";
        stat_string += divine_resist.ToString();
        return stat_string;
    }

    public virtual void ReadStatsfromList(string[] stats, int start)
    {
        for (int i = 0; i < different_stats; i++)
        {
            UpdateStatsOnebyOne(int.Parse(stats[start+i]), i);
        }
    }

    public virtual void UpdateStatsOnebyOne(int stat, int position)
    {
        switch (position)
        {
            case 0:
                type_id = stat;
                break;
            case 1:
                equipped = stat;
                break;
            case 2:
                rarity = stat;
                break;
            case 3:
                special_effect_id = stat;
                break;
            case 4:
                special_effect_strength = stat;
                break;
            case 5:
                physical_resist = stat;
                break;
            case 6:
                fire_resist = stat;
                break;
            case 7:
                poison_resist = stat;
                break;
            case 8:
                magic_resist = stat;
                break;
            case 9:
                divine_resist = stat;
                break;
        }
    }

}
