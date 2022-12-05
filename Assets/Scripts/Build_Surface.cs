using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build_Surface : MonoBehaviour{

    [SerializeField] Transform surface_dimensions;
    
    [SerializeField] Texture2D grid_texture = default;

    public Tile_Content[] tile_array {get;private set;}

    private Vector2Int size;
    private Vector2 offset;

    private Tile_Content surface;
    private Tile_Factory content_factory;
    private Transform surface_position;

    // Bounding box
    public Vector2 min {get; private set;}
    public Vector2 max {get; private set;}
    public int x {get; private set;}
    public int y {get; private set;}

    public void Initialize(Vector2Int size, Tile_Factory content_factory){

        this.size = size;
        this.content_factory = content_factory;
        surface_dimensions.localScale = new Vector3(size.x, size.y, 1f);

        offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        tile_array = new Tile_Content[this.size.x * this.size.y];

        for (int t = 0, r = 0; r < size.y; r++){ // the number of rows
            for(int c = 0; c < size.x; c++, t++){ // the number of columns

                //Tiles with content(textures, info, position);
                surface = tile_array[t] = content_factory.Get_Tile_Content(Random_Tile());

                // set positional data for tiles
                surface.arrayPosition = t;
                
                // makes the transform data from tile content to match surface transform data
                surface_position = surface.GetComponent<Transform>();

                // surface_position becomes the parent
                surface_position.SetParent(transform, false);
                surface_position.localPosition = new Vector3(c-offset.x, 1f, r-offset.y);
                surface.mapPosition = (new Vector2(c - offset.x, r-offset.y));
            }
        }

        min = tile_array[0].mapPosition;
        max = tile_array[tile_array.Length - 1].mapPosition;
        x = (int)(1 + max.x - min.x);
        y = (int)(1 + max.y - min.y);
    }

    // A random function that grabs a random terrain tile type
    private Tile_Terrain Random_Tile(){
        
        int num = Random.Range(1,8);
        //Debug.Log(num + "\n");
        switch(num){
            case 1: return Tile_Terrain.Grass;
            case 2: return Tile_Terrain.Hill;
            case 3: return Tile_Terrain.Sand;
            case 4: return Tile_Terrain.Mountain;
            case 5: return Tile_Terrain.Water;
            case 6: return Tile_Terrain.Forest;
            case 7: return Tile_Terrain.Road;
        }
        return Tile_Terrain.None;
    }

    public Tile_Content Get_Tile(int tile_pos){
        return tile_array[tile_pos];
    }

    // returns tile in specified relative direction
    public Tile_Content Up(Tile_Content tile){
        try{return (tile_array[tile.arrayPosition + x]);}
        catch{return null;}
    }
    public Tile_Content Down(Tile_Content tile){
        try{return (tile_array[tile.arrayPosition - x]);}
        catch{return null;}
    }
    public Tile_Content Left(Tile_Content tile){
        if (tile.mapPosition.x > min.x){
            try{return (tile_array[tile.arrayPosition - 1]);}
            catch{}
        }
        return null;
    }
    public Tile_Content Right(Tile_Content tile){
        if (tile.mapPosition.x < max.x){
            try{return (tile_array[tile.arrayPosition + 1]);}
            catch{}
        }
        return null;
    }
    //access tile array and check tile position.
}