using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoSingleton<ModuleManager> {
    private string[] initManager = {
        "UIManager",
        "DataManager",
    };
    public override void Awake() {
        InstantiateManager();
    }

    public void InstantiateManager() {
        foreach (string managerName in initManager) {
            GameObject managerObj = new GameObject(managerName);
            //managerObj.name
            managerObj.transform.localScale = Vector3.one;
            managerObj.transform.localRotation = Quaternion.identity;
            managerObj.transform.localPosition = Vector3.zero;
            Type type = Type.GetType(managerName);
            managerObj.AddComponent(type);
        }
    }
}
