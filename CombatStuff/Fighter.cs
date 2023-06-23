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

    public FighterEquipment equipment_stats;

    // Iframes.
    public float i_frames = 0.25f;
    protected float last_i_frame;
    public int dodge_chance = 0;
    public float dodge_cooldown = 1.0f;
    protected float last_dodge;

    // Push.
    protected Vector3 push_direction;

    public virtual void DungeonBuff()
    {
        health += Random.Range(0, GameManager.instance.current_depth);
    }

    // All fighters can take damage and die.
    protected virtual void ReceiveDamage(Damage damage)
    {
        // Check for i_frames and damage.
        if (Time.time - last_i_frame > i_frames && damage.damage_amount > 0)
        {
            last_i_frame = Time.time;
            if (CheckDodge(damage))
            {
                last_dodge = Time.time;
                GameManager.instance.ShowText("Dodged", 20, Color.white, transform.position, Vector3.up*25, 1.0f);
            }
            else
            {
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
    }

    protected virtual bool CheckDodge(Damage damage)
    {
        if (Time.time - last_dodge > dodge_cooldown && dodge_chance > 0)
        {
            int adjusted_dodge_chance = damage.accuracy - dodge_chance;
            int random_number = Random.Range(0, 99);
            if (adjusted_dodge_chance <= random_number)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    // Fighters will have resistances to certain types of damage.
    protected virtual int CheckResistances(Damage damage)
    {
        float reduction_float = damage_reduction;
        // Universal Damage Reduction will never be 100%.
        float reduction_percentage = reduction_float/(50 + reduction_float);
        damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1.0f - reduction_percentage));
        float resistance_float = 0f;
        if (resistances != null)
        {
            resistance_float = resistances.CheckResistance(damage.damage_type);
        }
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
