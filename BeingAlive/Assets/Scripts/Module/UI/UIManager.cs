using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager> {
    public override void Awake() {

    }
    // Start is called before the first frame update
    public override void Start() {
        AnalysisItemJsonData();
    }

    // Update is called once per frame
    public override void Update() {

    }

    public void OpenPanelById(int id) {

    }


    //解析面板的json数据
    private List<PanelData> panelDataList;

    public List<PanelData> PanelDataList {
        get {
            if (panelDataList == null) {
                AnalysisItemJsonData();
            }
            return panelDataList;
        }
    }
    public void AnalysisItemJsonData() {
        panelDataList = new List<PanelData>();
        List<JObject> jObjList = Utils.LoadJsonByPath<List<JObject>>("Data/PanelData.json");
        if (jObjList != null) {
            foreach (JObject jObj in jObjList) {
                //解析字段
                int id = int.Parse(jObj.GetValue("id").ToString());
                string path = jObj.GetValue("path").ToString();
                string name = jObj.GetValue("name").ToString();
                PanelData panelData = new PanelData(id, path, name);
                panelDataList.Add(panelData);
            }
        }

    }
}
