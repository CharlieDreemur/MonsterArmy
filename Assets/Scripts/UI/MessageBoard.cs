using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoard : Singleton<MessageBoard>
{
    private string currentText;
    public Text messageText;
    public bool isData;
    public string AddMessage(string content){
        string prefix = null;
        if(isData){
            prefix = "["+System.DateTime.Now.ToString("HH;mm:ss")+"]";
        }
        if(currentText == null){
            currentText = prefix + content;
        }
        currentText = currentText+ "\n"+ prefix + content ;
        messageText.text = currentText;
        return currentText;
    }
}
