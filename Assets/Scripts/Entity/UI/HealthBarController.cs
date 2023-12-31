using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MonsterArmy.Core.UnitSystem.Interface;
using MonsterArmy.Core.UnitSystem.Data;
namespace MonsterArmy.UI
{
    public class HealthBarController : MonoBehaviour, IUnitComponent
    {
        public HealthBarData data;
        public Unit character;
        [SerializeField]
        private UnityEngine.GameObject healthBarPrefab;
        [SerializeField]
        private HealthBar healthBar;
        private void Start()
        {
            character = gameObject.GetComponent<Unit>();

            if (character == null)
            {
                return;
            }
            healthBarPrefab = data.Create();
            healthBarPrefab.transform.SetParent(character.transform, false);
            healthBar = healthBarPrefab.GetComponent<HealthBar>();
            healthBar.Init(character);

        }
        public void Update()
        {
            if (character == null)
            {
                return;
            }
            healthBar.Update();
        }

        public void Init(IUnitComponentInitData data)
        {
            this.data = data as HealthBarData;
        }
    }
}