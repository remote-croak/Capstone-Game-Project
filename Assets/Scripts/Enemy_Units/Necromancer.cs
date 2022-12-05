using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : UnitBase{


    public override void Start(){
        health = 4;
        maxHealth = health;
        attackRange = 5; // summon range
        attackDamage = 0;
        moveRange = 4;
        unitName = "Necromancer";
        phase = 0;
    }

    public override void Attack(Tile_Content target){
        originFactory.GetVillainUnit(VillainUnits.Generic);
        Debug.Log("Necromancer line 19: Need to switch to zombie type");
    }

    public override bool Targetable(Tile_Content tile){
        if (tile.unit == null){
            return true;
        }
        return false;
    }
}