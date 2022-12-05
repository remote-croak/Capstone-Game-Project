using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{

    [SerializeField] Vector2Int surfaceSize = new Vector2Int(0,0);
    [SerializeField] Build_Surface levelMap = default;
    [SerializeField] Tile_Factory tileContentFactory = default;
    [SerializeField] UnitFactory unitFactory = default;
    [SerializeField] PortalFactory portalFactory = default;

    [SerializeField] Spawn spawn = default;

    [SerializeField] UnitControl control;

    Ray TouchRay => Camera.main.ScreenPointToRay (Input.mousePosition);

    bool heroTurn = true;

    void Awake(){
        // set seed for random functions, only accepts ints
        //Random.InitState();

        Random_Map_Size();
        //Vector2Int test_surfaceSize = new Vector2Int(35,52);
        Vector2Int test_surfaceSize = new Vector2Int(8,16);
        //Vector2Int test_surfaceSize = new Vector2Int(16,16);   
        levelMap.Initialize(test_surfaceSize, tileContentFactory); // test initialize values
        //level_map.Initialize(surfaceSize, tileContentFactory);
        //level_map.ShowGrid = true;
        spawn.HeroesVillains(test_surfaceSize, unitFactory, portalFactory); //test spawn
        //spawn.HeroesVillains(surfaceSize, unitFactory);
    }

    private void Random_Map_Size(){
        surfaceSize.x = Random.Range(16,64);
        surfaceSize.y = Random.Range(16,64);
    }

    public Vector2 Get_Map_Dimensions(){
        return surfaceSize;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(heroTurn){
            if(Input.GetKeyDown(KeyCode.Q)){
                EndTurn();
            }
            if(Input.GetKeyDown(KeyCode.G)){
                //level_map.ShowGrid = !level_map.ShowGrid;
            }
            if(Input.GetMouseButtonDown(0)){
                HandleTouch();
            }
        }
        // this is where the enemy AI needs to be activated
        else{
            Debug.Log("Enemy Turn");
            Debug.Log(Score());
            Debug.Log(Victory());
            EndTurn();
        }
    }

    // flips boolean for turns and resets turn states for units
    void EndTurn(){
        if (heroTurn){foreach (UnitBase unit in spawn.heros){unit.phase = 0;}}
        //else {foreach (UnitBase unit in spawn.villains){unit.phase = 0;}}
        heroTurn = !heroTurn;
    }

    // Checks for what gameObject has been clicked on and highlights it accordingly
    void HandleTouch(){
        RaycastHit hit;
        if (Physics.Raycast(TouchRay, out hit)){
            control.Select(hit.collider.gameObject.GetComponent<Tile_Content>());
        }
    }

    // currently only determins total distance from portals to units, each unit and portal are counted once
    float Score(){
        List<UnitBase> heroList = new List<UnitBase>(spawn.heros);
        UnitBase closestHero;
        float distance;
        float newDistance;
        float score = 0;
        
        foreach (Tile_Content portal in spawn.portalList)
        {
            distance = DistanceCheck(portal.mapPosition, heroList[0].mapPosition);
            closestHero = heroList[0];
            foreach (UnitBase hero in heroList)
            {
                newDistance = DistanceCheck(portal.mapPosition, hero.mapPosition);
                if (portal.mapPosition.Equals(hero.mapPosition)){
                    closestHero = hero;
                    distance = 0;
                }
                else if (newDistance < distance){
                    closestHero = hero;
                    distance = newDistance;
                }
            }
            score += distance;
            // removes matched hero so the same hero can't be used for two portal measurements
            heroList.Remove(closestHero);
        }
        return score;
    }

    // determins absolute chicago distance between two grid points
    float DistanceCheck (Vector2 first, Vector2 second){
        Vector2 temp = first - second;
        return(Mathf.Abs(temp.x) + Mathf.Abs(temp.y));
    }

    // checks that each portal contains a hero unit
    bool Victory(){
        foreach (Tile_Content portal in spawn.portalList){
            if (!portal.unit || portal.unit.tag != "Hero"){return false;}
        }
        return true;
    }
}
