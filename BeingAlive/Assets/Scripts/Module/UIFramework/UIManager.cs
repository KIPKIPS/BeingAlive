using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager> {
    //属性字段
    public enum PanelId {
        Gm = 1,
        Demo = 2,
        LoginPanel = 3,
        CreateRolePanel = 4,
    }
    public enum PanelType {
        Module = 1,
        Pop = 2,
        Tips = 3,
    }
    //数据存储域
    private Stack<BasePanel> panelStack;
    public Stack<BasePanel> PanelStack {
        get {
            if (panelStack == null) {
                panelStack = new Stack<BasePanel>();
            }

            return panelStack;
        }
    }
    private Dictionary<int, BasePanel> basePanelDict;
    public Dictionary<int, BasePanel> BasePanelDict {
        get {
            if (basePanelDict == null) {
                basePanelDict = new Dictionary<int, BasePanel>();
            }
            return basePanelDict;
        }
    }
    //解析面板的json数据,PanelData列表
    private List<PanelData> panelDataList;
    public List<PanelData> PanelDataList {
        get {
            if (panelDataList == null) {
                panelDataList = new List<PanelData>();
            }
            return panelDataList;
        }
    }

    public Dictionary<int, PanelData> panelDict;
    public Dictionary<int, PanelData> PanelDict {
        get {
            if (panelDict == null) {
                panelDict = new Dictionary<int, PanelData>();
            }
            return panelDict;
        }
    }

    //Canvas区域
    private Canvas moduleCanvas;
    public Transform ModuleCanvas {
        get {
            if (moduleCanvas == null) {
                moduleCanvas = GameObject.Find("ModuleCanvas").GetComponent<Canvas>();
            }
            return moduleCanvas.transform;
        }
    }
    private Canvas popCanvas;
    public Transform PopCanvas {
        get {
            if (popCanvas == null) {
                popCanvas = GameObject.Find("PopCanvas").GetComponent<Canvas>();
            }
            return popCanvas.transform;
        }
    }
    private Canvas tipsCanvas;
    public Transform TipsCanvas {
        get {
            if (tipsCanvas == null) {
                tipsCanvas = GameObject.Find("TipsCanvas").GetComponent<Canvas>();
            }
            return tipsCanvas.transform;
        }
    }

    //方法
    public override void Awake() {
        base.Awake();
        AnalysisItemJsonData();
    }
    public override void Start() {
        base.Start();
        OpenPanelById(PanelId.LoginPanel);
    }

    public override void Update() {
        base.Update();
        // if (Input.GetKeyDown(KeyCode.G)) {
        //     OpenPanelById(PanelId.Gm);
        // }
        // if (Input.GetKeyDown(KeyCode.K)) {
        //     OpenPanelById(PanelId.Demo);
        // }
    }

    public void OpenPanelById(PanelId panelId) {
        int id = (int)panelId;
        PanelStackPush(id);
    }

    public BasePanel GetPanelById(int id) {
        BasePanel panel;
        BasePanelDict.TryGetValue(id, out panel);
        if (panel != null) {
            return panel;
        } else {
            // print("new");
            string path = panelDict[id].path;
            GameObject panelObj = Instantiate(Resources.Load<GameObject>(path), ModuleCanvas);
            panelObj.name = panelDict[id].name;
            panel = panelObj.transform.GetComponent<BasePanel>();
            BasePanelDict.Add(id, panel);
            return panel;
        }
    }

    public void PanelStackPush(int panelId) {
        BasePanel panel = GetPanelById(panelId);
        //print(panel.panelType);
        if (panel.IsShow) {
            return;
        }
        //显示当前界面时,应该先去判断当前栈是否为空,不为空说明当前有界面显示,把当前的界面OnPause掉
        if (PanelStack.Count > 0) {
            PanelStack.Peek().OnPause();
        }
        //每次入栈(显示页面的时候),触发panel的OnEnter方法
        panel.OnEnter();
        PanelStack.Push(panel);
    }

    public void PanelStackPop() {
        if (PanelStack.Count > 0) {
            //print("栈不为空");
            PanelStack.Pop();//.OnExit();//关闭栈顶界面
            if (PanelStack.Count <= 0) {
                //print("栈空");
                return;
            } else {
                //print("last panel resume");
                PanelStack.Peek().OnResume();//恢复原先的界面
            }
        } else {
            return;
        }
    }

    public void AnalysisItemJsonData() {
        List<JObject> jObjList = Utils.LoadJsonByPath<List<JObject>>("Data/PanelData.json");
        if (jObjList != null) {
            foreach (JObject jObj in jObjList) {
                //解析字段
                int id = int.Parse(jObj.GetValue("id").ToString());
                string path = jObj.GetValue("path").ToString();
                string name = jObj.GetValue("name").ToString();
                //PanelType panelType = (PanelType)Enum.Parse(typeof(PanelType), jObj.GetValue("panelType").ToString());
                PanelData panelData = new PanelData(id, path, name);//, panelType
                PanelDataList.Add(panelData);
            }
        }

        foreach (PanelData panelData in PanelDataList) {
            PanelDict.Add(panelData.id, panelData);
        }

    }
}
