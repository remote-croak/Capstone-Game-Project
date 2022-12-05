using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour{

    PortalFactory originFactory;

    public PortalFactory OriginFactory{
        get => originFactory;
        set{
            Debug.Assert(originFactory == null, "Redfined origin factory");
            originFactory = value;
        }
    }

}