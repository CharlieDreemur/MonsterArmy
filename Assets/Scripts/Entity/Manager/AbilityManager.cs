using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Physalia.Flexi;
namespace MonsterArmy.Core
{
    public class AbilityManager : MonoBehaviour
    {
        private AbilitySystem _abilitySystem;
        void Awake()
        {
            // Use the builder class to build the AbilitySystem
            _abilitySystem = new AbilitySystemBuilder().Build();
        }
        /*
                private Unit CreateUnit()
                {

                    // Unit may have abilities at start.
                    IReadOnlyList<SkillData> skills = unitData.Skills;
                    for (var i = 0; i < skills.Count; i++)
                    {
                        AbilityData abilityData = skills[i].AbilityAsset.Data;

                        // Normally you only need to append for index 0, below is just example.
                        for (var groupIndex = 0; groupIndex < abilityData.graphGroups.Count; groupIndex++)
                        {
                            // Save the source data to tell that this actor has this ability.
                            AbilityDataSource abilityDataSource = abilityData.CreateDataSource(groupIndex);
                            var container = new AbilityDataContainer { DataSource = abilityDataSource };
                            unit.AppendAbilityDataContainer(container);

                            // Explicitly cache the abilities in pools if necessary.
                            // Normally you should because instantiating abilities is really expansive.
                            if (!_abilitySystem.HasAbilityPool(abilityDataSource))
                            {
                                _abilitySystem.CreateAbilityPool(abilityDataSource, 2);
                            }
                        }
                    }

                    return unit;
                }
                */
    }
}