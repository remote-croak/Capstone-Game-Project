using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour{

// enemy spawn
// hero spawn
//private Locations[] spawn_points;

[SerializeField] Build_Surface map;

private int villainSpawnSize;
private int numVillainSpawns;
private int heroSpawnSize;
private int numHeroSpawns;

private List<Tile_Content> villainSpawn = new List<Tile_Content>();
private List<Tile_Content> heroSpawn = new List<Tile_Content>();
private List<Tile_Content> portalSpawn = new List<Tile_Content>();



// lists used for efficient access to units and spawns
public List<UnitBase> heros = new List<UnitBase>();
public List<UnitBase> villains = new List<UnitBase>();
public List<Tile_Content> portalList = new List<Tile_Content>();

private int greatest_map_length = 0;
private bool rowSpawn = false;
private bool colSpawn = false;

private Vector2 map_size;

private UnitFactory unitFactory;
private PortalFactory portalFactory;

[SerializeField] int debug_portalSpawn;
    public void HeroesVillains(Vector2 map_dimensions, UnitFactory units, PortalFactory portals){
        this.unitFactory = units;
        this.portalFactory = portals;
        this.map_size = map_dimensions;
        Set_Spawn_Data();
        HeroSpawnPoints();
        VillainSpawnPoints();
        Debug.Log("Spawn\n");
        //test_UnitSpawn(); // test an individual spawn at either a hero or villain spawn point
        
        SpawnHeroes();
        Debug.Log("Heroes");
        SpawnPortals();
        Debug.Log("Portals");
        SpawnVillains();

    }


    //Sets direction the units spawn(north/south or east/west) and the size of the spawn area for Villains and Heroes
    private void Set_Spawn_Data(){

        float difficulty = 0f;

        int random_difficulty = Random.Range(0,3);

        //determines which direction the units face.
        // if x is greater then the units face each other horizontally(rowSpawn)
        if (map_size.x > map_size.y){
            greatest_map_length = (int)map_size.x;
            rowSpawn = true;
        }
        // if y is greater then the units face each other vertically(colSpawn)
        else if (map_size.x < map_size.y){
            greatest_map_length = (int)map_size.y;
            colSpawn = true;
        }

        // if the dimensions are equal the direction is randomized
        else{

            greatest_map_length = (int)map_size.x;
            int row_col = Random.Range(0,2);
            
            if(row_col == 0){
                rowSpawn = true;
            }
            else{        
                colSpawn = true;
            }
        }

        //Randomly sets the difficulty of the map. This determines the spawn size of the villians
        //Also the size of the no man's land between the two sides.
        //This needs to be refactored so the randomized difficulty is uniform throughout the entire game.
        //Should it's own function where the difficulty is done with a switch statement
        if(random_difficulty == 0){
            difficulty = 0.25f; //easy difficulty
        }
        else if(random_difficulty == 1){
            difficulty = 0.5f; //medium difficulty
        }
        else{
            difficulty = 0.65f; //hard difficulty
        }

        villainSpawnSize = (int)Mathf.Round(greatest_map_length*difficulty);
        heroSpawnSize = (int)Mathf.Round(greatest_map_length*0.25f);

    }

    //Assigns spawns to tiles going from the southern section towards the north or
    // from the eastern section towards the west. 
    private void HeroSpawnPoints(){
        int tile_num = 0;
        
        //Fill the map horizontally with spawns starting from the eastern section going west.
        if(rowSpawn == true){
            for(int r = 1; r <= (int)map_size.y; r++){ // counts the rows. Should include the entire vertical length.
                for(int c = (greatest_map_length-1); c >= (greatest_map_length - heroSpawnSize); c--){
                    heroSpawn.Add(map.Get_Tile(tile_num + c));
                }

                // reverse count: multiples the last tile number by the next row number and then adds the next row number minus 1 to get the last tile of the next row.
                tile_num = (((int)map_size.x - 1) * r) + r;
                
            }
        }

        //Fill the map vertically with spawns starting from the southern section going north.
        else if (colSpawn == true){
            for(int r = 0; r < heroSpawnSize; r++ ){
                for(int c = 0; c < (int)map_size.x; c++){
                    heroSpawn.Add(map.Get_Tile(tile_num));
                    tile_num += 1;
                }
            }
        }
    }

    //assigns spawns to tiles from the northern section towards the south or
    //from the western section going towards the east.
    private void VillainSpawnPoints(){
        int tile_num = 0;

        //Fills the map horizontally with spawns starting from the western section going east.
        if(rowSpawn == true){
            for(int r = 1; r <= (int)map_size.y; r++){
                for(int c = 0; c < villainSpawnSize; c++){
                     villainSpawn.Add(map.Get_Tile(tile_num + c));
                }

                 tile_num = (int)map_size.x * r;
            }
        }

        //Fills the map vertically with spawns starting from the northern section going south.
        else if (colSpawn == true){

            tile_num = ((int)map_size.x * (int)map_size.y)-1;

            for(int r = 0; r < villainSpawnSize; r++){
                for(int c = 0; c < (int)map_size.x; c++ ){
                    //Debug.Log("tile_num: " + tile_num + "\nmap_size.x: " + (map_size.x));
                    villainSpawn.Add(map.Get_Tile(tile_num));
                    tile_num -= 1;
                }
            }
        }
    }

    private void SpawnPortals(){
        int numPortalSpawns = numHeroSpawns;
        int randomSpawnLoc = Random.Range(0, villainSpawn.Count);

        Tile_Content portalTile;
        List<Tile_Content> spawnTileList = new List<Tile_Content>();

        //
        if(rowSpawn == true){

            for(int i = 0; i < map_size.y; i++){
                spawnTileList.Add(portalTile = villainSpawn[i * villainSpawnSize]);
            }
        }

        else{

            for (int i = 0; i < villainSpawnSize; i++){
                spawnTileList.Add(portalTile = villainSpawn[i]);
            }
            
        }
        
        for(int s = 0; s < numPortalSpawns; s++){
            
            // select spawn location at random from the internal list.
            int randomSpawn = Random.Range(0, spawnTileList.Count);
            Vector2 spawnTile = spawnTileList[randomSpawn].mapPosition;
            Portal portal = portalFactory.GetPortal();
            spawnTileList[randomSpawn].portal = portal;
            portalList.Add(spawnTileList[randomSpawn]);
            Transform portalPosition = portal.GetComponent<Transform>();
            spawnTileList[randomSpawn].portal = portal.GetComponent<Portal>();
            portalPosition.SetParent(transform, false);
            portalPosition.localPosition = new Vector3(spawnTile.x, 1f, spawnTile.y);

            // add portal location to global list and remove location from internal list.
            portalSpawn.Add(spawnTileList[randomSpawn]);
            spawnTileList.RemoveAt(randomSpawn);
        }

        spawnTileList.Clear();
    }

    // Spawns villains on the map avoiding any duplicate locations and within the specified spawn locations for villains
    // Bugs: Villains spawn ontop of Portals.
    private void SpawnVillains(){
        //determine number of spawns
        numVillainSpawns = ((int)map_size.y * (int)map_size.x) / 16;
        Tile_Content spawnPortalTile;
        Tile_Content spawnVillainTile;
        List<Tile_Content> spawnTileList = new List<Tile_Content>();
        List<Tile_Content> validVillainSpawns = new List<Tile_Content>();

        int tile;

        Vector2 spawnTile;

        for (int v = 0; v < numVillainSpawns; v++)
        {
            // Doesn't prevent units from spawning on portals. Fix later
            // Spawns villians around and on the portals
            if (v < portalSpawn.Count){
                
                spawnPortalTile = portalSpawn[v];
                spawnTileList = SpawnArea(villainSpawn, spawnPortalTile, 2);
                for(int t = 0; t < spawnTileList.Count; t++){
                    if(!validVillainSpawns.Contains(spawnPortalTile)){
                        validVillainSpawns.Add(spawnTileList[t]);
                    }  
                }  
            }

            // determines random locations for the remaining villains to spawn.
            // avoids repeating of spawnable tiles so every villain is on their own square
            else{
                int randomSpawnLoc = Random.Range(0, villainSpawn.Count);
                spawnVillainTile = villainSpawn[randomSpawnLoc];
                spawnTileList = SpawnArea(villainSpawn, spawnVillainTile, 2);

                for(int t = 0; t < spawnTileList.Count; t++){
                    
                    if(!validVillainSpawns.Contains(spawnTileList[t])){
                        validVillainSpawns.Add(spawnTileList[t]);
                    } 
                } 
            }

            // from the validVillainSpawnList a random spawn is selected for the unit
            // the spawn location is then removed after the unit is spawned.
            tile = Random.Range(0, validVillainSpawns.Count);
            spawnTile = validVillainSpawns[tile].mapPosition;
            UnitBase villainUnit = unitFactory.GetVillainUnit(RandomVillain());
            Transform villainPosition = villainUnit.GetComponent<Transform>();
            validVillainSpawns[tile].unit = villainUnit.GetComponent<UnitBase>();
            villainPosition.SetParent(transform, false);
            villainPosition.localPosition = new Vector3(spawnTile.x, 1f, spawnTile.y);
            validVillainSpawns.Remove(validVillainSpawns[tile]);
            
        }
        
        // x number of death knights
        // x number of warlocks
        // x number of zombies
        // x number of necromancers
    }

    private void SpawnHeroes(){
        numHeroSpawns = (int)greatest_map_length / 8;

        int randomSpawnLoc = Random.Range(0, heroSpawn.Count);
        Tile_Content spawnHeroTile = heroSpawn[randomSpawnLoc];
        List<Tile_Content> spawnTileList = SpawnArea(heroSpawn, spawnHeroTile, 2);

        int hero_type = 0;

        for(int i = 0; i < 5; i++){
            hero_type = Random.Range(1,5);
        }

        for (int s = 0; s < numHeroSpawns; s++){    
            Vector2 spawnTile = spawnTileList[s].mapPosition;
            UnitBase heroUnit = unitFactory.GetHeroUnit(RandomHero());
            Transform heroUnitPosition = heroUnit.GetComponent<Transform>();
            spawnTileList[s].unit = heroUnit.GetComponent<UnitBase>();
            heros.Add(heroUnit);
            heroUnitPosition.SetParent(transform, false);
            heroUnitPosition.localPosition = new Vector3(spawnTile.x, 1f, spawnTile.y);
        }
    }

    //Test function that checks if all units can spawn in the correct spots.
    private void test_UnitSpawn(){
        //get spawn point Hero from hero list

        SpawnHeroes();
        SpawnVillains();
        //Northern/Western units
        for(int v = 0; v < villainSpawn.Count; v++){
            Vector2 spawnVillainLocation = villainSpawn[v].mapPosition;
            UnitBase villainUnit = unitFactory.GetVillainUnit(RandomVillain());
            Transform villainPosition = villainUnit.GetComponent<Transform>();
            villainSpawn[v].unit = villainUnit.GetComponent<UnitBase>();
            villains.Add(villainUnit);
            villainPosition.SetParent(transform, false);
            villainPosition.localPosition = new Vector3(spawnVillainLocation.x, 1f, spawnVillainLocation.y);
            }
    }

    //Currently hardcoded by unit name, change so that each case returns a unit by position in the Enum.
    private VillainUnits RandomVillain(){

        int numVillainTypes = VillainUnits.GetNames(typeof(VillainUnits)).Length;
        int randomType = Random.Range(0, numVillainTypes - 1);


        switch(randomType){

            case 0:
                return VillainUnits.DeathKnight;

            case 1:
                return VillainUnits.Necromancer;

            case 2:
                return VillainUnits.SkeletonArcher;

            case 3:
                return VillainUnits.Zombie;

            default:
                return VillainUnits.Generic;
        }
    }

    //Currently hardcoded by unit name, change so that each case returns a unit by position in the Enum.
    private HeroUnits RandomHero(){

        int numHeroTypes = HeroUnits.GetNames(typeof(HeroUnits)).Length;
        int randomType = Random.Range(0, numHeroTypes - 1);

        switch(randomType){

            case 0:
                return HeroUnits.Archer;
            
            case 1:
                return HeroUnits.DragonHunter;

            case 2:
                return HeroUnits.Explorer;

            case 3:
                return HeroUnits.Knight;
            
            case 4:
                return HeroUnits.Wizard;

            default:
                return HeroUnits.Generic;
        }
        
    }

    /*  Usage: SpawnArea(List<Tile_Content> side, Tile_Content center, int radius)
        Varables:
            List<Tile_Content> side: list of valid spawn locations for your side
            Tile_Content center: center of the area you want to place spawnable units
            int radius: the radius of the area you want to place spawnable units
        Return:
            List<Tile_Content> of valid spawn locations meeting provided parameters */
    private List<Tile_Content> SpawnArea(List<Tile_Content> side, Tile_Content center, int radius){
        List<Tile_Content> output = new List<Tile_Content>();
        List<int> reference = new List<int>();
        List<int> viable = new List<int>();
        int origin = center.arrayPosition;
        float center_x = center.mapPosition.x;

        // convert valid spawn tile list into easily referencable integer list
        foreach(Tile_Content location in side){reference.Add(location.arrayPosition);}

        // checks all arrayPositions that could be in radius
        for (int horizontal = radius; horizontal >= (0-radius); horizontal--){
            int offset = radius - Mathf.Abs(horizontal);
            for (int vertical = offset; vertical >= 0-offset; vertical--){
                // checks if current item is a viable spawn location and adds it
                int current = origin + (vertical*map.x) + horizontal;
                float current_x = center_x + (float)horizontal;
                if(reference.Contains(current) && map.min.x <= current_x && map.max.x >= current_x){
                    viable.Add(current);
                }
            }
        }
        // converts spawnable integer list into useable tile list
        foreach(int spawn in viable){output.Add(map.tile_array[spawn]);}

        return output;
    }
}