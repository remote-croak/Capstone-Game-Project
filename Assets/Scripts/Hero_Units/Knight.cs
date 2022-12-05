using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : UnitBase
{
    public override void Start(){
        health = 16;
        maxHealth = health;
        attackRange = 1;
        attackDamage = 3;
        moveRange = 5;
        unitName = "Knight";
        phase = 0;
    }
}
