using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace MonsterArmy.UI
{
    [CreateAssetMenu(fileName = "DamageTextData", menuName = "TheMonsterArmy/ScriptableObject/UISystem/DamageTextData", order = 0)]
    public class DamageTextData : ScriptableObject, IPoolData
    {
        [FoldoutGroup("伤害跳字基本设置")]
        [HideLabel]
        [LabelText("跳字模型")]
        [SerializeField]
        public UnityEngine.GameObject prefab;
        public UnityEngine.GameObject Prefab { get => prefab; set => prefab = value; }

        [FoldoutGroup("伤害跳字基本设置")]
        [HideLabel]
        [LabelText("正常字体颜色")]
        public Color normalColor; //正常状态的字体颜色

        [FoldoutGroup("伤害跳字基本设置")]
        [HideLabel]
        [LabelText("正常字体大小")]
        public int noramlFontSize = 16; //正常状态的字体大小

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("暴击字体颜色")]
        public Color critColor; //暴击状态的字体颜色

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("暴击字体大小")]
        public int critFontSize = 20; //暴击状态的字体大小
        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("治疗字体颜色")]
        public Color healColor; //治疗状态的字体颜色

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("治疗字体大小")]
        public int healFontSize = 16; //治疗状态的字体大小
        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("技能字体颜色")]
        public Color abilityColor; //技能状态的字体颜色

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("技能字体大小")]
        public int abilityFontSize = 16; //技能状态的字体大小

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("字体移动速度")]
        public float moveSpeed = 30f; //字体的移动速度

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("字体消失时间")]
        public float disappearTime = 1f; //字体的消失时间

        [FoldoutGroup("伤害跳字细节设置")]
        [HideLabel]
        [LabelText("字体消失速度")]
        public float disappearSpeed = 3f; //字体的消失速度
    }
}