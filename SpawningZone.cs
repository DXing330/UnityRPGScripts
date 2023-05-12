using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningZone : MonoBehaviour
{
    public Enemy enemy_to_spawn;
    public EnemyAnimated animated_enemy_to_spawn;
    public EnemyRanged ranged_enemy_to_spawn;
    public bool normal;
    public bool animated;
    public bool ranged;
    public float spawn_cooldown;
    private int spawn_limit = 1;
    private int current_spawns = 0;
    private float last_spawn;
    public ContactFilter2D filter;
    private BoxCollider2D spawn_zone;
    protected Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        spawn_zone = GetComponent<BoxCollider2D>();
        spawn_limit = Random.Range(0, GameManager.instance.current_depth) + 1;
    }

    protected virtual void Spawn()
    {
        if (Time.time - last_spawn > spawn_cooldown)
        {
            last_spawn = Time.time;
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
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        // Keep track of how many thing's you've spawned around you.
        if (coll.tag == "Enemy")
        {
            current_spawns += 1;
        }
        // Don't spawn if there are plaer characters nearby.
        if (coll.tag == "Fighter")
        {
            last_spawn = Time.time;
        }
    }

    protected virtual void Update()
    {
        current_spawns = 0;
        // Collision work.
        spawn_zone.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            OnCollide(hits[i]);

            // Clear the array after you're done.
            hits[i] = null;
        }
        if (current_spawns < spawn_limit)
        {
            Spawn();
        }
    }
}
