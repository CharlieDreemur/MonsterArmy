using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MonsterArmy.UI
{
    public class DamageText : MonoBehaviour, IPoolObject
    {
        public DamageTextData data;
        private TextMeshPro textMesh;
        private Color color;
        private int damageAmount;
        private Enum_DamageType damageType;
        private bool isCrit;
        private float disappearTimer;
        private Vector3 moveVector;

        private static int sortingOrder; //渲染层级，确保后生产的text会在上层

        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();

        }

        public void OnSpawn()
        {
            textMesh.SetText(damageAmount.ToString());
            switch (damageType)
            {
                case Enum_DamageType.normal:
                    textMesh.fontSize = data.noramlFontSize;
                    textMesh.color = data.normalColor;
                    break;
                case Enum_DamageType.crit:
                    //暴击字体 TODO:更多暴击特效
                    textMesh.fontSize = data.critFontSize;
                    textMesh.color = data.critColor;
                    break;
                case Enum_DamageType.heal:
                    textMesh.fontSize = data.healFontSize;
                    textMesh.color = data.healColor;
                    break;
                case Enum_DamageType.ability:
                    textMesh.fontSize = data.abilityFontSize;
                    textMesh.color = data.abilityColor;
                    break;
                default:
                    Debug.LogWarning("DamageText.OnObjectSpawn()未知的Enum_DamageType");
                    break;
            }
            color = textMesh.color;
            disappearTimer = data.disappearTime;
            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;
            moveVector = new Vector3(0.7f, 1) * data.moveSpeed;
            transform.localScale = Vector3.one;
        }

        public void OnRelease()
        {
            PoolManager.Release(gameObject);
        }
        public void Init(DamageTextData data, int damageAmount, Enum_DamageType damageType)
        {
            if (data == null)
            {
                Debug.LogWarning("没有提供伤害跳字的数据");
                return;
            }
            this.data = data;
            disappearTimer = data.disappearTime;
            this.damageType = damageType;
            this.damageAmount = damageAmount;

        }

        public void Update()
        {
            Run();
        }

        public void Run()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 8f * Time.deltaTime;
            if (disappearTimer > data.disappearTime * 0.5f)
            {
                //First hald of the damageText lifetime
                float increaseScaleAmout = 1f;
                transform.localScale += Vector3.one * increaseScaleAmout * Time.deltaTime;
            }
            else
            {
                //Second half of the damageText lifetime
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;

            }

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0)
            {
                //Start disappearing
                color.a -= data.disappearSpeed * Time.deltaTime;
                textMesh.color = color;
                if (color.a < 0)
                {
                    OnRelease();

                }
            }
        }
    }
}