using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tile_Factory : ObjectFactory
{
    [SerializeField] Tile_Content none_prefab = default;
    [SerializeField] Tile_Content grass_prefab = default;
    [SerializeField] Tile_Content sand_prefab = default;
    [SerializeField] Tile_Content mountain_prefab = default;
    [SerializeField] Tile_Content water_prefab = default;
    [SerializeField] Tile_Content hill_prefab = default;
    [SerializeField] Tile_Content forest_prefab = default;
    [SerializeField] Tile_Content road_prefab = default;

    public void Reclaim(Tile_Content terrain){
        Debug.Assert(terrain.Origin == this, "Wrong! Factory Reclaimed");
        Destroy(terrain.gameObject);
    }

    
    public Tile_Content Get_Tile_Content (Tile_Terrain terrain_type){
        switch (terrain_type){
            case Tile_Terrain.Grass: return Get_Prefab(grass_prefab);
            case Tile_Terrain.Hill: return Get_Prefab(hill_prefab);
            case Tile_Terrain.Sand: return Get_Prefab(sand_prefab);
            case Tile_Terrain.Mountain: return Get_Prefab(mountain_prefab);
            case Tile_Terrain.Water: return Get_Prefab(water_prefab);
            case Tile_Terrain.Forest: return Get_Prefab(forest_prefab);
            case Tile_Terrain.Road: return Get_Prefab(road_prefab);
        }
        Debug.Assert(false, "Unsupported Type: " + terrain_type);
        return null;
    }

    private Tile_Content Get_Prefab (Tile_Content prefab){
        Tile_Content instance = CreateObjectInstance(prefab);
        instance.Origin = this;
        return instance;
    }
}
