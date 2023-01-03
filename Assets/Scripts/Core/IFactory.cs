using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所以工厂模式的要继承这个接口
public interface IFactory<T>
{
   T Create();
}
