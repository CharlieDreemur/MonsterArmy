using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    public class TurnEndEvent : IEventContext
    {
        public UnitManager game;
    }


    public class ManaChangeEvent : IEventContext
    {
        public int modifyValue;
        public int newAmount;

        public override string ToString()
        {
            return $"Mana += {modifyValue} => {newAmount}";
        }
    }
    
    public class HealEvent : IEventContext
    {
        public IReadOnlyList<Unit> targets;
        public int amount;

        public override string ToString()
        {
            return $"{targets.ToContentString()} get {amount} healed";
        }
    }

    public class DamageEvent : IEventContext
    {
        public Unit attacker;
        public IReadOnlyList<Unit> targets;
        public int amount;

        public override string ToString()
        {
            return $"{targets.ToContentString()} received {amount} damage";
        }
    }

    public class UnitSpawnedEvent : IEventContext
    {
        public IReadOnlyList<Unit> units;
    }

    public class DeathEvent : IEventContext
    {
        public Unit target;
    }
}
