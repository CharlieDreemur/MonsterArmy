using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;
namespace MonsterArmy.Core.Unit
{
    [System.Serializable]
    //For import yaml config only
    public struct UnitAttributeConfig
    {
        public string Name;
        public string Description;
        public int MAXHP;
        public int MAXMP;
        public int ATK;
        public int DEF;
        public int AP;
        public float ATKSPD; //Default 0
        public float MOVSPD; //Default 2
        public float ATKRange; //Default 5
        public float CritChance;
        public float CritDamage; //Default 1.5
        public float DodgeChance;
    }


}