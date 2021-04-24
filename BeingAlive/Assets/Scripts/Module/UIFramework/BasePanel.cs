// author:KIPKIPS
// time:2020.12.05 01:43:52
// describe:BasePanel UI面板的基类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using UnityEngine.Events;

public class BasePanel : MonoBehaviour {
    //public int id;
    public UIManager.PanelId PanelId;
    [System.NonSerialized]
    public UIManager.PanelType panelType;
    //public string path;
    protected Transform scaleTrs;
    private bool isShow = false;
    public bool IsShow {
        get { return isShow; }
        set { isShow = value; }
    }

    private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup {
        get {
            if (canvasGroup == null) {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }
    public float fadeInOutTime = 0.3f;//渐隐渐显time
    public float scaleTime = 0.3f;//缩放time
    public bool showTween = false;
    public virtual void Awake() {

    }
    public virtual void Start() {
    }
    public virtual void InitPanel() {
        if (scaleTrs == null) {
            scaleTrs = this.transform;
        }
        if (showTween) {
            scaleTrs.localScale = Vector3.zero;
            CanvasGroup.alpha = 0;

        } else {
            scaleTrs.localScale = Vector3.one;
            CanvasGroup.alpha = 1;
        }
    }
    // Update is called once per frame
    public virtual void Update() {

    }

    //FindObj接口列表
    public T FindObj<T>(string name) {
        return Utils.FindObj<T>(this.transform, name);
    }
    public T FindObj<T>(string name, Transform trs) {
        Transform root = trs == null ? this.transform : trs;
        return Utils.FindObj<T>(root, name);
    }
    public T FindObj<T>(string name, Transform trs, Action func) {
        Transform root = trs == null ? this.transform : trs;
        T resObj = Utils.FindObj<T>(trs, name);
        if (typeof(T) == typeof(Button)) { //button支持绑定函数方法
            (resObj as Button).onClick.AddListener(delegate () {
                func();
            });
        }
        return resObj;
    }

    public T FindObj<T>(string name, Transform trs, UnityAction<BaseEventData> mouseEnter, UnityAction<BaseEventData> mouseExit) {
        Transform root = trs == null ? this.transform : trs;
        T resObj = Utils.FindObj<T>(trs, name);
        if (typeof(T) == typeof(Button)) { //button支持绑定函数方法
            Transform btnTrs = (resObj as Button).transform;
            var trigger = btnTrs.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) {
                trigger = btnTrs.gameObject.AddComponent<EventTrigger>();
            }
            // 实例化delegates(trigger.trigger是注册在EventTrigger组件上的所有功能)  
            trigger.triggers = new List<EventTrigger.Entry>();
            // 在EventSystem委托列表中进行登记  
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            // 设置 事件类型  
            enterEntry.eventID = EventTriggerType.PointerEnter;
            // 实例化回调函数  
            enterEntry.callback = new EventTrigger.TriggerEvent();
            //UnityAction 本质上是delegate，且有数个泛型版本（参数最多是四个），一个UnityAction可以添加多个函数（多播委托）  
            //将方法绑定在回调上（给回调方法添加监听）  
            enterEntry.callback.AddListener(mouseEnter);
            // 添加事件触发记录到GameObject的事件触发组件  
            trigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            // 设置 事件类型  
            exitEntry.eventID = EventTriggerType.PointerExit;
            // 实例化回调函数  
            exitEntry.callback = new EventTrigger.TriggerEvent();
            //将方法绑定在回调上（给回调方法添加监听）  
            exitEntry.callback.AddListener(mouseExit);
            // 添加事件触发记录到GameObject的事件触发组件  
            trigger.triggers.Add(exitEntry);
        }
        return resObj;
    }

    //界面生命周期流程,这里只提供虚方法,具体的逻辑由各个业务界面进行重写

    //进入界面
    public virtual void OnEnter() {
        InitPanel();
        //print("on enter" + PanelId);
        if (showTween) {
            scaleTrs.DOScale(Vector3.one, scaleTime).SetEase(Ease.InOutBack).OnComplete(() => scaleTrs.localScale = Vector3.one);
            CanvasGroup.DOFade(1, fadeInOutTime).SetEase(Ease.InOutBack).OnComplete(() => CanvasGroup.alpha = 1);
        }
        CanvasGroup.blocksRaycasts = true;
        isShow = true;
    }

    //暂停界面
    public virtual void OnPause() {
        CanvasGroup.blocksRaycasts = false;//弹出新的面板时,鼠标和这个界面不再进行交互(禁用射线检测)
        isShow = false;
    }

    //恢复界面
    public virtual void OnResume() {
        CanvasGroup.blocksRaycasts = true;
        //print("on resume");
        isShow = true;
    }

    //关闭界面
    public virtual void OnExit() {
        if (showTween) {
            CanvasGroup.DOFade(0, fadeInOutTime).SetEase(Ease.InOutBack).OnComplete(() => CanvasGroup.alpha = 0);
            scaleTrs.DOScale(Vector3.zero, scaleTime).SetEase(Ease.InOutBack).OnComplete(() => scaleTrs.localScale = Vector3.zero);
        }
        CanvasGroup.blocksRaycasts = false;
        isShow = false;
        UIManager.Instance.PanelStackPop();
    }
}
