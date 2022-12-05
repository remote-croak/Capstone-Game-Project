using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    [SerializeField] Build_Surface map;

    // Highlight tile information
    private Tile_Content tile = null; // current tile
    private List<Tile_Content> moveable = new List<Tile_Content>(); // tiles you can move to
    private Dictionary<Tile_Content, Tile_Content> targetable = new Dictionary<Tile_Content, Tile_Content>(); // tiles you can target, and from where
    private Dictionary<Tile_Content, int> traversed = new Dictionary<Tile_Content, int>();
    private UnitBase unit = null;

    // common highlight variable
    private Vector3 highlight = new Vector3(0,1,0);
    
    // portal highlight
    public void PortalHighlight(Tile_Content tile, int direction){
        if(tile.portal != null){
            tile.portal.transform.position += direction*highlight;
        }
    }
    
    public void Highlight(){
        foreach(Tile_Content tiles in moveable){
            // highlight moveable
            tiles.transform.position += highlight;
            PortalHighlight(tiles, 1);
        }
        foreach(Tile_Content tiles in targetable.Keys){
            // highlight targetable
            tiles.transform.position += highlight;
            tiles.unit.transform.position += highlight;
            PortalHighlight(tiles, 1);
        }
    }

    
    public void Unhighlight(){
        if (tile != null){
            foreach(Tile_Content tiles in moveable){
                // unhighlight moveable
                tiles.transform.position -= highlight;
                PortalHighlight(tiles, -1);
            }
            foreach(Tile_Content tiles in targetable.Keys){
                // unhighlight targetable
                tiles.transform.position -= highlight;
                if (tiles.unit != null){
                    tiles.unit.transform.position -= highlight;
                }
                PortalHighlight(tiles, -1);
            }
            moveable.Clear();
            targetable.Clear();
            traversed.Clear();
            tile = null;
        }
    }

    // needs updating (shift into units after enemy AI is implemented)
    public void Select(Tile_Content selected){
        if(moveable.Contains(selected)){
            unit.phase = 1;
            Move(selected);
        }
        else if(targetable.ContainsKey(selected)){
            unit.phase = 2;
            if(targetable[selected] != tile){
                Move(targetable[selected]);
            }
            else{Unhighlight();}
            unit.Attack(selected);
        }
        else if(selected.tag == "Terrain"){
            Unhighlight();
            tile = selected;
            if (selected.unit != null){
                unit = selected.unit;
                if (unit.phase == 0){Traverse(selected);}
                else if(unit.phase == 1){Target(tile);}
                Highlight();
            }
        }
        else{Unhighlight();}
    }

    // moves current active unit to target destination
    public void Move(Tile_Content target){
        target.unit = tile.unit;
        tile.unit = null;
        Unhighlight();
        unit.transform.position = new Vector3 (target.transform.position.x, 1, target.transform.position.z);
    }

    /*  Usage: Traverse(Tile_Content current)
        Required Parameter: 
            Tile_Content current: the location with a unit you want to highlight relevant tiles for
        Optional Parameter:
            int range: utilized for recursion, do not fill in
        Return: none
            this function modifies variables within this class */
    public void Traverse(Tile_Content current, int range = -500){
        // unity peculiarities necessitate this check
        if(range == -500){
            range = unit.moveRange;
            Target(current);
        }
        // add current tile to applicable list
        try{
            traversed.Add(current, range);
            if(current.unit == null){
                moveable.Add(current);
                Target(current);
            }
        }
        catch{
            if (traversed[current] < range) {traversed[current] = range;}
            else {return;}
        }
        // explore surrounding tiles
        if (range > 0){
            range -= current.moveCost;
            if (map.Up(current) != null) {Traverse(map.Up(current), range);}
            if (map.Left(current) != null) {Traverse(map.Left(current), range);}
            if (map.Down(current) != null) {Traverse(map.Down(current), range);}
            if (map.Right(current) != null) {Traverse(map.Right(current), range);}
        }
    }
    
    /*  Usage: Target(Tile_Content center)
        Parameter: Tile_Content center: center of the searchable area
        Return: none
            this function modifies variables within this class */
    public void Target(Tile_Content center){
        Tile_Content reachable;
        int origin = center.arrayPosition;
        int radius = unit.attackRange;
        int max = map.tile_array.Length;
        float center_x = center.mapPosition.x;

        // tiles to check within an area
        for (int horizontal = radius; horizontal >= (0-radius); horizontal--){
            int offset = radius - Mathf.Abs(horizontal);
            // determine vertical range along the horizontal axis
            for (int vertical = offset; vertical >= 0-offset; vertical--){
                int current = origin + (vertical*map.x) + horizontal;
                float current_x = center_x + (float)horizontal;
                // if current tile in area existis on the map
                if (current >= 0 && current < max && map.min.x <= current_x && map.max.x >= current_x){
                    reachable = map.tile_array[current];
                    // add to relevant targeting location
                    if (unit.Targetable(reachable)){
                        try{targetable.Add(reachable, center);}
                        catch{}
                    }
                }
            }
        }
    }
}
