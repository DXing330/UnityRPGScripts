using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSpawnRoom : MonoBehaviour
{
    public List<Enemy> spawnable_enemies;
    public List<EnemyAnimated> spawnable_a_enemies;
    public List<EnemyRanged> spawnable_r_enemies;
    private int spawn_index;
    public bool normal;
    public bool animated;
    public bool ranged;
    public bool limited_use = false;
    public float spawn_cooldown;
    public int spawn_limit = 1;
    private int current_spawns = 0;
    private float last_spawn;
    public ContactFilter2D filter;
    private BoxCollider2D spawn_zone;
    protected Collider2D[] hits = new Collider2D[10];
    protected float check_cooldown = 6.0f;
    protected float last_check;

    void Start()
    {
        spawn_zone = GetComponent<BoxCollider2D>();
        last_check = -check_cooldown;
    }

    protected virtual void AdjustSpawnLimit(int new_limit)
    {
        spawn_limit = new_limit;
        limited_use = true;
    }
    
    protected virtual void Spawn()
    {
        if (Time.time - last_spawn > spawn_cooldown)
        {
            last_spawn = Time.time;
            Vector3 random_location =  Random.insideUnitSphere * 5;
            random_location.z = 0;
            if (normal)
            {
                spawn_index = Random.Range(0, spawnable_enemies.Count);
                Enemy clone = Instantiate(spawnable_enemies[spawn_index], transform.position + random_location, new Quaternion(0, 0, 0, 0));
            }
            else if (animated)
            {
                spawn_index = Random.Range(0, spawnable_a_enemies.Count);
                EnemyAnimated clone = Instantiate(spawnable_a_enemies[spawn_index], transform.position + random_location, new Quaternion(0, 0, 0, 0));   
            }
            else if (ranged)
            {
                spawn_index = Random.Range(0, spawnable_r_enemies.Count);
                EnemyRanged clone = Instantiate(spawnable_r_enemies[spawn_index], transform.position + random_location, new Quaternion(0, 0, 0, 0));   
            }
            if (limited_use)
            {
                spawn_limit--;
                if (spawn_limit <= 0)
                {
                    Destroy(gameObject);
                }
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

    void Update()
    {
        if (Time.time - last_check > check_cooldown)
        {
            last_check = Time.time;
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
}
