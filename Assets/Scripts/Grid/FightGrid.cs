using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGrid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs:EventArgs{
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private float size;
    private Vector3 centerPos;
    private TGridObject [,] gridArray;
    private TextMesh[,] debugTextArray;

    public FightGrid(int _width, int _height, float _size, Vector3 _centerPos, Func<FightGrid<TGridObject>, int, int, TGridObject> createGridObject){
        width = _width;
        height = _height;
        size = _size;
        centerPos = _centerPos;
        gridArray = new TGridObject[width, height];

        for(int x=0; x<gridArray.GetLength(0); x++){
                for(int y=0; y<gridArray.GetLength(1); y++){
                   gridArray[x, y] = createGridObject(this, x, y);
                }
            }    
        
        bool showDebug = true;
        if(showDebug){
            debugTextArray = new TextMesh[width, height];
            for(int i=0; i<gridArray.GetLength(0); i++){
                for(int j=0; j<gridArray.GetLength(1); j++){
                    debugTextArray[i,j] = UtilsClass.CreateWorldText(gridArray[i,j]?.ToString(), null, GetWorldPosition(i,j)+new Vector3(size, size) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i+1, j), Color.white, 100f); 
                    Debug.DrawLine(GetWorldPosition(i,j), GetWorldPosition(i, j+1), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width, height), Color.white, 100f);
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
       
    }
    public int GetWidth(){
        return width;
    }
    
    public int GetHeight(){
        return height;
    }

    public float GetSize(){
        return size;
    }

    private Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x,y)*size + centerPos;
    }

    public Vector2 GetXY(Vector3 worldPos){
        return new Vector2(Mathf.FloorToInt((worldPos-centerPos).x / size), Mathf.FloorToInt((worldPos-centerPos).y / size));
    }

    public void GetXY(Vector3 worldPos, out int x, out int y){
        x = Mathf.FloorToInt((worldPos-centerPos).x / size);
        y = Mathf.FloorToInt((worldPos-centerPos).y / size);
    }

    public void SetGridObject(int x, int y, TGridObject value){
        if(x>=0 && y>=0 && x<width && y<height){
            gridArray[x,y] = value;
            if(OnGridObjectChanged !=null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs{x=x, y=y});
        }
    }

    public void TriggerGridObjectChanged(int x, int y){
        if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs{x=x, y=y});
    }

    public void SetGridObject(Vector3 worldPos, TGridObject value){
        Vector2 xy = GetXY(worldPos);
        SetGridObject((int)xy.x, (int)xy.y, value);
    }

    public TGridObject GetGridObject(int x, int y){
        if(x>=0 && y>=0 && x<width && y < height){
            return gridArray[x,y];
        }   else{
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPos){
        Vector2 xy = GetXY(worldPos);
        return GetGridObject((int)xy.x,(int)xy.y);
    }
}


