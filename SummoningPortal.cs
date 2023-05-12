using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningPortal : MonoBehaviour
{
    public Enemy enemy_to_spawn;
    public EnemyAnimated animated_enemy_to_spawn;
    public EnemyRanged ranged_enemy_to_spawn;
    public bool normal;
    public bool animated;
    public bool ranged;
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
            if (normal)
            {
                Enemy clone = Instantiate(enemy_to_spawn, transform.position, new Quaternion(0, 0, 0, 0));
                clone.DungeonBuff();
            }
            else if (animated)
            {
                EnemyAnimated clone = Instantiate(animated_enemy_to_spawn, transform.position, new Quaternion(0, 0, 0, 0));
                clone.DungeonBuff();
            }
            else if (ranged)
            {
                EnemyRanged clone = Instantiate(ranged_enemy_to_spawn, transform.position, new Quaternion(0, 0, 0, 0));
                clone.DungeonBuff();
            }
        }
        if (spawn_limit <= 0)
        {
            Destroy(gameObject);
        }
    }
}
