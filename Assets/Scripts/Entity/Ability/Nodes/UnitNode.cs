using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Selection Node")]
    public class UnitNode : ValueNode
    {
        public Outport<Unit> unitPort;

        protected override void EvaluateSelf()
        {
            //unitPort.SetValue(Actor as Unit);
        }
    }
}
