using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Card Game Sample")]
    public class HealNode : ProcessNode
    {
        public Inport<IReadOnlyList<Unit>> targetsPort;
        public Inport<int> valuePort;

        protected override AbilityState DoLogic()
        {
            var targets = targetsPort.GetValue();
            if (targets.Count == 0)
            {
                return AbilityState.RUNNING;
            }

            var value = valuePort.GetValue();
            for (var i = 0; i < targets.Count; i++)
            {
                targets[i].Attribute.HP += value;
            }
            /*
            EnqueueEvent(new HealEvent
            {
                targets = targets,
                amount = value,
            });
            */
            return AbilityState.RUNNING;
        }
    }
}
