using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Card Game Sample")]
    public class StatusStackNode : ValueNode
    {
        public Inport<UnitManager> gamePort;
        public Inport<Unit> unitPort;
        public Outport<int> stackPort;
        public Variable<int> statusId;

        protected override void EvaluateSelf()
        {
            UnitManager game = gamePort.GetValue();
            Unit unit = unitPort.GetValue();
            //int stack = game.GetUnitStatusStack(unit, statusId.Value);
            //stackPort.SetValue(stack);
        }
    }
}
