using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Selection Node")]
    public class RandomPickNode : ValueNode
    {
        public Inport<IReadOnlyList<Unit>> sourcePort;
        public Outport<Unit> resultPort;

        protected override void EvaluateSelf()
        {
            IReadOnlyList<Unit> source = sourcePort.GetValue();
            //var payload = GetPayload<PlayCardNode.Payload>();
            //Unit unit = source.RandomPickOne(payload.random);
            //resultPort.SetValue(unit);
        }
    }
}
