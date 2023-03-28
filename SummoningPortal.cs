using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningPortal : MonoBehaviour
{
    public Enemy enemy_to_spawn;
    public float distance_to_spawn;
    public float spawn_cooldown;
    private float spawn_limit;
    private float last_spawn;
    private Transform player_position;

    public virtual void Start()
    {
        player_position = GameManager.instance.player.transform;
        spawn_limit = GameManager.instance.player.playerLevel;
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time - last_spawn > spawn_cooldown && (player_position.position - transform.position).magnitude < distance_to_spawn && spawn_limit != 0)
        {
            last_spawn = Time.time;
            spawn_limit -= 1;
            Enemy clone = Instantiate(enemy_to_spawn, transform);
            clone.transform.position = transform.position;
        }
    }
}
