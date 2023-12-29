using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//所有管理projectile的public方法
public class ProjectileManager: Singleton<ProjectileManager>, IManager
{
  //public static Transform parent;

  private UnityAction<string> instantiateProjectileAction;
  protected override void OnAwake() {
    instantiateProjectileAction = new UnityAction<string>(CreateProjectile);
  }

  public void Init(){
    EventManager.StartListening("InstantiateProjectile", instantiateProjectileAction);
  }

  void OnDisable() {
    EventManager.StopListening("InstantiateProjectile", instantiateProjectileAction);
  }
  
  public static void CreateProjectile(string jsonValue){
    ProjectileArgs args = JsonUtility.FromJson<ProjectileArgs>(jsonValue);
    CreateProjectile(args);
  }
  //当检测到射击事件时
  private static void CreateProjectile(ProjectileArgs args){
    UnityEngine.GameObject bullet = Projectile.InstantiateProjectile(args);
  }

   
}
