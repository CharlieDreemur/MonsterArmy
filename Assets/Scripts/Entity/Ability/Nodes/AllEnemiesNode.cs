using System.Collections.Generic;
using Physalia.Flexi;

namespace MonsterEnemy.SkillNodeSystem
{
    [NodeCategory("MonsterEnemy")]
    public class AllEnemiesNode : ValueNode
    {
        public Inport<EntityManager> gamePort;
        public Outport<List<Unit>> resultPort;

        private readonly List<Unit> resultCache = new();

        protected override void EvaluateSelf()
        {
            resultCache.Clear();
            EntityManager entityManager = gamePort.GetValue();
            resultCache.AddRange(entityManager.List_Enemy);
            resultPort.SetValue(resultCache);
        }
    }
}
