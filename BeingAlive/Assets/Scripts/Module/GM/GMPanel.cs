using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMPanel : BasePanel {

    public override void Awake() {
        base.Awake();
        panelType = UIManager.PanelType.Module;
        fadeInOutTime = 0.4f;
        scaleTime = 0.4f;
        showTween = true;
        scaleTrs = FindObj<Transform>("Main");

    }

    public override void InitPanel() {
        base.InitPanel();
        FindObj<Button>("Bg", this.transform, delegate () {
            base.OnExit();
        });
    }
    // Start is called before the first frame update
}
