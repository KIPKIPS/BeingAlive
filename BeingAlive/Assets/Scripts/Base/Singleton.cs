using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new() {
    private static T _instance;

    /// <summary>
    /// 单例静态对象
    /// </summary>
    /// <value></value>
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = new T();
            }
            return _instance;
        }
    }
}
