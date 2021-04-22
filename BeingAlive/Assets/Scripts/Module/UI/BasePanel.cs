// author:KIPKIPS
// time:2020.12.05 01:43:52
// describe:BasePanel UI面板的基类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BasePanel : MonoBehaviour {
    public int id;
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
        InitPanel();
    }
    public virtual void InitPanel() {
        if (scaleTrs == null) {
            scaleTrs = transform;
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
        return Utils.FindObj<T>(this.transform, name);
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

    //界面生命周期流程,这里只提供虚方法,具体的逻辑由各个业务界面进行重写

    //进入界面
    public virtual void OnEnter() {
        print("on enter" + id);
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
        print("on resume");
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
