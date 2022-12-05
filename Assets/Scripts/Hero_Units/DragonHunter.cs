using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHunter : UnitBase{

    public override void Start(){
        health = 24;
        maxHealth = health;
        attackRange = 4;
        attackDamage = 1;
        moveRange = 4;
        unitName = "Dragon Hunter";
        phase = 0;
        AOERadius = 2;
        map = FindObjectOfType<Build_Surface>();
    }

    public override void Attack(Tile_Content target){
        List<Tile_Content> tiles = base.AOE(target);
        foreach (Tile_Content tile in tiles)
        {
            if (tile.unit != null){
                base.Attack(tile);
                tile.unit.primaryTarget = this;
            }
        }
    }
}