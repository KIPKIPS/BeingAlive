using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

//
public static class Utils {
    /// <summary>
    /// 洗牌算法,传入目标列表,返回打乱顺序之后的列表,支持泛型
    /// </summary>
    /// <param name="list">打乱的目标列表</param>
    /// <param name="seed">随机种子</param>
    /// <typeparam name="T">目标列表类型</typeparam>
    /// <returns></returns>
    public static T[] ShuffleCoords<T>(T[] list, int seed) {
        System.Random random = new System.Random(seed);//根据随机种子获得一个随机数
        //遍历并随机交换
        for (int i = 0; i < list.Length - 1; i++) {
            int randomIndex = random.Next(i, list.Length);//返回一个随机的索引
            T tempItem = list[randomIndex];//swap操作
            list[randomIndex] = list[i];
            list[i] = tempItem;
        }
        return list;
    }

    /// <summary>
    /// 查找对象
    /// </summary>
    /// <param name="root"></param>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindObj<T>(Transform root, string name) {
        if (name == null) {
            T res = root.GetComponent<T>();
            return res;
        } else {
            Transform target = GetChild(root, name);
            T res = target.GetComponent<T>();
            return res;
        }
    }

    public static T FindObj<T>(string name) {
        Transform target = GameObject.Find(name).transform;
        T res = target.GetComponent<T>();
        return res;
    }

    /// <summary>
    /// 递归查找父节点下的对象
    /// </summary>
    /// <param name="root">根节点</param>
    /// <param name="childName">目标名称</param>
    /// <returns></returns>
    private static Transform GetChild(Transform root, string childName) {
        //根节点查找
        Transform childTF = root.Find(childName);
        if (childTF != null) {
            return childTF;
        }
        //遍历子物体查找
        int count = root.childCount;
        for (int i = 0; i < count; i++) {
            childTF = GetChild(root.GetChild(i), childName);
            if (childTF != null) {
                return childTF;
            }
        }
        return null;
    }
    //AnalysisJson
    public static T LoadJsonByPath<T>(string path) {
        string filePath = Application.dataPath + "/" + path;
        //print(filePath);
        //读取文件
        StreamReader reader = new StreamReader(filePath);
        string jsonStr = reader.ReadToEnd();
        reader.Close();
        //Debug.Log(jsonStr);
        //字符串转换为DataSave对象
        T data = JsonConvert.DeserializeObject<T>(jsonStr);
        return data;
    }

    public static string AddColor(string str, int colorIndex) {
        //其中ColorHelper.GetColor(color) 返回十六进制格式的颜色对象,color参数传入色码号即可,色码号在配置表可以看到
        //返回一个类似html标签语言包装好颜色信息的字符串,Unity的Text组件可以解析此串中的颜色信息
        return string.Format("<color={0}>{1}</color>", GetColor(colorIndex), str);

    }
    public static string GetColor(int colorIndex) {
        return DataManager.Instance.ColorDict[colorIndex];
        //return string.Format("#{0}", DataColor.data_get[tonumber(color)].color);
    }
}
