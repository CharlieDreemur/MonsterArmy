using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArmy : Singleton<MonsterArmy>
{
    //游戏主系统，控制其他所有的系统
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
    
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            MessageBoard.Instance.AddMessage("测试消息");
        }
    }

    void OnApplicationQuit() {
        //退出软件时调用
    }
}
