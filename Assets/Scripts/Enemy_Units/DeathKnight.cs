using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need to update how damage boost works once turn system is implemented
public class DeathKnight : UnitBase{
    private bool damageBoost {get; set;}
    private int boostValue {get; set;}

    public override void Start(){
        health = 6;
        maxHealth = health;
        attackRange = 1;
        attackDamage = 3;
        moveRange = 5;
        unitName = "Death Knight";
        phase = 0;

        damageBoost = false;
        boostValue = 2;
    }

    public override void Attack(Tile_Content target){
        if (damageBoost){
            attackDamage += boostValue;
            base.Attack(target);
            attackDamage -= boostValue;
            damageBoost = false;
        }
        else{base.Attack(target);}
        if (target.unit == null){
            damageBoost = true;
            originFactory.GetVillainUnit(VillainUnits.Generic);
            Debug.Log("DeathKnight line 33: need to switch to zombie type");
        }
    }
}