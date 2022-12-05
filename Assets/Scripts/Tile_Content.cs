using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Content : MonoBehaviour
{

    [SerializeField] Tile_Terrain terrain_type = default;
    [SerializeField] bool spawn_point = false;
    
    private Tile_Factory origin;

    public Tile_Terrain Terrain => terrain_type;

    // relevant positional data
    public int arrayPosition {get; set;}
    public Vector2 mapPosition {get; set;}
    public int moveCost {get; set;}

    // occupying unit, also sets positional data for occupying unit
    private UnitBase _unit;
    public UnitBase unit {
        get{return _unit;}
        set{
            _unit = value;
            if (value != null){
                value.mapPosition = mapPosition;
            }
        }
    }

    // occupying portal
    public Portal portal {get; set;}

    public Tile_Factory Origin{
        get => origin;
        set{ 
            Debug.Assert(origin == null, "Redefined origin factory!");
            origin = value;
        }
    }

    public void Start(){
        moveCost = 1;
    }

    public void Recycle(){
        origin.Reclaim(this);
    }
}
