using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : UnitBase{

    public override void Start(){
        health = 8;
        maxHealth = health;
        attackRange = 5;
        attackDamage = 3;
        moveRange = 4;
        unitName = "Wizard";
        phase = 0;
        AOERadius = 3;
        map = FindObjectOfType<Build_Surface>();
    }

    public override void Attack(Tile_Content target){
        List<Tile_Content> tiles = base.AOE(target);
        foreach (Tile_Content tile in tiles)
        {
            if (tile.unit != null){
                base.Attack(tile);
            }
        }
    }
}