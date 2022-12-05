using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : UnitBase{
    
    public override void Start(){
        health = 10;
        maxHealth = health;
        attackRange = 6;
        attackDamage = 3;
        moveRange = 4;
        unitName = "Archer";
        phase = 0;
    }
}