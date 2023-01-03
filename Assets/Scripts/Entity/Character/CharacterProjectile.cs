using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有管理projectile的public方法
public class CharacterProjectile:MonoBehaviour
{
  //public static Transform parent;
  ObjectPooler objectPooler;
  void Awake() {
    //parent = gameObject.transform;
  }

  private void Start() {
    objectPooler = ObjectPooler.Instance;
  }
  void OnEnable() {
    ICharacter.Event_Shoot+=Event_OnShoot;
  }

  void OnDisable() {
    ICharacter.Event_Shoot-=Event_OnShoot;
  }
  
  //当检测到射击事件时
  private static void Event_OnShoot(object sender, Character_OnShoot e){
    Vector3 pos = e.GetAttacker().GetCharacterData().relativePos;
    if(e.GetAttacker().characterDirection == Enum_CharacterDirection.right){
       e.SetProjectileBirthPos(e.GetAttackerPosition()+ pos);
    }
    else{
       e.SetProjectileBirthPos(e.GetAttackerPosition() + new Vector3(-pos.x, pos.y, pos.z));
    }
        UnityEngine.GameObject bullet = ObjectPooler.Instance.Spawn(e.GetProjectileData(), e.GetProjectileBirthPos(), Quaternion.identity);

    bullet.GetComponent<Projectile>().Init(e); //初始化
    bullet.GetComponent<Projectile>().OnObjectSpawn(); //启动
    
  }

   
}

public class Character_OnShoot:EventArgs{

  private ICharacter attacker; //子弹射击者
  private ICharacter target; //子弹目标
  private Vector3 projectileBirthPos; //子弹出生位置
  private ProjectileData projectileData;//子弹数据
  public Character_OnShoot(ICharacter _attacker, ICharacter _target, ProjectileData _projectileData){
    attacker = _attacker;
    target = _target;
    projectileData = _projectileData;
  }

  public ICharacter GetAttacker(){
    return attacker;
  }

  public ICharacter GetTarget(){
    return target;
  }

  public Vector3 GetAttackerPosition(){
    return attacker.GetPosition();
  }

  public Vector3 GetTargetPosition(){
    return target.GetPosition();
  }

  public ProjectileData GetProjectileData(){
    return projectileData;
  }

  public Vector3 GetProjectileBirthPos(){
    return projectileBirthPos;
  }
  
  public void SetProjectileBirthPos(Vector3 _projectileBirthPos){
    projectileBirthPos = _projectileBirthPos;
  }

}
