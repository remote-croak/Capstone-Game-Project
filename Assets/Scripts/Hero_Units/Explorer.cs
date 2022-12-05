using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : UnitBase
{
    public override void Start(){
        health = 10;
        maxHealth = health;
        attackRange = 1; // Heal Range
        attackDamage = 1; // Heal Amount
        moveRange = 8;
        unitName = "Explorer";
        phase = 0;
    }

    public override void Attack(Tile_Content target){
        if (target.unit.health+attackDamage > target.unit.maxHealth){
            target.unit.health = maxHealth;
        }
        else{target.unit.health += attackDamage;}
    }
}