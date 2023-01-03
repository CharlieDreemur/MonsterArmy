using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VFXUtils
{
    
    public static void ActivateVFX(List<VFX> List_VFX, Vector3 charPos){

        ActivateVFX(List_VFX, charPos, null);
        
    }


    public static void ActivateVFX(List<VFX> List_VFX, Vector3 charPos, List<Entity> List_Target){
        if(List_VFX.Count == 0 || List_VFX == null){
            return;
        }
        for(int i = 0; i<List_VFX.Count;i++){
            switch(List_VFX[i].locationType){
                case Enum_VFXLocation.self:
                {
                        UnityEngine.GameObject spawnedVFX = List_VFX[i].spawnVFX();
                    spawnedVFX.transform.position = charPos;
                    spawnedVFX.transform.position += List_VFX[i].relativeposition;
                    break;
                }
                case Enum_VFXLocation.target:
                {
                    //如果技能目标数大于1，则对每个目标都施加特效
                    if(List_Target.Count>1){
                            List<UnityEngine.GameObject> List_spawnedVFX = new List<UnityEngine.GameObject>();
                        for(int number = 0; number<List_Target.Count; number++){
                                UnityEngine.GameObject spawnedVFX = List_VFX[i].spawnVFX();
                            spawnedVFX.transform.position = List_Target[number].GetPosition();
                            spawnedVFX.transform.position += List_VFX[i].relativeposition;
                            List_spawnedVFX.Add(spawnedVFX);
                        }
                    }
                    else{
                            UnityEngine.GameObject spawnedVFX = List_VFX[i].spawnVFX();
                        spawnedVFX.transform.position = List_Target[0].GetPosition();
                        spawnedVFX.transform.position += List_VFX[i].relativeposition;
                    }
    
                    break;
                }
            }
        }
    }
}
