using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttribute : ICharacterAttribute
{

    private Enum_Star star = Enum_Star.none;
    public Enum_Star Star{
        set{
            star = value;
        }   
        get{
            return star;
        }
    }

    public override void Init(EntityData data){
        Star = (data as AllyData).star;
        base.Init(data);
    }

}
