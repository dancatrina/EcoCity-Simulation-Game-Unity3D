using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private int x;
    private int y;

    private Building building;

    //Cladirea pe care urmeaza sa o amplasez private (Builing)
    
    public GridObject(int x, int y)
    {
        this.x = x; 
        this.y = y;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public Building getBuilding() => building;
    public bool canBuild() => building == null;

    public void setBuilding(Building newBuilding)
    {
        building = newBuilding;
        BuildingSystem.getInstance().getGrid().triggerGridObjectChanged(x, y);
    }

    public void removeBuilding()
    {
        building = null;
        BuildingSystem.getInstance().getGrid().triggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return "X = " + x + ", Y = " + y + ", OBJ = " + building;
    }


}
