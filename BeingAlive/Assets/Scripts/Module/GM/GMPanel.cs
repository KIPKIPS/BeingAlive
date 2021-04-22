using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMPanel : BasePanel {

    public override void Awake() {

        fadeInOutTime = 0.4f;
        scaleTime = 0.4f;
        showTween = true;
        scaleTrs = FindObj<Transform>("Main");
        base.Awake();
    }

    public override void InitPanel() {
        base.InitPanel();
        FindObj<Button>("Bg", transform, delegate () {
            base.OnExit();
        });
    }
    public override void OnEnter() {
        base.OnEnter();
    }
    // Start is called before the first frame update
}
