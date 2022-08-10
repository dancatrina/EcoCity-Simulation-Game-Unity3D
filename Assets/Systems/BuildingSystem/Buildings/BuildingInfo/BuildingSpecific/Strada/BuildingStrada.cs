using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStrada : ABuilding
{
    public BuildingStrada()
    {
        this.tip = tipCladire.Strada;
    }



    public override ABuilding clone()
    {
        return new BuildingStrada();
    }
}
