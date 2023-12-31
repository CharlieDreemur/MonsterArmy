using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterEnemy.SkillNodeSystem
{
    [NodeCategory("MonsterEnemy")]
    public class AllEnemiesNode : ValueNode
    {
        public Inport<UnitManager> gamePort;
        public Outport<List<Unit>> resultPort;

        private readonly List<Unit> resultCache = new();

        protected override void EvaluateSelf()
        {
            resultCache.Clear();
            UnitManager unitManager = gamePort.GetValue();
            resultCache.AddRange(unitManager.List_Enemy);
            resultPort.SetValue(resultCache);
        }
    }
}
