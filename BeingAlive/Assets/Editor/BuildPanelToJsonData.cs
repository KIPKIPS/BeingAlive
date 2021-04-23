using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


//编辑器作用对象
[CustomEditor(typeof(BuildPanelToJsonData))]
public class BuildPanelToJsonData : Editor {

    [MenuItem("Tools/BulidPanel", false, 3)]
    [MenuItem("Assets/BulidPanel", false)]
    static void BulidPanelToJsonData() {
        //Debug.Log("BulidPanelToJsonData...");
        EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);//进度条

        if (Selection.gameObjects.Length < 1) {
            Debug.LogWarning("Select Nothing");
            return;
        }
        List<JObject> jObjList = Utils.LoadJsonByPath<List<JObject>>("Data/PanelData.json");
        List<PanelData> dataList = new List<PanelData>();
        if (jObjList != null) {
            foreach (JObject jObj in jObjList) {
                int id = int.Parse(jObj.GetValue("id").ToString());
                string path = jObj.GetValue("path").ToString();
                string name = jObj.GetValue("name").ToString();
                PanelData pd = new PanelData(id, path, name);
                dataList.Add(pd);
                //PanelType panelType = (PanelType)Enum.Parse(typeof(PanelType), jObj.GetValue("panelType").ToString());
            }
        }
        foreach (GameObject item in Selection.gameObjects) {
            string path = AssetDatabase.GetAssetPath(item);
            if (!path.EndsWith(".prefab")) {
                return;
            }
            //Debug.Log(path);
            string pattern = @"Resources/[\S]+/?";
            string relativePath = "";
            foreach (Match match in Regex.Matches(path, pattern)) {
                relativePath = match.ToString().Replace("Resources/", "").Replace(".prefab", "").ToString();//获取相对路径
            }
            BasePanel panel = item.GetComponent<BasePanel>();
            int id = (int)panel.PanelId;
            PanelData panelData = new PanelData(id, relativePath, item.name);//panelType
            bool isExit = false;
            for (int i = 0; i < dataList.Count; i++) {
                if (dataList[i].id == id) {
                    dataList[i] = panelData;
                    isExit = true;
                    break;
                }
            }

            if (!isExit) {
                dataList.Add(panelData);
            }
            //UIManager.PanelType panelType = panel.panelType;

            // StoragePanelData(panelData);
        }
        //Debug.Log(dataList.Count);
        string filePath = Application.dataPath + "/Data" + "/PanelData.json";
        string jsonStr = JsonConvert.SerializeObject(dataList);
        //Debug.Log(jsonStr);
        //将转换过后的json字符串写入json文件
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(jsonStr);//写入文件
        writer.Close();
        //AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
