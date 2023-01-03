
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
   
    [Header("Set in the Inspector")]
    public UnityEngine.GameObject testPrefab;
    [Header("Set Dynamically")]
    public string fileName = "Test";

    private Pathfinding pathfinding;
    public void Awake(){
       
    }
    public void Start(){
        pathfinding = new Pathfinding(16,9, 4, new Vector3(-32, -18));
    }
    public void Update() {
        
        if(Input.GetMouseButtonDown(0)){
            Vector3 mouseWorldPos = UtilsClass.GetMouseWorldPosition2D();
            Vector2 endPos = pathfinding.GetGrid().GetXY(mouseWorldPos);
            List<PathNode> path = pathfinding.FindPath(0,0,(int)endPos.x, (int)endPos.y);
            if(path!=null){
                for(int i=0; i<path.Count-1;i++){
                    Debug.Log(path[i]);
                    Debug.DrawLine(new Vector3(path[i].x,path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i+1].x, path[i+1].y)*10f +Vector3.one *5f, Color.green );
                }
            }
        }

        if(Input.GetMouseButtonDown(1)){
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition2D();
            Vector2 endPos = pathfinding.GetGrid().GetXY(mouseWorldPosition);
            pathfinding.GetNode((int)endPos.x, (int)endPos.y).SetIsWalkable(!pathfinding.GetNode((int)endPos.x, (int)endPos.y).isWalkable);
        }
    }
   
}
