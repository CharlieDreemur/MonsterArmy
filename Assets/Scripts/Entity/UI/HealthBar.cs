using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HealthBar : MonoBehaviour
{
    public HealthBarData data;
    public ICharacter character;
    [SerializeField]
    private UnityEngine.GameObject healthBar;
    [SerializeField]
    private HealthBarSetup setup;
    private void Start() {
        character = gameObject.GetComponent<ICharacter>();
        
        if(character==null){
            return;
        }
        healthBar = data.Create();
        healthBar.transform.SetParent(character.transform, false);
        setup = healthBar.GetComponent<HealthBarSetup>();
        setup.Init(character);
    
    }

    public void Update(){
        if(character==null){
            return;
        }
        setup.Update();
    }
}
