using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Passive ally that assists the hero.
public class Familiar : MonoBehaviour
{
    // Will need to stick around the player.
    private Transform player_transform;
    // Familiar will fly around the player.
    private float rotate_speed = 1.0f;
    private float radius = 0.16f;
    private Vector3 center;
    private float angle;
    // Has a chance of healing the hero, with their blood magic.
    private float heal_cooldown = 6.6f;
    private float last_heal;
    // Keep track of collisions.
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];
    // Base stats that will automatically adjust on level.
    private float push_force = 1.0f;
    private float hit_cooldown = 0.66f;
    private float last_hit;
    // Customizable stats that the player can put stat points into whenever the familiar levels up.
    public int upgrade_cost = 1;
    public int bonus_rotate_speed = 0;
    public float bonus_rotate_speed_float = 0;
    public int heal_threshold_increase = 0;
    public int bonus_damage = 0;
    public int bonus_push_force = 0;
    public int bonus_heal = 0;
    // Stats where the player feeds mana and the familiar levels up.
    public int level;
    public int exp;

    protected void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        player_transform = GameManager.instance.player.transform;
        center = player_transform.position;
    }

    private void FixedUpdate()
    {
        center = player_transform.position;
        angle += (rotate_speed + (bonus_rotate_speed_float/10)) * Time.deltaTime;
        var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
        transform.position = center + offset;
        boxCollider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            OnCollide(hits[i]);

            // Clear the array after you're done.
            hits[i] = null;
        }
        if (Time.time - last_heal > heal_cooldown)
        {
            last_heal = Time.time;
            CheckOnMaster();
        }
    }

    protected void CheckOnMaster()
    {
        if ((GameManager.instance.player.health) * 3 < GameManager.instance.player.max_health - heal_threshold_increase)
            {
                HealMaster();
            }
        else if (GameManager.instance.player.health >= GameManager.instance.player.max_health)
        {
            int talk = Random.Range(0, 10);
            if (talk == 0)
            {
                GameManager.instance.ShowText("Find Enemies! Make Blood!", 15, Color.red, transform.position, Vector3.up*25, 0.66f);
            }
            if (talk == 1)
            {
                Damage damage = new Damage
                {
                    damage_amount = GameManager.instance.player.playerLevel,
                    origin = transform.position,
                    push_force = 0
                };
                GameManager.instance.player.SendMessage("ReceiveDamage", damage);
                GameManager.instance.ShowText("Blood from Master.", 15, Color.red, transform.position, Vector3.up*25, 0.66f);
            }
        }
    }
    protected void HealMaster()
    {
        GameManager.instance.player.SendMessage("ReceiveHealing", bonus_heal+1);
        GameManager.instance.ShowText("Blood for Master.", 15, Color.white, transform.position, Vector3.up*25, 0.66f);
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Enemy" && Time.time - last_hit > hit_cooldown)
        {
            last_hit = Time.time;
            // Make a damage object and send it to the player.
            Damage damage = new Damage
            {
                damage_amount = 1 + bonus_damage,
                origin = transform.position,
                push_force = push_force + (bonus_push_force/10)
            };
            coll.SendMessage("ReceiveDamage", damage);
            int laugh = Random.Range(0, 5);
            if (laugh == 0)
            {
                GameManager.instance.ShowText("Hehe, Fresh Blood.", 15, Color.red, transform.position, Vector3.up*25, 0.66f);
            }
        }
    }

    public void SetStats(FamiliarStatsWrapper loaded_stats)
    {
        level = loaded_stats.level;
        exp = loaded_stats.exp;
        bonus_rotate_speed = level;
        bonus_rotate_speed_float = bonus_rotate_speed * 1.0f;
        heal_threshold_increase = level;
        bonus_damage = level;
        bonus_push_force = level;
        bonus_heal = level;
    }

    public void GainExp(int exp_points)
    {
        exp += exp_points;
        LevelUp();
    }

    public void LevelUp()
    {
        int variance = Random.Range(-level, level);
        if (exp >= level*level + variance)
        {
            exp -= level*level + variance;
            level++;
            bonus_rotate_speed++;
            bonus_rotate_speed_float = bonus_rotate_speed * 1.0f;
            heal_threshold_increase++;
            bonus_damage++;
            bonus_push_force++;
            bonus_heal++;
        }
    }
}
