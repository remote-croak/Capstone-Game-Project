using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour{
    protected Build_Surface map;
    protected UnitFactory originFactory;

    public int health {get; set;}
    public int maxHealth {get; set;}
    public int attackRange {get; set;}
    public int attackDamage {get; set;}
    public int moveRange {get; set;}
    public string unitName { get; set;}
    public int phase {get; set;}
    public int AOERadius {get; set;}

    public Vector2 mapPosition {get; set;}
    public UnitBase primaryTarget {get; set;}
    private GameObject _target {get; set;}


    public virtual void Start(){
        health = 1;
        maxHealth = health;
        attackRange = 5;
        attackDamage = 0;
        moveRange = 5;
        unitName = "generic unit";
        phase = 0;
        AOERadius = 0;
        map = FindObjectOfType<Build_Surface>();
    }
    
    public virtual void Attack(Tile_Content target){
        target.unit.health -= attackDamage;
        if (target.unit.health <= 0){
            Destroy(target.unit);
            target.unit = null;
        }
    }

    public virtual bool Targetable(Tile_Content tile){
        if (tile.unit != null && tile.unit.tag != gameObject.tag){
            return true;
        }
        return false;
    }

    public UnitFactory OriginFactory{
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory");
            originFactory = value;
        }
    }

    // used for resolving AOE areas, returns a list of tiles within the AOE area.
    public virtual List<Tile_Content> AOE(Tile_Content tile){
        List<Tile_Content> tiles = new List<Tile_Content>();
        int origin = tile.arrayPosition;
        int max = map.tile_array.Length;
        float center_x = tile.mapPosition.x;

        for (int horizontal = AOERadius; horizontal >= (0-AOERadius); horizontal--){
            int offset = AOERadius - Mathf.Abs(horizontal);
            for (int vertical = offset; vertical >= 0-offset; vertical--){
                int current = origin + (vertical*map.x) + horizontal;
                float current_x = center_x + (float)horizontal;
                if (current >= 0 && current < max && map.min.x <= current_x && map.max.x >= current_x){
                    tiles.Add(map.tile_array[current]);
                }
            }
        }
        return tiles;
    }
}