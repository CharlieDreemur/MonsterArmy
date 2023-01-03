using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Set in the Inspector")]
    public UnityEngine.GameObject Prefab_grid;
    public Vector3 centerPos;
    public int width;
    public int height;
    public int size;
    private FightGrid<HeatMapGridObject> grid;

    void Start()
    {
        grid = new FightGrid<HeatMapGridObject>(10,10,4f, Vector3.zero, (FightGrid<HeatMapGridObject> g, int x, int y)=>new HeatMapGridObject(g,x,y));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Vector3 pos = UtilsClass.GetMouseWorldPosition2D();
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(pos);
            if(heatMapGridObject != null){
                heatMapGridObject.AddValue(5);
            }

    }
    }
   
}

public class HeatMapGridObject{
    private const int MIN = 0;
    private const int MAX = 100;
    private FightGrid<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;
    public HeatMapGridObject(FightGrid<HeatMapGridObject> _grid, int _x, int _y){
        grid = _grid;
        x = _x;
        y = _y;
    }

    public void AddValue(int _value){
        value +=_value;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized(){
        return (float)value/MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
