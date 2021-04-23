using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel {
    public override void Awake() {
        base.Awake();
        panelType = UIManager.PanelType.Module;
        showTween = false;
    }

    public override void InitPanel() {
        base.InitPanel();
    }
}
