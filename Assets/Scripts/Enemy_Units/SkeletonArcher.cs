using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : UnitBase
{
    public override void Start(){
        health = 4;
        maxHealth = health;
        attackRange = 6;
        attackDamage = 3;
        moveRange = 4;
        unitName = "Skeleton Archer";
        phase = 0;
    }
}
