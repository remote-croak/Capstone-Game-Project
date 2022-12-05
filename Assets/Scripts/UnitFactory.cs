using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitFactory : ObjectFactory
{

    // Hero Units
    [SerializeField] UnitBase genericHeroPrefab = default;
    [SerializeField] UnitBase archerPrefab = default;
    [SerializeField] UnitBase dragonHunterPrefab = default;
    [SerializeField] UnitBase explorerPrefab = default;
    [SerializeField] UnitBase knightPrefab = default;
    [SerializeField] UnitBase wizardPrefab  = default;
    
    // Villain Units
    [SerializeField] UnitBase genericVillainPrefab = default;
    [SerializeField] UnitBase deathKnightPrefab = default;
    [SerializeField] UnitBase necromancerPrefab = default;
    [SerializeField] UnitBase skeletonArcherPrefab = default;
    [SerializeField] UnitBase zombiePrefab = default;

    public UnitBase GetHeroUnit(HeroUnits unitType){
        switch (unitType){
            case HeroUnits.Generic: return GetUnitPrefab(genericHeroPrefab);
            case HeroUnits.Archer: return GetUnitPrefab(archerPrefab);
            case HeroUnits.DragonHunter: return GetUnitPrefab(dragonHunterPrefab);
            case HeroUnits.Explorer: return GetUnitPrefab(explorerPrefab);
            case HeroUnits.Knight: return GetUnitPrefab(knightPrefab);
            case HeroUnits.Wizard: return GetUnitPrefab(wizardPrefab);
        }
        
        Debug.Assert(false, "Nonexistant Hero Unit: " + unitType);
        return null;
    }

    public UnitBase GetVillainUnit(VillainUnits unitType){
        switch (unitType){
            case VillainUnits.Generic: return GetUnitPrefab(genericVillainPrefab);
            case VillainUnits.DeathKnight: return GetUnitPrefab(deathKnightPrefab);
            case VillainUnits.Necromancer: return GetUnitPrefab(necromancerPrefab);
            case VillainUnits.SkeletonArcher: return GetUnitPrefab(skeletonArcherPrefab);
            case VillainUnits.Zombie: return GetUnitPrefab(zombiePrefab);
        }
        
        Debug.Assert(false, "Nonexistant Villain Unit: " + unitType);
        return null;
    }

    public UnitBase GetUnitPrefab(UnitBase prefab){
        UnitBase instance = CreateObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim (UnitBase unit){
        Debug.Assert(unit.OriginFactory == this, "Wrong Factory Reclaimed");
        Destroy(unit.gameObject);
    }

}