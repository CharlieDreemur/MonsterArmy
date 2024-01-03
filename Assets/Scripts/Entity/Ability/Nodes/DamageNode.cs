using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Basic Node")]
    public class DamageNode : ProcessNode
    {
        public Inport<Unit> attackerPort;
        public Inport<IReadOnlyList<Unit>> targetsPort;
        public Inport<int> valuePort;

        protected override AbilityState DoLogic()
        {
            var attacker = attackerPort.GetValue();
            var targets = targetsPort.GetValue();
            if (targets.Count == 0)
            {
                return AbilityState.RUNNING;
            }

            var value = valuePort.GetValue();
            for (var i = 0; i < targets.Count; i++)
            {
                targets[i].TakeDamage(attacker, value);
            }
            /*
            EnqueueEvent(new DamageEvent
            {
                attacker = attacker,
                targets = new List<Unit>(targets),
                amount = value,
            });
            */
            return AbilityState.RUNNING;
        }
    }
}
