using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AbilityIndicator: MonoBehaviour
{
    public AbilityIndicatorData data;

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("游戏Object")] [SerializeField]
    private UnityEngine.GameObject obj = null; //游戏画面显示的obj,会不断的显示和隐藏
    
    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("角色")] 
    public ICharacter character;

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("技能")]
    public AbilityData abilityData;

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("角色坐标")] [SerializeField]
    private Vector3 characterPos;

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("技能范围")] [SerializeField]
    private float abilityRange; //技能范围

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("是否显示")] [SerializeField]
    private bool isShow = false;

    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("当前显示的技能序号")] 
    public int index = 0;

    private float initialScale = 0.84f*2; //默认scale为0.84*2以确保技能指示器的单位长度为1,直径为2
    private void Start() {
        character = gameObject.GetComponent<ICharacter>();
        Init(character);
    }
    
    

    private void Init(ICharacter character){
        if(character.GetCharacterAbility() == null || character.GetCharacterAbility().List_Ability == null || character.GetCharacterAbility().List_Ability.Count == 0){
            Debug.LogWarning(character.Name+"没有技能");
            this.enabled = true;
            return;
        }
        if(character.GetCharacterAbility().List_Ability[index] == null){
            Debug.LogWarning(character.Name+"的当前技能序号的技能不存在");
            this.enabled = true;
            return;
        }
        abilityData = character.GetCharacterAbility().List_Ability[index].abilityData;
        //data = Resources.Load("Data/InfoUI/AbilityIndicatorData") as AbilityIndicatorData;
    }

    

    private void Update() {
        
        if(obj == null){
            return;
        }
        abilityRange = abilityData.targetChooser.range;
        characterPos = character.GetPosition();
        if(isShow){
            obj.transform.position = characterPos;
            obj.transform.localScale = new Vector3(abilityRange*initialScale, abilityRange*initialScale, 1);
        }
    }

    private void Show(){
        isShow = true;
        if(obj == null){
            obj = Instantiate(data.prefab); //按照prefab实例化一个obj
        }
        else{
            obj.SetActive(true);
        }
    }

    private void Hide(){
        isShow = false;
        if(obj!=null){
            obj.SetActive(false);
        }   
        else{
            Debug.LogWarning("Hide(): AbilitySelecter does not exist so that cannot be hide");
        }
    }

    [Button ("SwitchChange")]
    public void SwitchChange(){
        if(isShow){
            Hide();
        }
        else{
            Show();
        }
    }
}
