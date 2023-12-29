using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterArmy.Core.UnitSystem.Components
{
    public interface IUnitComponent
    {

    }
    public interface IAttackComponent:IUnitComponent{
        void TryAttack(Unit attacker, Unit target);
    }
}