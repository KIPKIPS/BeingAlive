using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager> {
    public List<JObject> LoadConfigTableByName(string tabName) {
        return Utils.LoadJsonByPath<List<JObject>>("Data/" + tabName + ".json");
    }

    private class ColorData {
        public int index;
        public string hexColorCode;

        public ColorData(int _index, string _hexColorCode) {
            index = _index;
            hexColorCode = _hexColorCode;
        }
    }

    public void AnalysisConfigTableByName(string tabName) {
        List<JObject> jObjList = LoadConfigTableByName(tabName);
        switch (tabName) {
            case "ColorData":
                AnalysisColorData(jObjList); break;
        }
    }

    private Dictionary<int, string> colorDict;//十六进制的颜色字典

    private string[] loadConfigTableList = {
        "ColorData",
    };

    public Dictionary<int, string> ColorDict {
        get {
            if (colorDict == null) {
                colorDict = new Dictionary<int, string>();
            }
            return colorDict;
        }
    }
    public override void Awake() {
        base.Awake();
        foreach (string tabName in loadConfigTableList) {
            AnalysisConfigTableByName(tabName);
        }
    }

    public void AnalysisColorData(List<JObject> jObjList) {
        foreach (JObject jObj in jObjList) {
            int index = (int)jObj.GetValue("index");
            string hexColorCode = jObj.GetValue("hexColorCode").ToString();
            if (!ColorDict.ContainsKey(index)) {
                ColorDict.Add(index, hexColorCode);
            }
        }
    }
}
