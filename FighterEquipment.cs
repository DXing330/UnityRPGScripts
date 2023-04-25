using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FighterEquipment : MonoBehaviour
{
    // Base stats.
    public int base_damage_multiplier = 0;
    public int base_damage_reduction = 0;
    public int base_i_frames = 25;
    public int base_dodge_chance = 0;
    public int base_move_speed = 100;
    public int base_dash_distance = 60;
    public int base_attack_cooldown = 50;
    public int base_knockback_resist = 20;
    // Bonus stats.
    public int bonus_damage_multiplier = 0;
    public int bonus_damage_reduction = 0;
    public int bonus_i_frames = 0;
    public int bonus_dodge_chance;
    public int bonus_move_speed = 0;
    public int bonus_dash_distance = 0;
    public int bonus_attack_speed = 0;
    public int bonus_knockback_resist = 0;
    // Stats.
    public int damage_multiplier;
    public int damage_reduction;
    public float i_frames;
    public int dodge_chance;
    public float move_speed;
    public float dash_distance;
    public float attack_cooldown;
    public float knockback_resist;

    public virtual void UpdateStats()
    {
        damage_multiplier = base_damage_multiplier + bonus_damage_multiplier;
        damage_reduction = base_damage_reduction + bonus_damage_reduction;
        float fbase_i_frames = base_i_frames;
        float fbonus_i_frames = bonus_i_frames;
        i_frames = (fbase_i_frames + fbonus_i_frames)/100;
        dodge_chance = base_dodge_chance + bonus_dodge_chance;
        float fbase_move_speed = base_move_speed;
        float fbonus_move_speed = bonus_move_speed;
        move_speed = (fbase_move_speed + fbonus_move_speed)/100;
        Debug.Log(move_speed);
        float fbase_dash_distance = base_dash_distance;
        float fbonus_dash_distance = bonus_dash_distance;
        dash_distance = (fbase_dash_distance + fbonus_dash_distance)/100;
        Debug.Log(dash_distance);
        float fbase_attack_cooldown = base_attack_cooldown;
        float fbonus_attack_speed = bonus_attack_speed;
        attack_cooldown = (fbase_attack_cooldown/(1+(fbonus_attack_speed)/100))/100;
        Debug.Log(attack_cooldown);
        float fbase_knockback_resist = base_knockback_resist;
        float fbonus_knockback_resist = bonus_knockback_resist;
        knockback_resist = (fbase_knockback_resist + fbonus_knockback_resist)/100;
        Debug.Log(knockback_resist);
    }
}