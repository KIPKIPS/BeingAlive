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
        foreach (GameObject item in Selection.gameObjects) {
            string path = AssetDatabase.GetAssetPath(item);
            if (!path.EndsWith(".prefab"))
                return;
            //Debug.Log(path);
            string pattern = @"Resources/[\S]+/?";
            string relativePath = "";
            foreach (Match match in Regex.Matches(path, pattern)) {
                relativePath = match.ToString().Replace("Resources/", "").Replace(".prefab", "").ToString();//获取相对路径
            }
            BasePanel panel = item.GetComponent<BasePanel>();
            int id = panel.id;
            PanelData panelData = new PanelData(id, relativePath, item.name);
            StoragePanelData(panelData);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    static void StoragePanelData(PanelData panelData) {
        //将对象转化为字符串
        List<PanelData> list = UIManager.Instance.PanelDataList;
        bool isExist = false;
        foreach (PanelData data in list) {
            if (data.id == panelData.id) {
                data.name = panelData.name;
                data.path = panelData.path;
                isExist = true;
                break;
            }
        }
        if (!isExist) {
            list.Add(panelData);
        }
        string filePath = Application.dataPath + "/Data" + "/PanelData.json";
        string jsonStr = JsonConvert.SerializeObject(list);
        //print(jsonStr);
        //将转换过后的json字符串写入json文件
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(jsonStr);//写入文件
        writer.Close();
        AssetDatabase.Refresh();
    }
}
