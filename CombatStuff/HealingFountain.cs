using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collideable
{
    public int heal_amount = 1;
    public float heal_cooldown = 1.0f;
    public float last_heal;
    public int total_healing = 100;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            OnCollect();
    }

    protected void OnCollect()
    {
        if (total_healing != 0)
        {
            if (Time.time - last_heal > heal_cooldown)
            {
                last_heal = Time.time;
                GameManager.instance.player.SendMessage("ReceiveHealing", heal_amount);
                total_healing -= heal_amount;
            }
        }
    }
}
