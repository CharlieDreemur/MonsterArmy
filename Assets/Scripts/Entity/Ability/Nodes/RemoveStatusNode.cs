using System.Collections.Generic;

namespace Physalia.Flexi.Samples.CardGame
{
    [NodeCategory("Card Game Sample")]
    public class RemoveStatusNode : ProcessNode
    {
        public Inport<EntityManager> gamePort;
        public Inport<IReadOnlyList<Unit>> targetsPort;
        public Inport<int> stackPort;
        public Variable<int> statusId;

        protected override AbilityState DoLogic()
        {
            EntityManager game = gamePort.GetValue();
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
