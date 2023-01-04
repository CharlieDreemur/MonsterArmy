using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//所有管理projectile的public方法
public class ProjectileManager: Singleton<ProjectileManager>, IManager
{
  //public static Transform parent;
  ObjectPooler objectPooler;
  private UnityAction<string> instantiateProjectileAction;
  protected override void OnAwake() {
    instantiateProjectileAction = new UnityAction<string>(CreateProjectile);
    objectPooler = ObjectPooler.Instance;
  }

  public void Init(){
    EventManager.StartListening("InstantiateProjectile", instantiateProjectileAction);
  }

  void OnDisable() {
    EventManager.StopListening("InstantiateProjectile", instantiateProjectileAction);
    Debug.Log("ProjectileManager OnDisable");
  }
  
  public static void CreateProjectile(string jsonValue){
    ProjectileArgs args = JsonUtility.FromJson<ProjectileArgs>(jsonValue);
    CreateProjectile(args);
  }
  //当检测到射击事件时
  private static void CreateProjectile(ProjectileArgs e){
    Vector3 pos = e.attacker.GetCharacterData().relativePos;
    if(e.attacker.characterDirection == Enum_CharacterDirection.right){
       e.projectileBirthPos = e.GetAttackerPosition()+ pos;
    }
    else{
       e.projectileBirthPos = e.GetAttackerPosition() + new Vector3(-pos.x, pos.y, pos.z);
    }
        UnityEngine.GameObject bullet = ObjectPooler.Instance.Spawn(e.projectileData, e.projectileBirthPos, Quaternion.identity);

    bullet.GetComponent<Projectile>().Init(e); //初始化
    bullet.GetComponent<Projectile>().OnObjectSpawn(); //启动
    
  }

   
}

public class ProjectileArgs:EventArgs{

  public Entity attacker; //子弹射击者
  public Entity target; //子弹目标
  public Vector3 projectileBirthPos; //子弹出生位置
  public ProjectileData projectileData;//子弹数据
  public ProjectileArgs(Entity _attacker, Entity _target, ProjectileData _projectileData){
    attacker = _attacker;
    target = _target;
    projectileData = _projectileData;
  }
 
  public Vector3 GetAttackerPosition(){
    return attacker.GetPosition();
  }

  public Vector3 GetTargetPosition(){
    return target.GetPosition();
  }


}
