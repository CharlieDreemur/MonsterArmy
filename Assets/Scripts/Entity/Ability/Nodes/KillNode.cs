using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Card Game Sample")]
    public class KillNode : ProcessNode
    {
        public Inport<IReadOnlyList<Unit>> targetsPort;

        protected override AbilityState DoLogic()
        {
            var targets = targetsPort.GetValue();
            if (targets.Count == 0)
            {
                return AbilityState.RUNNING;
            }

            for (var i = 0; i < targets.Count; i++)
            {
                int health = targets[i].Attribute.HP;
                if (health <= 0)
                {   
                    /*
                    EnqueueEvent(new DeathEvent
                    {
                        target = targets[i],
                    });
                    */
                }
            }

            return AbilityState.RUNNING;
        }
    }
}
