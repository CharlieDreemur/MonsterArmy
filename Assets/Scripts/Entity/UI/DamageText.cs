using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MonsterArmy.Core;

namespace MonsterArmy.UI
{
    public class DamageText : MonoBehaviour, IPoolObject
    {
        public DamageTextData data;
        private TextMeshPro textMesh;
        private Color color;
        private float disappearTimer;
        private Vector3 moveVector;
        private CreateDamageTextEventArgs args;

        private static int sortingOrder; //渲染层级，确保后生产的text会在上层

        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }

        public void OnSpawn()
        {
            textMesh.SetText(args.damageAmount.ToString());
            switch (args.damageType)
            {
                case Enum_DamageType.PhysicalDamage:
                    if (args.isCrit)
                    {
                        textMesh.fontSize = data.physicalCritFontSize;
                        textMesh.color = data.physicalCritColor;
                        break;
                    }
                    textMesh.fontSize = data.noramlFontSize;
                    textMesh.color = data.normalColor;
                    break;
                case Enum_DamageType.Heal:
                    textMesh.fontSize = data.healFontSize;
                    textMesh.color = data.healColor;
                    break;
                case Enum_DamageType.MagicDamage:
                    if(args.isCrit)
                    {
                        textMesh.fontSize = data.magicCritFontSize;
                        textMesh.color = data.magicCritColor;
                        break;
                    }
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
        public void Init(DamageTextData data, CreateDamageTextEventArgs args)
        {
            if (data == null)
            {
                Debug.LogWarning("没有提供伤害跳字的数据");
                return;
            }
            this.data = data;
            disappearTimer = data.disappearTime;
            this.args = args;

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