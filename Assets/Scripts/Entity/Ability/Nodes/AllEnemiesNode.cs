using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterArmy.SkillNodes
{
    [NodeCategory("Selection Node")]
    public class AllEnemiesNode : ValueNode
    {
        public Inport<UnitManager> gamePort;
        public Variable<Enum_UnitType> unitTypePort;
        public Outport<List<Unit>> resultPort;

        private readonly List<Unit> resultCache = new();

        protected override void EvaluateSelf()
        {
            resultCache.Clear();
            UnitManager unitManager = gamePort.GetValue();
            if(unitTypePort == Enum_UnitType.Ally)
            {
                resultCache.AddRange(unitManager.List_Enemy);
            }
            else
            {
                resultCache.AddRange(unitManager.List_Ally);
            }
            resultPort.SetValue(resultCache);
        }
    }
}
