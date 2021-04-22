using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoPanel : BasePanel {
    public override void Awake() {

        showTween = true;
        fadeInOutTime = 0.4f;
        scaleTime = 0.4f;
        scaleTrs = FindObj<Transform>("Main");
        base.Awake();
    }

    public override void InitPanel() {
        base.InitPanel();
        FindObj<Button>("Bg", transform, delegate () { base.OnExit(); });
    }

    public override void OnEnter() {
        base.OnEnter();
    }
}
