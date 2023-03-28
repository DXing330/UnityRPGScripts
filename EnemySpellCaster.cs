using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellCaster : MonoBehaviour
{
    public SpellAttack spell_to_cast;
    public CircleCollider2D detection_range;
    protected float cast_cooldown = 5.0f;
    protected float last_cast = -5.0f;
    protected int cast_limit = 5;
    protected Transform target_transform = null;
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        detection_range = GetComponent<CircleCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (target_transform == null)
        {
            detection_range.OverlapCollider(filter, hits);
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j] == null)
                {
                    continue;
                }
                if (hits[j].tag == "Fighter")
                {
                    target_transform = hits[j].transform;
                    return;
                }
                hits[j] = null;
            }
        }
        else if (target_transform != null)
        {
            if (Time.time - last_cast > cast_cooldown && cast_limit > 0)
            {
                Cast_Spell();
            }
        }
    }

    protected virtual void Cast_Spell()
    {
        last_cast = Time.time;
        cast_limit--;
        SpellAttack clone = Instantiate(spell_to_cast, target_transform.position + new Vector3(0, 0.16f, 0), new Quaternion(0, 0, 0, 0));
    }
}
