using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public info.
    public int health = 10;
    public int max_health = 10;
    public int defense = 0;
    public int damage_reduction = 0;
    public float recovery_speed = 0.2f;

    // Resistances.
    public FighterResistances resistances;

    // Equipment.
    public FighterEquipment armor = null;

    // Iframes.
    protected float i_frames = 0.25f;
    public float last_i_frame;

    // Push.
    protected Vector3 push_direction;

    // Fighters can use equipment to adjust their stats.
    protected virtual void Equip(FighterEquipment equipment)
    {
        if (!equipment.equipped && equipment.armor && armor == null)
        {
            equipment.equipped = true;
            armor = equipment;
            equipment.AddResistances(resistances);
        }
    }

    protected virtual void UnequipArmor()
    {
        if (armor != null)
        {
            armor.RemoveResistances(resistances);
            armor.equipped = false;
            armor = null;
        }
    }

    // All fighters can take damage and die.
    protected virtual void ReceiveDamage(Damage damage)
    {
        if (Time.time - last_i_frame > i_frames)
        {
            last_i_frame = Time.time;
            int damage_taken = CheckResistances(damage);
            health -= damage_taken;
            push_direction = (transform.position - damage.origin).normalized * damage.push_force;
            GameManager.instance.ShowText(damage.damage_amount.ToString(), 20, Color.red, transform.position, Vector3.up*25, 1.0f);

            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }
    }

    // Fighters will have resistances to certain types of damage.
    protected virtual int CheckResistances(Damage damage)
    {
        float reduction_float = damage_reduction;
        float reduction_percentage = reduction_float/(100 + reduction_float);
        damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1.0f - reduction_percentage));
        float resistance_float = resistances.CheckResistance(damage.damage_type);
        float resistance_percentage = resistance_float/100;
        damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1-resistance_percentage));
        if (damage.damage_amount < 1)
        {
            damage.damage_amount = 1;
        }
        return damage.damage_amount;
    }

    // Fighters can also be healed, by fountains or other things.
    protected virtual void ReceiveHealing(int healing)
    {
        if (health < max_health)
        {
            health += healing;
            GameManager.instance.ShowText(healing.ToString(), 20, Color.green, transform.position, Vector3.up*25, 1.0f);
            if (health > max_health)
            {
                health = max_health;
            }
        }
    }

    protected virtual void Death()
    {

    }
}
