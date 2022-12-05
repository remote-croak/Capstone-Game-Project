using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : UnitBase{
    
    public override void Start(){
        health = 6;
        maxHealth = health;
        attackRange = 1;
        attackDamage = 3;
        moveRange = 3;
        unitName = "Zombie";
        phase = 0;
    }
}
