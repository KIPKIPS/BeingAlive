using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour
    where T : Component {
    private static T _instance;

    /// <summary>
    /// 单例静态对象
    /// </summary>
    /// <value></value>
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = (T)obj.AddComponent(typeof(T));
                }
            }
            return _instance;
        }
    }
    public virtual void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null) {
            _instance = this as T;
        } else {
            Destroy(gameObject);
        }
    }
    public virtual void Start() {

    }

    // Update is called once per frame
    public virtual void Update() {
    }
}
