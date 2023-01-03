using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode  {
    private FightGrid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public PathNode cameFromNode;
    public PathNode(FightGrid<PathNode> _grid, int _x, int _y){
        grid = _grid;
        x = _x;
        y = _y;
        isWalkable = true;
    }

    public void CalculateFCost(){
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool _isWalkable){
        isWalkable = _isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString(){
        return x + "," + y;
    }
}