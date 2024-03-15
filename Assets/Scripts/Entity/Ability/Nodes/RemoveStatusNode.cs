using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Status Node")]
    public class RemoveStatusNode : ProcessNode
    {
        public Inport<UnitManager> gamePort;
        public Inport<IReadOnlyList<Unit>> targetsPort;
        public Inport<int> stackPort;
        public Variable<int> statusId;

        protected override AbilityState DoLogic()
        {
            UnitManager game = gamePort.GetValue();
            IReadOnlyList<Unit> targets = targetsPort.GetValue();
            int stack = stackPort.GetValue();

            for (var i = 0; i < targets.Count; i++)
            {
                //game.RemoveStatus(targets[i], statusId.Value, stack);
            }

            return AbilityState.RUNNING;
        }
    }
}
