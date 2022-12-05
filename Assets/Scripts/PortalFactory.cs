using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PortalFactory : ObjectFactory{

    [SerializeField] Portal exitPortal = default;

    public Portal GetPortal(){
        Portal instance = CreateObjectInstance(exitPortal);
        instance.OriginFactory = this;
        return instance;
    }
    
    public void Reclaim (Portal portal){
        Debug.Assert(portal.OriginFactory == this, "Wrong Factory Reclaimed");
        Destroy(portal.gameObject);
    }
}