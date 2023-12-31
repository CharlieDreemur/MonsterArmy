using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterArmy.Core.UnitSystem.Interface
{
    public interface IUnitComponent
    {
        void Init(IUnitComponentInitData initData);
    }
    public interface IAttackComponent:IUnitComponent{
        void TryAttack(Unit attacker, Unit target);
    }

    public interface IUnitComponentInitData{

    }

    //Default init data, null
    public class DeafultUnitComponentInitData : IUnitComponentInitData
    {
        //Left empty
    }
}